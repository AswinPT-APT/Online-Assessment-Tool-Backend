using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OnlineAssessmentTool.Application.Common.Interfaces.IRepository;
using OnlineAssessmentTool.Application.Common.Interfaces.IService;
using OnlineAssessmentTool.Persistence;
using OnlineAssessmentTool.Repository;
using OnlineAssessmentTool.Services;
using OnlineAssessmentTool.Services.BackgroundServices;

namespace OnlineAssessmentTool.Infrastructure
{
    public static class ServiceRegistry
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IBatchRepository, BatchRepository>();
            services.AddScoped<IPermissionsRepository, PermissionsRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITrainerRepository, TrainerRepository>();
            services.AddScoped<ITraineeRepository, TraineeRepository>();
            services.AddScoped<ITrainerBatchRepository, TrainerBatchRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IAssessmentService, AssessmentService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IScheduledAssessmentRepository, ScheduledAssessmentRepository>();
            services.AddScoped<IScheduledAssessmentService, ScheduledAssessmentService>();
            services.AddScoped<ITraineeAnswerRepository, TraineeAnswerRepository>();
            services.AddScoped<IAssessmentScoreRepository, AssessmentScoreRepository>();
            services.AddScoped<IAssessmentScoreService, AssessmentScoreService>();
            services.AddScoped<IAssessmentPostService, AssessmentPostService>();
            services.AddScoped<IIlpRepository, IlpIntegrationRepository>();
            services.AddScoped<ILPIntegrationService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();

            var connectionString = "User Id=postgres.qasikddpduvbagpmuthq;Password=Aefmrs@rev01;Server=aws-0-ap-southeast-1.pooler.supabase.com;Port=5432;Database=postgres;";

            NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString).EnableSensitiveDataLogging()
                                  .EnableDetailedErrors();
            });

            services.AddHostedService<AssessmentStatusUpdater>();
            services.AddHostedService<PasswordExpiryChecker>();
            services.AddHostedService<BatchActiveService>();
        }
    }
}
