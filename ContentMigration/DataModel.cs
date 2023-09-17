using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentMigration
{
    public class AccessControl
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string EntityId { get; set; }
        [Required]
        [MaxLength(255)]
        public string TypeName { get; set; }
        [Required]
        public string AccessRules { get; set; }
    }

    public class Archive
    {
        [Key]
        public Guid ArchivalId { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        public Guid ParentId { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required]
        public string OriginalLocation { get; set; }
        [Required]
        public DateTime ArchiveDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string ArchivedBy { get; set; }
        [Required]
        [MaxLength(50)]
        public string ArchiveName { get; set; }
    }

    public class ArchivedFields
    {
        [Key]
        public Guid RowId { get; set; }
        [Required]
        public Guid ArchivalId { get; set; }
        [Required]
        public Guid VersionId { get; set; }
        [Required]
        public Guid FieldId { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
    }

    public class ArchivedItems
    {
        [Key]
        public Guid RowId { get; set; }
        [Required]
        public Guid ArchivalId { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        [Required]
        public Guid TemplateID { get; set; }
        [Required]
        public Guid MasterID { get; set; }
        [Required]
        public Guid ParentID { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
    }

    public class ArchivedVersions
    {
        [Key]
        public Guid VersionId { get; set; }
        [Required]
        public Guid ArchivalId { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        [MaxLength(50)]
        public string Language { get; set; }
        public int? Version { get; set; }
        [Required]
        public DateTime ArchivedDate { get; set; }
        [MaxLength(50)]
        public string ArchivedBy { get; set; }
    }

    public class Blobs
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid BlobId { get; set; }
        [Required]
        public int Index { get; set; }
        [Required]
        public byte[] Data { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }

    public class ClientData
    {
        [Key]
        [MaxLength(50)]
        public string SessionKey { get; set; }
        [Required]
        public string Data { get; set; }
        [Required]
        public DateTime Accessed { get; set; }
        [Required]
        public Guid ID { get; set; }
    }

    public class Descendants
    {
        [Column(Order = 0)]
        public Guid Ancestor { get; set; }
        [Column(Order = 1)]
        public Guid Descendant { get; set; }
    }

    public class EventQueue
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(256)]
        public string EventType { get; set; }
        [Required]
        [MaxLength(256)]
        public string InstanceType { get; set; }
        [Required]
        public string InstanceData { get; set; }
        [Required]
        [MaxLength(128)]
        public string InstanceName { get; set; }
        [Required]
        public int RaiseLocally { get; set; }
        [Required]
        public int RaiseGlobally { get; set; }
        [Required]
        [MaxLength(128)]
        public string UserName { get; set; }
        [Timestamp]
        public byte[] Stamp { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }

    public class History
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Category { get; set; }
        [Required]
        [MaxLength(50)]
        public string Action { get; set; }
        [Required]
        public Guid ItemId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ItemLanguage { get; set; }
        [Required]
        public int ItemVersion { get; set; }
        [Required]
        public string ItemPath { get; set; }
        [Required]
        [MaxLength(250)]
        public string UserName { get; set; }
        [Required]
        public string TaskStack { get; set; }
        [Required]
        public string AdditionalInfo { get; set; }
        [Required]
        public DateTime Created { get; set; }
    }

    public class IDTable
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(255)]
        public string Prefix { get; set; }
        [Required]
        [MaxLength(255)]
        public string Key { get; set; }
        public Guid ParentID { get; set; }
        [Required]
        [MaxLength(255)]
        public string CustomData { get; set; }
    }

    public class Items
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }
        public Guid TemplateID { get; set; }
        public Guid MasterID { get; set; }
        public Guid ParentID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Links
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(150)]
        public string SourceDatabase { get; set; }
        public Guid SourceItemID { get; set; }
        [Required]
        [MaxLength(50)]
        public string SourceLanguage { get; set; }
        public int SourceVersion { get; set; }
        public Guid SourceFieldID { get; set; }
        [Required]
        [MaxLength(150)]
        public string TargetDatabase { get; set; }
        public Guid TargetItemID { get; set; }
        [Required]
        [MaxLength(50)]
        public string TargetLanguage { get; set; }
        public string TargetPath { get; set; } // ntext in SQL translates to string in C#
        public int TargetVersion { get; set; }
    }

    public class Notifications
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Language { get; set; }
        public int Version { get; set; }
        public bool Processed { get; set; }
        [Required]
        [MaxLength(256)]
        public string InstanceType { get; set; }
        public string InstanceData { get; set; } // nvarchar(max) is represented as string
    }

    public class Properties
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        [MaxLength(256)]
        public string Key { get; set; }
        public string Value { get; set; } // ntext in SQL is represented as string
    }

    public class PublishQueue
    {
        [Key]
        public Guid ID { get; set; }
        public Guid ItemID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Language { get; set; }
        public int Version { get; set; }
        [Required]
        [MaxLength(50)]
        public string Action { get; set; }
        public DateTime Date { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Index { get; set; }
    }

    public class Shadows
    {
        [Key]
        public Guid ID { get; set; }
        public Guid ProxyID { get; set; }
        public Guid TargetID { get; set; }
        public Guid ShadowID { get; set; }
    }

    public class SharedFields
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid FieldId { get; set; }
        public string Value { get; set; } // nvarchar(max) is represented as string
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class Tasks
    {
        [Key]
        public Guid ID { get; set; }
        public DateTime NextRun { get; set; }
        [Required]
        [MaxLength(500)]
        public string TaskType { get; set; }
        public string Parameters { get; set; } // ntext in SQL is represented as string
        [Required]
        [MaxLength(200)]
        public string Recurrence { get; set; }
        public Guid ItemID { get; set; }
        [Required]
        [MaxLength(50)]
        public string Database { get; set; }
        public int Pending { get; set; }
        public int Disabled { get; set; }
        [MaxLength(128)]
        public string InstanceName { get; set; }
    }

    public class UnversionedFields
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Language { get; set; }
        public Guid FieldId { get; set; }
        public string Value { get; set; } // nvarchar(max) is represented as string
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class VersionedFields
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        [Required]
        [MaxLength(50)]
        public string Language { get; set; }
        public int Version { get; set; }
        public Guid FieldId { get; set; }
        public string Value { get; set; } // nvarchar(max) is represented as string
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }

    public class WorkflowHistory
    {
        [Key]
        public Guid ID { get; set; }
        public Guid ItemID { get; set; }
        [Required]
        [MaxLength(20)]
        public string Language { get; set; }
        public int Version { get; set; }
        [Required]
        [MaxLength(200)]
        public string OldState { get; set; }
        [Required]
        [MaxLength(200)]
        public string NewState { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Text { get; set; }
        [Required]
        [MaxLength(100)]
        public string User { get; set; }
        public DateTime Date { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sequence { get; set; }
    }
}
