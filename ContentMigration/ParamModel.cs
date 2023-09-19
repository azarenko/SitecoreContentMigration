using CommandLine;
using System.Threading.Tasks;

namespace ContentMigration
{
    internal class ParamModel
    {
        [Option(longName:"item", Required = true)]
        public Guid Item { get; set; }

        [Option(longName: "lang", Default = null)]
        public string Languages { get; set; }

        [Option(longName: "overwrite", Default = false)]
        public bool Overwrite { get; set; }

        [Option(longName: "recursive", Default = true)]
        public bool Recursive { get; set; }

        [Option(longName: "importtemplates", Default = false)]
        public bool ImportTemplates { get; set; }

        [Option(longName: "importblobs", Default = false)]
        public bool ImportBlobs { get; set; }
    }
}
