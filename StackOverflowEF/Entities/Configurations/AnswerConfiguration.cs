using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static StackOverflowEF.Entities.Configurations.UserConfiguration;

namespace StackOverflowEF.Entities.Configurations;

public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.Property(a => a.Content).IsRequired().HasMaxLength(2000);
        builder.Property(a => a.CreatedDate).HasDefaultValueSql("getutcdate()");
        builder.Property(a => a.UpdatedDate).ValueGeneratedOnUpdate();
        builder.Property(a => a.Score).HasDefaultValue(0);

        builder.HasMany(a => a.Comments)
            .WithOne(c => c.Answer)
            .HasForeignKey(c => c.AnswerId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasData
        (
            new Answer() { Id = 1, Content = "You can do it in dbContext class", QuestionId = 1, UserId = Guid.Parse(FirstUserId) },
            new Answer() { Id = 2, Content = "You need to configure in Program.cs", QuestionId = 2, UserId = Guid.Parse(FirstUserId) },
            new Answer() { Id = 3, Content = "You can just configure like this:", QuestionId = 3, UserId = Guid.Parse(SecondUserId) },
            new Answer() { Id = 4, Content = "DateTimeOffset is a representation of instantaneous time.", QuestionId = 4, UserId = Guid.Parse(ThirdUserId) }
        );
    }
}