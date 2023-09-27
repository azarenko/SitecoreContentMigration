using Microsoft.Extensions.DependencyInjection;

namespace ContentMigration.Migration
{
    internal class BlobMigration : BaseMigration
    {
        public BlobMigration(ServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override void Run(bool overwrite)
        {
            Console.WriteLine("Start blob importing");

            using (var scope = _serviceProvider.CreateScope())
            using (var sourceDbContext = scope.ServiceProvider.GetRequiredService<SourceDbContext>())
            using (var targetDbContext = scope.ServiceProvider.GetRequiredService<TargetDbContext>())
            {
                foreach (var srcBlob in sourceDbContext.Blobs)
                {
                    var trgBlob = targetDbContext.Blobs.FirstOrDefault(x => x.BlobId == srcBlob.BlobId && x.Index == srcBlob.Index && x.Id == srcBlob.Id);
                    if (trgBlob != null)
                    {
                        if (overwrite)
                        {
                            Console.WriteLine($"Overwrite blob item {trgBlob.Id}");
                            trgBlob.Data = srcBlob.Data;
                        }
                    }
                    else
                    {
                        var newTrgBlob = new Blobs()
                        {
                            Id = srcBlob.Id,
                            Index = srcBlob.Index,
                            BlobId = srcBlob.Id,
                            Data = srcBlob.Data,
                            Created = srcBlob.Created
                        };

                        Console.WriteLine($"Add blob item {trgBlob.Id}");
                        targetDbContext.Blobs.Add(newTrgBlob);
                    }
                }

                targetDbContext.SaveChanges();
            }

            Console.WriteLine("Finish blob importing");
        }
    }
}
