using FluentValidation;
using OnlineAssessmentTool.Dtos;
using OnlineAssessmentTool.ServiceRegistry;
using OnlineAssessmentTool.Validations;
using Serilog;
using Serilog.Events;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var smtpSettings = new SmtpSettings
{
    Host = "smtp.gmail.com",
    Port = 587,
    UserName = "aswinpt.apt.2001@gmail.com",
    Password = "anaftkkmcrexkhdo",
    FromEmail = "aswinpt.apt.2001@gmail.com",
    FromName = "Knowlix"
};

/*Log.Logger = new LoggerConfiguration()
.WriteTo.File("logs\\myapp.log", rollingInterval: RollingInterval.Day)
.CreateLogger();*/

Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Information()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
           .CreateLogger();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
);

builder.Services.AddValidatorsFromAssemblyContaining<TestValidator>();

builder.Services.AddFluentEmail(smtpSettings.FromEmail, smtpSettings.FromName)
    .AddRazorRenderer()
    .AddSmtpSender(new SmtpClient(smtpSettings.Host)
    {
        Port = smtpSettings.Port,
        Credentials = new System.Net.NetworkCredential(smtpSettings.UserName, smtpSettings.Password),
        EnableSsl = true,


        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
    });

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost");

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
