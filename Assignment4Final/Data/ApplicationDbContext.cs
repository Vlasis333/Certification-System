using Duende.IdentityServer.EntityFramework.Options;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModelLibrary.Models;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.Exams;
using ModelLibrary.Models.Questions;

namespace Assignment4Final.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<AppUser>
    {
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<PhotoIdType> PhotoIdTypes { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Certificate> Certificates { get; set; }

        public virtual DbSet<Topic> Topics { get; set; }

        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<DifficultyLevel> DifficultyLevels { get; set; }

        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<CandidateExam> CandidateExams { get; set; }
        public virtual DbSet<CandidateExamAnswers> CandidateExamAnswers { get; set; }
        public virtual DbSet<Marker> Markers { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions
        ) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<AppUser>()
                .HasOne(a => a.Candidate)
                .WithOne(c => c.AppUser)
                .HasForeignKey<Candidate>(c => c.AppUserId)
                .IsRequired(false);
            builder.Entity<Candidate>().HasKey(c => c.AppUserId);

            builder
                .Entity<AppUser>()
                .HasOne(a => a.Marker)
                .WithOne(m => m.AppUser)
                .HasForeignKey<Marker>(m => m.AppUserId)
                .IsRequired(false);
            builder.Entity<Marker>().HasKey(m => m.AppUserId);
        }

        // NOTE:(akotro) Is this needed? See https://learn.microsoft.com/en-us/ef/core/querying/single-split-queries
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseSqlServer(
        //         o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        //     );
        // }
    }
}
