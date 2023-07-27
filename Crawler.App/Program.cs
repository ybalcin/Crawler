// See https://aka.ms/new-console-template for more information

using Crawler.Application.Abstract;
using Crawler.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();
services.AddMongoSettings(configuration);
services.RegisterServices();
services.BuildServiceProvider();
var serviceProvider = services.BuildServiceProvider();

var productCrawler = serviceProvider.GetService<IProductCrawler>();

var result = await productCrawler.Start();
Console.WriteLine(result ? "success": "failed");

Console.WriteLine("Hello, World!");