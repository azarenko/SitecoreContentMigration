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
            options.UseSqlServer(configuration.GetConnectionString("SourceDb")))
        .AddDbContext<TargetDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("TargetDb")))
        .BuildServiceProvider();


Parser.Default.ParseArguments<ParamModel>(args)
              .WithParsed<ParamModel>(o =>
              {
                  new ItemMigration(serviceProvider).Run(o.Item
                              , o.Languages
                              , o.Recursive
                              , o.ImportTemplates
                              , o.ImportBlobs
                              , o.Overwrite);
              });

Console.WriteLine("Completed!");