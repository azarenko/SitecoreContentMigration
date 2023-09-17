using ContentMigration;
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


using (var scope = serviceProvider.CreateScope())
using (var sourceContext = scope.ServiceProvider.GetRequiredService<SourceDbContext>())
using (var targetContext = scope.ServiceProvider.GetRequiredService<TargetDbContext>())
{
    var count = sourceContext.Items.Count();
}


Console.WriteLine("Completed!");