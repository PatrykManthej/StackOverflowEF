using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static StackOverflowEF.Entities.Configurations.UserConfiguration;

namespace StackOverflowEF.Entities.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(q => q.Title).IsRequired().HasMaxLength(150);
            builder.Property(q => q.Content).IsRequired().HasMaxLength(30000);
            builder.Property(q => q.Score).HasDefaultValue(0);
            builder.Property(q => q.CreatedDate).HasDefaultValueSql("getutcdate()");
            builder.Property(q => q.UpdatedDate).ValueGeneratedOnUpdate();

            builder.HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId);

            builder.HasMany(q=>q.Comments)
                .WithOne(c=>c.Question)
                .HasForeignKey(c => c.QuestionId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(q => q.User)
                .WithMany(u=>u.Questions)
                .HasForeignKey(q=>q.UserId);

            builder.HasMany(q => q.Tags)
                .WithMany(t => t.Questions)
                .UsingEntity<QuestionTag>(qtBuilder => {

                    qtBuilder.HasOne(qt => qt.Question)
                    .WithMany()
                    .HasForeignKey(qt => qt.QuestionId);

                    qtBuilder.HasOne(qt => qt.Tag)
                    .WithMany()
                    .HasForeignKey(qt => qt.TagId);

                    qtBuilder.HasKey(qt => new { qt.QuestionId, qt.TagId });

                    qtBuilder.HasData(
                        new QuestionTag { QuestionId = 1, TagId = 3},
                        new QuestionTag { QuestionId = 1, TagId = 2},
                        new QuestionTag { QuestionId = 2, TagId = 1},
                        new QuestionTag { QuestionId = 3, TagId = 2},
                        new QuestionTag { QuestionId = 4, TagId = 4 }
                        );

                    }
                );

            builder.HasData(
                new Question() { Id = 1, Title = "Entity Framework", Content = "How to add indexes by using Fluent API?", UserId = Guid.Parse(FirstUserId), Score = -2 },
                new Question() { Id = 2, Title = "Asp .Net Core", Content = "How to configure services in ASP.NET Core?", UserId = Guid.Parse(SecondUserId), Score = 3},
                new Question() { Id = 3, Title = "Asp .Net Core MVC", Content = "How to add Rotativa.aspnetcore configuration in Program.cs instead of RotativaConfiguration.Setup(env); that was in Startup.cs in .NET 5 and below?", UserId = Guid.Parse(ThirdUserId), Score = -3},
                new Question() { Id = 4, Title = "DateTime vs DateTimeOffset", Content = "What is the difference between a DateTime and a DateTimeOffset and when should one be used?", UserId = Guid.Parse(ThirdUserId), Score = 1}
                ) ;
        }
    }
}
