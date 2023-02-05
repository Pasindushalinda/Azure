using System;
using AzureFunctionFundamentals;
using AzureFunctionFundamentals.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly:WebJobsStartup(typeof(Startup))]
namespace AzureFunctionFundamentals;

public class Startup:IWebJobsStartup
{
    public void Configure(IWebJobsBuilder builder)
    {
        string connectionString = Environment.GetEnvironmentVariable("AzureSqlDatabase");

        builder.Services.AddDbContext<AzureFunctionDbContext>(opt =>
            opt.UseSqlServer(connectionString));

        builder.Services.BuildServiceProvider();
    }
}