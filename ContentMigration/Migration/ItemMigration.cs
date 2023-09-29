using Microsoft.Extensions.DependencyInjection;

namespace ContentMigration.Migration
{
    internal class ItemMigration : BaseMigration
    {
        public ItemMigration(ServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override void Run(Guid itemId, string languages, bool recursive, bool overwrite)
        {
            List<Items> childs = new List<Items>();

            using (var scope = _serviceProvider.CreateScope())
            using (var sourceDbContext = scope.ServiceProvider.GetRequiredService<SourceDbContext>())
            using (var targetDbContext = scope.ServiceProvider.GetRequiredService<TargetDbContext>())
            {
                var sourceItem = sourceDbContext.Items.FirstOrDefault(i => i.ID == itemId);

                if (sourceItem == null)
                    throw new Exception($"Source item {itemId} {sourceItem.Name} not exist");

                Log($"Start importing item {itemId} {sourceItem.Name}", ConsoleColor.White);

                // Make sure we have a parent for current item
                var targetParentItem = targetDbContext.Items.FirstOrDefault(i => i.ID == sourceItem.ParentID);

                if (targetParentItem == null)
                {
                    new ItemMigration(_serviceProvider).Run(sourceItem.ParentID, languages, recursive, overwrite);
                }

                var targetItem = targetDbContext.Items.FirstOrDefault(i => i.ID == itemId);

                if (targetItem != null)
                {

                    if ((targetItem.Updated == sourceItem.Updated) || !overwrite)
                    {
                        Log($"Item {itemId} similar in Source and Target databases. Skip importing", ConsoleColor.White);
                    }
                    else
                    {
                        // Update existing item
                        targetItem.Name = sourceItem.Name;
                        targetItem.Updated = sourceItem.Updated;
                        targetItem.ParentID = sourceItem.ParentID;
                        targetItem.TemplateID = sourceItem.TemplateID;
                        targetItem.MasterID = sourceItem.MasterID;
                        targetItem.Created = sourceItem.Created;

                        targetDbContext.SaveChanges();

                        Log($"Item {itemId} updated", ConsoleColor.Yellow);
                    }
                }
                else
                {
                    // Import new item
                    var newTargetItem = new Items()
                    {
                        ID = sourceItem.ID,
                        Name = sourceItem.Name,
                        TemplateID = sourceItem.TemplateID,
                        MasterID = sourceItem.MasterID,
                        ParentID = sourceItem.ParentID,
                        Created = sourceItem.Created,
                        Updated = sourceItem.Updated
                    };

                    targetDbContext.Items.Add(newTargetItem);

                    targetDbContext.SaveChanges();

                    Log($"Item {itemId} added", ConsoleColor.Green);
                }

                // Import shared fields
                var sharedFields = sourceDbContext.SharedFields.Where(sf => sf.ItemId == itemId);

                foreach (var sharedField in sharedFields)
                {
                    var targetSharedField = targetDbContext.SharedFields.FirstOrDefault(tsf => tsf.ItemId == sharedField.ItemId && tsf.FieldId == sharedField.FieldId);

                    if (targetSharedField != null)
                    {
                        
                        if ((targetSharedField.Updated == sharedField.Updated) || !overwrite)
                        {
                            continue;
                        }
                        else
                        {
                            targetSharedField.Value = sharedField.Value;
                            targetSharedField.FieldId = sharedField.FieldId;
                            targetSharedField.Created = sharedField.Created;
                            targetSharedField.Updated = sharedField.Updated;

                            Log($"Shared field {sharedField.Id} updated", ConsoleColor.Yellow);
                        }
                    }
                    else
                    {
                        var newTargetSharedField = new SharedFields()
                        {
                            Id = sharedField.Id,
                            ItemId = sharedField.ItemId,
                            FieldId = sharedField.FieldId,
                            Value = sharedField.Value,
                            Created = sharedField.Created,
                            Updated = sharedField.Updated
                        };

                        targetDbContext.SharedFields.Add(newTargetSharedField);

                        Log($"Shared field {sharedField.Id} added", ConsoleColor.Green);
                    }
                }
                targetDbContext.SaveChanges();

                // Import unversioned fields
                IQueryable<UnversionedFields> unversionedFields;

                if (string.IsNullOrEmpty(languages))
                {
                    unversionedFields = sourceDbContext.UnversionedFields.Where(uf => uf.ItemId == itemId);
                }
                else
                {
                    var langList = new List<string>(languages.Split(','));

                    if (!langList.Contains("en"))
                    {
                        langList.Add("en");
                    }

                    unversionedFields = sourceDbContext.UnversionedFields.Where(uf => uf.ItemId == itemId && langList.Contains(uf.Language));
                }

                foreach (var unversionedField in unversionedFields)
                {
                    var targetunversionedField = targetDbContext.UnversionedFields.FirstOrDefault(tuf => tuf.ItemId == unversionedField.ItemId 
                    && tuf.FieldId == unversionedField.FieldId
                    && tuf.Language == unversionedField.Language);

                    if (targetunversionedField != null)
                    {
                        if ((targetunversionedField.Updated == unversionedField.Updated) || !overwrite)
                        {
                            continue;
                        }
                        else
                        {
                            targetunversionedField.Value = unversionedField.Value;
                            targetunversionedField.FieldId = unversionedField.FieldId;
                            targetunversionedField.Created = unversionedField.Created;
                            targetunversionedField.Updated = unversionedField.Updated;
                            targetunversionedField.Language = unversionedField.Language;

                            Log($"Unversioned language:{unversionedField.Language} field {unversionedField.Id} updated", ConsoleColor.Yellow);
                        }
                    }
                    else
                    {
                        var newTargetunversionedField = new UnversionedFields()
                        {
                            Id = unversionedField.Id,
                            ItemId = unversionedField.ItemId,
                            FieldId = unversionedField.FieldId,
                            Value = unversionedField.Value,
                            Created = unversionedField.Created,
                            Updated = unversionedField.Updated,
                            Language = unversionedField.Language
                        };

                        targetDbContext.UnversionedFields.Add(newTargetunversionedField);

                        Log($"Unversioned language:{unversionedField.Language} field {unversionedField.Id} added", ConsoleColor.Green);
                    }
                }
                targetDbContext.SaveChanges();

                // Import versioned fields
                IQueryable<VersionedFields> versionedFields;

                if (string.IsNullOrEmpty(languages))
                {
                    versionedFields = sourceDbContext.VersionedFields.Where(vf => vf.ItemId == itemId);
                }
                else
                {
                    var langList = new List<string>(languages.Split(','));

                    if (!langList.Contains("en"))
                    {
                        langList.Add("en");
                    }

                    versionedFields = sourceDbContext.VersionedFields.Where(vf => vf.ItemId == itemId && langList.Contains(vf.Language));
                }

                foreach (var versionedField in versionedFields)
                {
                    var targetversionedField = targetDbContext.VersionedFields.FirstOrDefault(tuf => tuf.ItemId == versionedField.ItemId
                    && tuf.FieldId == versionedField.FieldId
                    && tuf.Language == versionedField.Language
                    && tuf.Version == versionedField.Version);

                    if (targetversionedField != null)
                    {
                        if ((targetversionedField.Updated == versionedField.Updated) || !overwrite)
                        {
                            continue;
                        }
                        else
                        {
                            targetversionedField.Value = versionedField.Value;
                            targetversionedField.FieldId = versionedField.FieldId;
                            targetversionedField.Created = versionedField.Created;
                            targetversionedField.Updated = versionedField.Updated;
                            targetversionedField.Language = versionedField.Language;
                            targetversionedField.Version = versionedField.Version;

                            Log($"Versioned Version:{versionedField.Version} language:{versionedField.Language} field {versionedField.Id} updated", ConsoleColor.Yellow);
                        }
                    }
                    else
                    {
                        var newTargetversionedField = new VersionedFields()
                        {
                            Id = versionedField.Id,
                            ItemId = versionedField.ItemId,
                            FieldId = versionedField.FieldId,
                            Value = versionedField.Value,
                            Created = versionedField.Created,
                            Updated = versionedField.Updated,
                            Language = versionedField.Language,
                            Version = versionedField.Version
                        };

                        targetDbContext.VersionedFields.Add(newTargetversionedField);

                        Log($"Versioned Version:{versionedField.Version} language:{versionedField.Language} field {versionedField.Id} added", ConsoleColor.Green);
                    }
                }
                targetDbContext.SaveChanges();

                if (recursive)
                {
                    childs = sourceDbContext.Items.Where(d => d.ParentID == itemId).ToList();
                }
            }

            var options = new ParallelOptions { MaxDegreeOfParallelism = 30 };
            Parallel.ForEach(childs, options, child =>
            {
                new ItemMigration(_serviceProvider).Run(child.ID
                            , languages
                            , recursive
                            , overwrite);
            });

            Log($"Finish importing item {itemId}", ConsoleColor.White);
        }

        private void Log(string message, ConsoleColor color)
        {
            lock (Console.Out)
            {
                var initColor = Console.ForegroundColor;
                Console.ForegroundColor = color;

                Console.WriteLine(message);

                Console.ForegroundColor = initColor;
            }
        }
    }
}
