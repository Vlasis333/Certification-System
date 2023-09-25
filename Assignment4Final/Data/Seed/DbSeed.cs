using Bogus;
using ModelLibrary.Models.Candidates;
using ModelLibrary.Models.Certificates;
using ModelLibrary.Models.Exams;
using ModelLibrary.Models.Questions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ModelLibrary.Models;
using System.Reflection.Emit;
using System;
using Bogus.DataSets;
using System.Security.Claims;

namespace Assignment4Final.Data.Seed
{
    public static class DbSeed
    {
        public static async Task Seed(WebApplication? app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                try
                {
                    var db = service.GetRequiredService<ApplicationDbContext>();

                    bool migrated = await Migrate(db);

                    SeedStandaloneTables(db);
                    SeedRelatedTables(db);
                    SeedJoiningTables(db);

                    if (migrated)
                    {
                        SeedCalculatedFields(db);
                    }

                    var userManager = service.GetRequiredService<UserManager<AppUser>>();
                    await SeedRoles(db, userManager);
                }
                catch (Exception ex)
                {
                    var logger = service.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static async Task<bool> Migrate(ApplicationDbContext db)
        {
            try
            {
                Console.WriteLine("Attempting to apply migration...");
                await db.Database.MigrateAsync();
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: Tried to apply migration but failed.");
                Console.WriteLine("Do you want to continue to drop db and update? (y/n)");

                string input = Console.ReadLine();
                if (input.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Dropping the database and migrating...");
                    await db.Database.EnsureDeletedAsync();
                    await db.Database.MigrateAsync();
                }
                else if (input.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Exiting program...");
                    Environment.Exit(0);
                }
            }

            return true;
        }

        public static void SeedStandaloneTables(ApplicationDbContext db)
        {
            Randomizer.Seed = new Random(8675309);

            #region // Adding AppUsers

            // ---------------------------------
            // Login Details Example

            // user: admin1@gmail.com
            // pass: Admin1!

            // user: candidate1@gmail.com
            // pass: Candidate1!

            // user: marker1@gmail.com
            // pass: Marker1!

            // user: qualitycontrol1@gmail.com
            // pass: Qualitycontrol1!

            // user: nothing1@gmail.com
            // pass: Nothing1!
            // ---------------------------------

            // check if table is empty and only then add users
            if (!db.Users.Any())
            {
                List<string> fakeGuids = new List<string>();
                List<AppUser> fakeAppUsers = new List<AppUser>();
                PasswordHasher<AppUser> passHash = new PasswordHasher<AppUser>();

                // create 25 GUIDs
                for (int i = 0; i < 25; i++)
                {
                    fakeGuids.Add(Guid.NewGuid().ToString());
                }

                int candidateIndex = 1;
                int adminIndex = 1;
                int markerIndex = 1;
                int qualitycontrolIndex = 1;
                int nothingIndex = 1;

                // Create details for the fake AppUsers
                for (int i = 0; i < fakeGuids.Count; i++)
                {
                    if (i < 10) // NOTE:(akotro) Make 10 candidates
                    {
                        fakeAppUsers.Add(
                            new AppUser()
                            {
                                Id = fakeGuids[i],
                                UserName = $"candidate{candidateIndex}@gmail.com",
                                NormalizedUserName = $"candidate{candidateIndex}@gmail.com",
                                Email = $"candidate{candidateIndex}@gmail.com",
                                NormalizedEmail = $"candidate{candidateIndex}@gmail.com",
                                LockoutEnabled = false,
                                PhoneNumber = "1234567890",
                                SecurityStamp = Guid.NewGuid().ToString(),
                                EmailConfirmed = true,
                            }
                        );
                        fakeAppUsers[i].PasswordHash = passHash.HashPassword(
                            fakeAppUsers[i],
                            $"Candidate{candidateIndex}!"
                        );

                        candidateIndex++;
                    }
                    else if (i >= 10 && i < 14) // NOTE:(akotro) Make 4 admins
                    {
                        fakeAppUsers.Add(
                            new AppUser()
                            {
                                Id = fakeGuids[i],
                                UserName = $"admin{adminIndex}@gmail.com",
                                NormalizedUserName = $"admin{adminIndex}@gmail.com",
                                Email = $"admin{adminIndex}@gmail.com",
                                NormalizedEmail = $"admin{adminIndex}@gmail.com",
                                LockoutEnabled = false,
                                PhoneNumber = "1234567890",
                                SecurityStamp = Guid.NewGuid().ToString(),
                                EmailConfirmed = true,
                            }
                        );
                        fakeAppUsers[i].PasswordHash = passHash.HashPassword(
                            fakeAppUsers[i],
                            $"Admin{adminIndex}!"
                        );

                        adminIndex++;
                    }
                    else if (i >= 14 && i < 18) // NOTE:(akotro) Make 4 markers
                    {
                        fakeAppUsers.Add(
                            new AppUser()
                            {
                                Id = fakeGuids[i],
                                UserName = $"marker{markerIndex}@gmail.com",
                                NormalizedUserName = $"marker{markerIndex}@gmail.com",
                                Email = $"marker{markerIndex}@gmail.com",
                                NormalizedEmail = $"marker{markerIndex}@gmail.com",
                                LockoutEnabled = false,
                                PhoneNumber = "1234567890",
                                SecurityStamp = Guid.NewGuid().ToString(),
                                EmailConfirmed = true,
                            }
                        );
                        fakeAppUsers[i].PasswordHash = passHash.HashPassword(
                            fakeAppUsers[i],
                            $"Marker{markerIndex}!"
                        );

                        markerIndex++;
                    }
                    else if (i >= 18 && i < 20) // NOTE:(akotro) Make 2 qualitycontrols
                    {
                        fakeAppUsers.Add(
                            new AppUser()
                            {
                                Id = fakeGuids[i],
                                UserName = $"qualitycontrol{qualitycontrolIndex}@gmail.com",
                                NormalizedUserName =
                                    $"qualitycontrol{qualitycontrolIndex}@gmail.com",
                                Email = $"qualitycontrol{qualitycontrolIndex}@gmail.com",
                                NormalizedEmail = $"qualitycontrol{qualitycontrolIndex}@gmail.com",
                                LockoutEnabled = false,
                                PhoneNumber = "1234567890",
                                SecurityStamp = Guid.NewGuid().ToString(),
                                EmailConfirmed = true,
                            }
                        );
                        fakeAppUsers[i].PasswordHash = passHash.HashPassword(
                            fakeAppUsers[i],
                            $"Qualitycontrol{qualitycontrolIndex}!"
                        );

                        qualitycontrolIndex++;
                    }
                    else
                    {
                        fakeAppUsers.Add(
                            new AppUser()
                            {
                                Id = fakeGuids[i],
                                UserName = $"nothing{nothingIndex}@gmail.com",
                                NormalizedUserName = $"nothing{nothingIndex}@gmail.com",
                                Email = $"nothing{nothingIndex}@gmail.com",
                                NormalizedEmail = $"nothing{nothingIndex}@gmail.com",
                                LockoutEnabled = false,
                                PhoneNumber = "1234567890",
                                SecurityStamp = Guid.NewGuid().ToString(),
                                EmailConfirmed = true,
                            }
                        );
                        fakeAppUsers[i].PasswordHash = passHash.HashPassword(
                            fakeAppUsers[i],
                            $"Nothing{nothingIndex}!"
                        );

                        nothingIndex++;
                    }
                }

                // Add and save Users
                db.Users.AddRange(fakeAppUsers);
                db.SaveChanges();
            }

            #endregion


            #region // Adding Enum options and Seeding Gender Table in DB

            // check if table is empty and only then proceed
            if (!db.Genders.Any())
            {
                var genderEntries = new List<Gender>();

                // Looping through the Gender Options
                foreach (var value in Enum.GetValues(typeof(GenderEnum)))
                {
                    genderEntries.Add(new Gender { GenderType = (GenderEnum)value });
                }

                // Add and save Gender Options
                db.Genders.AddRange(genderEntries);
                db.SaveChanges();
            }

            #endregion


            #region // Adding Enum options and Seeding PhotoIdType Table in DB

            // check if table is empty and only then proceed
            if (!db.PhotoIdTypes.Any())
            {
                var photoIdTypeEntries = new List<PhotoIdType>();

                foreach (var value in Enum.GetValues(typeof(PhotoIdTypeEnum)))
                {
                    photoIdTypeEntries.Add(new PhotoIdType { IdType = (PhotoIdTypeEnum)value });
                }

                // Add and save PhotoIdType Options
                db.PhotoIdTypes.AddRange(photoIdTypeEntries);
                db.SaveChanges();
            }

            #endregion


            #region // Adding Enum options and Seeding DifficultyLevels Table in DB

            // check if table is empty and only then proceed
            if (!db.DifficultyLevels.Any())
            {
                var difficultyLevelEntries = new List<DifficultyLevel>();

                foreach (var value in Enum.GetValues(typeof(DifficultyEnum)))
                {
                    difficultyLevelEntries.Add(
                        new DifficultyLevel { Difficulty = (DifficultyEnum)value }
                    );
                }

                // Add and save DifficultyLevel Options
                db.DifficultyLevels.AddRange(difficultyLevelEntries);
                db.SaveChanges();
            }

            #endregion


            #region // Seeding Languages table

            if (!db.Languages.Any())
            {
                var languages = new string[] { "English", "Greek", "Russian", "Chinese" };
                for (int i = 0; i < languages.Length; i++)
                {
                    db.Languages.Add(new Language { NativeLanguage = languages[i] });
                }

                db.SaveChanges();
            }

            #endregion


            #region // Seeding Countries table

            if (!db.Countries.Any())
            {
                var countryFaker = new Faker<Country>().RuleFor(
                    u => u.CountryOfResidence,
                    f => f.Address.Country()
                );

                var fakeCountries = countryFaker.Generate(10);

                db.Countries.AddRange(fakeCountries);
                db.SaveChanges();
            }

            #endregion


            #region // Seeding Topics table

            if (!db.Topics.Any())
            {
                var topicFaker = new Faker<Topic>().RuleFor(
                    u => u.Name,
                    f => f.Random.Words(3).ToString()
                );

                var fakeTopics = topicFaker.Generate(10);
                db.Topics.AddRange(fakeTopics);
                db.SaveChanges();
            }

            #endregion


            #region // Seeding Certificates table

            if (!db.Certificates.Any())
            {
                var certFaker = new Faker<Certificate>()
                    .RuleFor(c => c.Title, f => f.Random.Words(3))
                    .RuleFor(c => c.Description, f => f.Random.Words(10))
                    .RuleFor(c => c.Category, f => f.Random.Words(1))
                    .RuleFor(c=> c.Price, f=> f.Random.Number(5,10)*20)
                    .RuleFor(c => c.Active, f => f.Random.Bool());
                var fakeCertificates = certFaker.Generate(5);
                db.Certificates.AddRange(fakeCertificates);
                db.SaveChanges();
            }

            #endregion


            #region // Seeding Exams table

            if (!db.Exams.Any())
            {
                var fakeExams = new List<Exam>();

                var examFaker = new Faker<Exam>().RuleFor(
                    c => c.Certificate,
                    f => f.PickRandom(db.Certificates.ToList())
                );

                var fakeExamEntries = examFaker.Generate(10);

                fakeExams.AddRange(fakeExamEntries);
                db.Exams.AddRange(fakeExams);
                db.SaveChanges();
            }

            #endregion
        }

        public static void SeedRelatedTables(ApplicationDbContext db)
        {
            #region // Seeding Candidate table

            if (
                !db.Candidates.Any()
                && db.Genders.Any()
                && db.PhotoIdTypes.Any()
                && db.Languages.Any()
            )
            {
                var candidateFaker = new Faker<Candidate>()
                    .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                    .RuleFor(u => u.MiddleName, f => f.Name.FirstName())
                    .RuleFor(u => u.LastName, f => f.Name.LastName())
                    .RuleFor(u => u.Email, f => f.Internet.Email())
                    .RuleFor(u => u.Landline, f => f.Phone.PhoneNumber())
                    .RuleFor(u => u.Mobile, f => f.Phone.PhoneNumber())
                    .RuleFor(
                        u => u.CandidateNumber,
                        f => f.Random.Number(100000000, 999999999).ToString()
                    )
                    .RuleFor(u => u.PhotoIdNumber, f => f.Random.AlphaNumeric(6))
                    .RuleFor(u => u.DateOfBirth, f => f.Date.Past(100, DateTime.Now))
                    .RuleFor(u => u.PhotoIdIssueDate, f => f.Date.Past(10, DateTime.Now))
                    .RuleFor(u => u.Gender, f => f.PickRandom(db.Genders.ToList()))
                    .RuleFor(u => u.PhotoIdType, f => f.PickRandom(db.PhotoIdTypes.ToList()))
                    .RuleFor(u => u.Language, f => f.PickRandom(db.Languages.ToList()));

                var fakeCandidates = candidateFaker.Generate(10);

                List<AppUser> candidateUsers = db.Users
                    .Where(u => u.Email.StartsWith("candidate"))
                    .OrderBy(u => u.Email)
                    .ToList();
                // var random = new Random();

                // Add an existing User Id to a each Candidate
                for (int i = 0; i < fakeCandidates.Count; i++)
                {
                    // int randomIndex = random.Next(0, candidateUsers.Count);
                    fakeCandidates[i].AppUserId = candidateUsers[i].Id;
                }

                db.Candidates.AddRange(fakeCandidates);
                db.SaveChanges();
            }

            #endregion

            #region // Seeding Marker table

            if (!db.Markers.Any())
            {
                var markerFaker = new Faker<Marker>();
                var fakeMarkers = markerFaker.Generate(4);

                var markerUsers = db.Users
                    .Where(u => u.Email.StartsWith("marker"))
                    .OrderBy(u => u.Email)
                    .ToList();
                // var random = new Random();

                for (int i = 0; i < fakeMarkers.Count; i++)
                {
                    // int randomIndex = random.Next(0, fakeMarkers.Count);
                    fakeMarkers[i].AppUserId = markerUsers[i].Id;
                }

                db.Markers.AddRange(fakeMarkers);
                db.SaveChanges();
            }

            #endregion

            #region // Seeding Addresses table

            if (!db.Addresses.Any() && db.Candidates.Any() && db.Countries.Any())
            {
                var addressFaker = new Faker<ModelLibrary.Models.Candidates.Address>()
                    .RuleFor(u => u.Address1, f => f.Address.StreetAddress())
                    .RuleFor(u => u.Address2, f => f.Address.StreetAddress())
                    .RuleFor(u => u.City, f => f.Address.City())
                    .RuleFor(u => u.State, f => f.Address.State())
                    .RuleFor(u => u.PostalCode, f => f.Address.ZipCode())
                    .RuleFor(u => u.Country, f => f.PickRandom(db.Countries.ToList()))
                    .RuleFor(u => u.Candidate, f => f.PickRandom(db.Candidates.ToList()));

                var fakeAddresses = addressFaker.Generate(10);
                db.Addresses.AddRange(fakeAddresses);
                db.SaveChanges();
            }

            #endregion

            #region // Seeding Questions and options table

            if (!db.Questions.Any() && db.DifficultyLevels.Any() && db.Topics.Any())
            {
                var optionFaker = new Faker<Option>()
                    .RuleFor(c => c.Text, f => $"{f.Random.Words(5)} !!!")
                    .RuleFor(c => c.Correct, f => false);

                var questionFaker = new Faker<Question>()
                    .RuleFor(
                        c => c.DifficultyLevel,
                        f => f.PickRandom(db.DifficultyLevels.ToList())
                    )
                    .RuleFor(c => c.Topic, f => f.PickRandom(db.Topics.ToList()))
                    .RuleFor(c => c.Text, f => $"<strong>{f.Random.Words(15)} ???</strong>")
                    .RuleFor(
                        c => c.Options,
                        f =>
                        {
                            var opt = optionFaker.Generate(4);
                            opt[1].Correct = true;
                            return opt;
                        }
                    );

                var fakeQuestions = questionFaker.Generate(40);
                db.Questions.AddRange(fakeQuestions);
                db.SaveChanges();
            }

            #endregion

            #region // Seeding Options table

            //if (!db.Options.Any() && db.Questions.Any())
            //{


            //    var fakeOptions = new List<Option>();
            //    int optionIndex = 0;
            //    for (int i = 0; i < db.Questions.Count(); i++)
            //    {
            //        for (int j = 0; j < 4; j++)
            //        {
            //            var option = new Option();
            //            option.QuestionId = i + 1;
            //            option.Id = optionIndex + 1;

            //            if (j == 0)
            //                option.Correct = true;
            //            else
            //                option.Correct = false;

            //            option.Text = faker.PickRandom(options);
            //            fakeOptions.Add(option);
            //            optionIndex++;

            //        }
            //    }
            //    modelBuilder.Entity<Option>().HasData(fakeOptions);
            //}

            #endregion

            #region // Seeding CandidateExam table

            if (!db.CandidateExams.Any())
            {
                var candiExamAnsFaker = new Faker<CandidateExamAnswers>()
                    .RuleFor(
                        c => c.Question,
                        f => f.PickRandom(db.Questions.Include(q => q.Options).ToList())
                    )
                    .RuleFor(c => c.QuestionText, (f, c) => c.Question.Text)
                    .RuleFor(
                        c => c.CorrectOption,
                        (f, c) => c.Question.Options.FirstOrDefault(o => o.Correct).Text.ToString()
                    )
                    .RuleFor(
                        c => c.ChosenOption,
                        (f, c) =>
                            f.Random.Number(100) < 65
                                ? c.CorrectOption
                                : f.PickRandom(
                                    c.Question.Options
                                        .Where(o => o.Text != c.CorrectOption)
                                        .ToList()
                                )
                                    .Text.ToString()
                    )
                    .RuleFor(
                        c => c.IsCorrect,
                        (f, c) =>
                            c.Question.Options.FirstOrDefault(o => o.Text == c.ChosenOption).Correct
                    );

                var candiExamFaker = new Faker<CandidateExam>()
                    .RuleFor(c => c.ExamDate, f => f.Date.Past(2, DateTime.Now))
                    .RuleFor(c => c.Exam, f => f.PickRandom(db.Exams.ToList()))
                    .RuleFor(c => c.Candidate, f => f.PickRandom(db.Candidates.ToList()))
                    .RuleFor(
                        c => c.CandidateExamAnswers,
                        f =>
                        {
                            //return candiExamAnsFaker.Generate(100);
                            return candiExamAnsFaker.Generate(10);
                        }
                    )
                    .RuleFor(
                        c => c.ReportDate,
                        f => f.Date.Between(new DateTime(2022, 6, 10, 0, 0, 0), DateTime.Now)
                    )
                    .RuleFor(c => c.Marker, f => f.PickRandom(db.Markers.ToList()))
                    .RuleFor(
                        c => c.MarkerAssignedDate,
                        f => f.Date.Between(DateTime.Now, f.Date.Soon(7))
                    )
                    .RuleFor(u => u.Voucher, f => f.Random.AlphaNumeric(10));

                // .RuleFor(
                //     c => c.MarkingDate,
                //     f => f.Date.Between(new DateTime(2022, 6, 10, 0, 0, 0), DateTime.Now)
                // );
                var fakecandiExams = candiExamFaker.Generate(10);

                db.CandidateExams.AddRange(fakecandiExams);
                db.SaveChanges();
            }

            #endregion
        }

        private static List<Topic> PickRandomTopics(
            Faker faker,
            int count,
            List<Topic> potentialTopics
        )
        {
            var randomTopics = new List<Topic>();

            for (int i = 0; i < count; i++)
            {
                bool added = false;
                while (!added)
                {
                    var topic = faker.PickRandom(potentialTopics);
                    if (!randomTopics.Contains(topic))
                    {
                        randomTopics.Add(topic);
                        added = true;
                    }
                }
            }

            return randomTopics;
        }

        private static List<Question> PickRandomQuestions(
            Faker faker,
            int count,
            List<Question> potentialQuestions
        )
        {
            var randomQuestions = new HashSet<Question>();

            while (
                randomQuestions.Count < count && randomQuestions.Count < potentialQuestions.Count
            )
            {
                var question = faker.PickRandom(potentialQuestions);
                randomQuestions.Add(question);
            }

            return randomQuestions.ToList();
        }

        public static void SeedJoiningTables(ApplicationDbContext db)
        {
            Randomizer.Seed = new Random(8675309);
            var faker = new Faker();

            //adding 4Topics to each Certificate
            foreach (var item in db.Certificates.Include(c => c.Topics).ToList())
            {
                if (item.Topics.Count == 0)
                {
                    item.Topics = PickRandomTopics(faker, 4, db.Topics.ToList());
                }
            }

            db.SaveChanges();

            var passPercentage = 60;
            //adding 10 questions to each Exam
            foreach (
                var item in db.Exams
                    .AsSplitQuery()
                    .Include(c => c.Questions)
                    .Include(e => e.Certificate)
                    .ThenInclude(c => c.Topics)
                    .ThenInclude(t => t.Questions)
                    .ToList()
            )
            {
                if (item.Questions.Count == 0)
                {
                    var potentialQuestions = new List<Question>();
                    item.Certificate
                        ?.Topics?.ToList()
                        .ForEach(t => potentialQuestions.AddRange(t.Questions));

                    item.Questions = PickRandomQuestions(faker, 10, potentialQuestions);
                    item.MaxMark = item.Questions.Count;
                    item.PassMark = Convert.ToInt32(item.MaxMark * (passPercentage / 100.0));
                }
            }

            db.SaveChanges();
        }

        public static void SeedCalculatedFields(ApplicationDbContext db)
        {
            //calculating MaxMark and Passing mark for each Certificate
            // var passMark = 65;
            // foreach (var cert in db.Certificates)
            // {
            //     if (cert.MaxMark == null)
            //     {
            //         cert.MaxMark = 0;
            //         var cerTopics = cert.Topics.ToList();
            //         foreach (var topic in cerTopics)
            //         {
            //             cert.MaxMark = cert.MaxMark + topic.MaxMarks;
            //         }

            //         cert.PassingMark = Convert.ToInt32(cert.MaxMark * (passMark / 100.0));
            //     }
            // }

            // db.SaveChanges();

            foreach (var candExam in db.CandidateExams.Include(ce => ce.CandidateExamAnswers))
            {
                if (candExam.MaxScore == null)
                {
                    candExam.MaxScore = candExam.Exam?.MaxMark;
                }

                if (candExam.CandidateScore == null)
                {
                    candExam.CandidateScore = 0;
                    if (candExam.CandidateExamAnswers != null)
                    {
                        foreach (var ans in candExam.CandidateExamAnswers)
                        {
                            if (ans.IsCorrect != null && (bool)ans.IsCorrect)
                            {
                                candExam.CandidateScore = candExam.CandidateScore + 1;
                            }
                        }
                    }
                }
                candExam.PercentScore =
                    ((decimal)candExam.CandidateScore / (decimal)candExam.MaxScore) * 100;
                candExam.Result = candExam.PercentScore >= 60 ? true : false;
            }
            db.SaveChanges();
        }

        public static async Task SeedRoles(
            ApplicationDbContext db,
            UserManager<AppUser> userManager
        )
        {
            if (!db.UserClaims.Any())
            {
                // NOTE:(akotro) Add admin roles
                var admins = await userManager.Users
                    .Where(u => u.Email.StartsWith("admin"))
                    .ToListAsync();

                foreach (var admin in admins)
                {
                    await userManager.AddClaimAsync(admin, new Claim("role", "admin"));
                }

                // NOTE:(akotro) Add candidate roles
                var candidates = await userManager.Users
                    .Where(u => u.Email.StartsWith("candidate"))
                    .ToListAsync();

                foreach (var candidate in candidates)
                {
                    await userManager.AddClaimAsync(candidate, new Claim("role", "candidate"));
                }

                // NOTE:(akotro) Add marker roles
                var markers = await userManager.Users
                    .Where(u => u.Email.StartsWith("marker"))
                    .ToListAsync();

                foreach (var marker in markers)
                {
                    await userManager.AddClaimAsync(marker, new Claim("role", "marker"));
                }

                // NOTE:(akotro) Add qualitycontrol roles
                var qualitycontrols = await userManager.Users
                    .Where(u => u.Email.StartsWith("qualitycontrol"))
                    .ToListAsync();

                foreach (var qualitycontrol in qualitycontrols)
                {
                    await userManager.AddClaimAsync(
                        qualitycontrol,
                        new Claim("role", "qualitycontrol")
                    );
                }
            }
        }
    }
}
