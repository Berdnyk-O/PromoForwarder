using PromoForwarder;
using System.Configuration;
using Microsoft.Extensions.Hosting;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Logging;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

string? SenderMail = ConfigurationManager.AppSettings["SenderMail"];
string? SenderMailPass = ConfigurationManager.AppSettings["SenderMailPass"];
string? RecipientMail = ConfigurationManager.AppSettings["RecipientMail"];
string? cronSchedule = ConfigurationManager.AppSettings["CronSchedule"];

if (string.IsNullOrEmpty(SenderMail) || string.IsNullOrEmpty(SenderMailPass) || string.IsNullOrEmpty(RecipientMail))
{
    Console.WriteLine("Error while receiving configuration data");
    return;
}

if (string.IsNullOrEmpty(cronSchedule))
{
    cronSchedule = "0 30 10 ? * WED,FRI";
}

var builder = Host.CreateDefaultBuilder()
    .ConfigureServices((cxt, services) =>
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey("ForwardPromoJob-trigger");
            q.AddJob<ForwardPromoJob>(opt => opt.WithIdentity(jobKey));
            q.AddTrigger(opt =>
            {
                opt.ForJob(jobKey)
                .WithIdentity("ForwardPromoJob")
                .WithCronSchedule(cronSchedule);
            });
        });
        services.AddQuartzHostedService(opt =>
        {
            opt.WaitForJobsToComplete = true;
        });
        services.AddScoped<EmailReader>(x =>
            new(SenderMail, SenderMailPass));
        services.AddScoped<EmailForwarder>(x =>
            new(SenderMail, SenderMailPass));
        services.AddScoped<ForwardPromoJob>(serviceProvider =>
        {
            var emailReader = serviceProvider.GetRequiredService<EmailReader>();
            var emailForwarder = serviceProvider.GetRequiredService<EmailForwarder>();
            return new(emailReader, emailForwarder, RecipientMail);
        });
    }).Build();

await builder.RunAsync();