using Microsoft.Extensions.DependencyInjection;

namespace ContentMigration.Migration
{
    internal abstract class BaseMigration
    {
        protected readonly ServiceProvider _serviceProvider;

        public BaseMigration(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public virtual void Run(Guid itemId, string languages, bool recurcive, bool overwrite)
        { }

        public virtual void Run(bool overwrite)
        { }
    }
}
