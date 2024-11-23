﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineAssessmentTool.Persistence;

namespace OnlineAssessmentTool.Services.BackgroundServices
{
    public class AssessmentStatusUpdater : BackgroundService
    {
        private readonly ILogger<AssessmentStatusUpdater> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AssessmentStatusUpdater(ILogger<AssessmentStatusUpdater> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Checking for overdue assessments...");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var now = DateTime.UtcNow;
                    var currentDate = now.Date;
                    var currentTime = now.TimeOfDay;

                    try
                    {
                        var assessmentsToUpdate = context.ScheduledAssessments
                        .Where(a => a.EndTime <= now && a.Status == 0)
                        .ToList();

                        foreach (var assessment in assessmentsToUpdate)
                        {
                            assessment.Status = (Shared.Enums.AssessmentStatus)3;
                            context.ScheduledAssessments.Update(assessment);
                        }

                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex) { }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Check every minute
            }
        }
    }
}
