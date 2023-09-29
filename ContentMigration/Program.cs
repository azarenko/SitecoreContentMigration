using CommandLine;
using ContentMigration;
using ContentMigration.Migration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var serviceProvider = new ServiceCollection()
        .AddSingleton<IConfiguration>(configuration)
        .AddDbContext<SourceDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("SourceDb"), options => options.CommandTimeout(360)))
        .AddDbContext<TargetDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TargetDb"), options => options.CommandTimeout(360)))
        .BuildServiceProvider();


Parser.Default.ParseArguments<ParamModel>(args)
              .WithParsed<ParamModel>(o =>
              {
                  new BlobMigration(serviceProvider).Run(o.Overwrite);

                  foreach (var id in o.Item.Split(','))
                  {
                      new ItemMigration(serviceProvider).Run(Guid.Parse(id)
                                  , o.Languages
                                  , o.Recursive
                                  , o.Overwrite);
                  }
              });

Console.WriteLine("Completed!");