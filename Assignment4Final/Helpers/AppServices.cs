using Assignment4Final.Data;
using Assignment4Final.Data.Repositories;
using Assignment4Final.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.DTO.Candidates;
using ModelLibrary.Models.DTO.Certificates;
using ModelLibrary.Models.DTO.Questions;
using ModelLibrary.Models.DTO.Exams;
using ModelLibrary.Models.DTO.CandidateExam;
using ModelLibrary.Models.Exams;
using ModelLibrary.Models.Questions;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using ModelLibrary.Models.DTO.Accounts;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Assignment4Final.Helpers;

public class AppServices
{
    public static void AddDatabaseServices(WebApplicationBuilder builder)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var connectionString =
            builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found."
            );
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(connectionString)
        );
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services
            .AddIdentity<AppUser, IdentityRole>(opt => opt.User.RequireUniqueEmail = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    // NOTE:(akotro) Make sure to remove and recreate migration for sqlite when using this
    public static void AddDatabaseServicesSqlite(WebApplicationBuilder builder)
    {
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        var connectionString =
            builder.Configuration.GetConnectionString("sqlite")
            ?? throw new InvalidOperationException("Connection string 'sqlite' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlite(connectionString)
        );
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services
            .AddIdentity<AppUser, IdentityRole>(opt => opt.User.RequireUniqueEmail = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddAuthentication(
        WebApplicationBuilder builder,
        IConfiguration configuration
    )
    {
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["keyjwt"])
                        ),
                        ClockSkew = TimeSpan.Zero
                    };
                }
            );
    }

    public static void AddAuthorization(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            // NOTE:(akotro) Admin has full rights to all data
            options.AddPolicy("IsAdmin", policy => policy.RequireClaim("role", "admin"));

            // NOTE:(akotro) Marker has right to mark assigned exams at a specific time
            options.AddPolicy("IsMarker", policy => policy.RequireClaim("role", "marker"));

            // NOTE:(akotro) Quality Control has new rights to all data
            options.AddPolicy(
                "IsQualityControl",
                policy => policy.RequireClaim("role", "qualitycontrol")
            );
            // NOTE:(akotro) Candidate can see and buy certificates and take exams
            options.AddPolicy("IsCandidate", policy => policy.RequireClaim("role", "candidate"));

            // NOTE:(akotro) Combined policies
            options.AddPolicy(
                "IsAdminOrMarker",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement("role", new[] { "admin", "marker" })
                    )
            );
            options.AddPolicy(
                "IsAdminOrQualityControl",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement(
                            "role",
                            new[] { "admin", "qualitycontrol" }
                        )
                    )
            );
            options.AddPolicy(
                "IsAdminOrCandidate",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement("role", new[] { "admin", "candidate" })
                    )
            );
            options.AddPolicy(
                "IsMarkerOrQualityControl",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement(
                            "role",
                            new[] { "marker", "qualitycontrol" }
                        )
                    )
            );
            options.AddPolicy(
                "IsMarkerOrCandidate",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement("role", new[] { "marker", "candidate" })
                    )
            );
            options.AddPolicy(
                "IsQualityControlOrCandidate",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement(
                            "role",
                            new[] { "qualitycontrol", "candidate" }
                        )
                    )
            );
            options.AddPolicy(
                "IsAdminOrMarkerOrQualityControl",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement(
                            "role",
                            new[] { "admin", "marker", "qualitycontrol" }
                        )
                    )
            );
            options.AddPolicy(
                "IsAdminOrMarkerOrCandidate",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement(
                            "role",
                            new[] { "admin", "marker", "candidate" }
                        )
                    )
            );
            options.AddPolicy(
                "IsAdminOrQualityControlOrCandidate",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement(
                            "role",
                            new[] { "admin", "qualitycontrol", "candidate" }
                        )
                    )
            );
            options.AddPolicy(
                "IsMarkerOrQualityControlOrCandidate",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement(
                            "role",
                            new[] { "marker", "qualitycontrol", "candidate" }
                        )
                    )
            );
            options.AddPolicy(
                "IsAny",
                policy =>
                    policy.Requirements.Add(
                        new ClaimsAuthorizationRequirement(
                            "role",
                            new[] { "admin", "marker", "qualitycontrol", "candidate" }
                        )
                    )
            );
        });
    }

    public static void AddControllers(WebApplicationBuilder builder)
    {
        builder.Services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
    }

    public static void AddSwagger(WebApplicationBuilder builder)
    {
        // NOTE:(akotro) Add Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description =
                        "Add the JWT bearer token obtained from authentication to the header."
                }
            );
            c.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                }
            );
        });
    }

    public static void AddRepositoriesAndServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IQuestionsRepository, QuestionsRepository>();
        builder.Services.AddScoped<QuestionsService>();

        builder.Services.AddScoped<ICandidateRepository, CandidateRepository>();
        builder.Services.AddScoped<CandidateService>();

        builder.Services.AddScoped<ICertificatesRepository, CertificatesRepository>();
        builder.Services.AddScoped<CertificatesService>();

        builder.Services.AddScoped<ITopicsRepository, TopicsRepository>();
        builder.Services.AddScoped<TopicsService>();

        builder.Services.AddScoped<IDifficultyLevelsRepository, DifficultyLevelsRepository>();
        builder.Services.AddScoped<DifficultyLevelsService>();

        builder.Services.AddScoped<IGenericRepository<Country>, CountriesRepository>();
        builder.Services.AddScoped<CountriesService>();

        builder.Services.AddScoped<IGenericRepository<Gender>, GendersRepository>();
        builder.Services.AddScoped<GendersService>();

        builder.Services.AddScoped<IGenericRepository<Language>, LanguagesRepository>();
        builder.Services.AddScoped<LanguagesService>();

        builder.Services.AddScoped<IGenericRepository<PhotoIdType>, PhotoIdTypesRepository>();
        builder.Services.AddScoped<PhotoIdTypesService>();

        builder.Services.AddScoped<ExamRepository>();
        builder.Services.AddScoped<ExamService>();
        builder.Services.AddScoped<CandidateExamRepository>();
        builder.Services.AddScoped<CandidateExamService>();

        builder.Services.AddScoped<
            IGenericRepository<CandidateExamAnswers>,
            CandidateExamAnswersRepository
        >();
        builder.Services.AddScoped<CandidateExamAnswersService>();

        builder.Services.AddScoped<IMarkersRepository, MarkersRepository>();
        builder.Services.AddScoped<MarkersService>();

        builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
        builder.Services.AddScoped<AccountsService>();

        builder.Services.AddScoped<PaypalService>();
    }

    public static void AddAutoMapper(WebApplicationBuilder builder)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.CreateMap<OptionDto, Option>().ReverseMap();
            mc.CreateMap<QuestionDto, Question>()
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                .ReverseMap();
            mc.CreateMap<CertificateDto, Certificate>()
                .ForMember(dest => dest.Topics, opt => opt.MapFrom(src => src.Topics))
                .ReverseMap();
            mc.CreateMap<TopicDto, Topic>().ReverseMap();
            mc.CreateMap<DifficultyLevelDto, DifficultyLevel>().ReverseMap();

            mc.CreateMap<CountryDto, Country>().ReverseMap();
            mc.CreateMap<AddressDto, Address>()
                .ForMember(c => c.Country, opt => opt.MapFrom(src => src.Country))
                .ReverseMap();
            mc.CreateMap<LanguageDto, Language>().ReverseMap();
            mc.CreateMap<GenderDto, Gender>().ReverseMap();
            mc.CreateMap<PhotoIdTypeDto, PhotoIdType>().ReverseMap();
            mc.CreateMap<CandidatesDto, Candidate>()
                .ForMember(c => c.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(c => c.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(c => c.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(c => c.PhotoIdType, opt => opt.MapFrom(src => src.PhotoIdType))
                .ReverseMap();
            mc.CreateMap<AppUser, UserDto>().ReverseMap();

            mc.CreateMap<Exam, ExamDto>().ReverseMap();

            mc.CreateMap<CandidateExam, CandidateExamDto>().ReverseMap();
            mc.CreateMap<CandidateExamAnswers, CandidateExamAnswersDto>().ReverseMap();

            mc.CreateMap<Marker, MarkerDto>().ReverseMap();
        });
        IMapper mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);
    }

    public static void AddCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
            options =>
                options.AddPolicy(
                    "FrontEndPolicy",
                    policy =>
                        policy
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .WithOrigins("https://localhost:44473")
                )
        );
    }

    public static void ConfigureApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseCors("FrontEndPolicy");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");
    }
}
