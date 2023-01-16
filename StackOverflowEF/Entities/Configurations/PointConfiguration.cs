using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static StackOverflowEF.Entities.Configurations.UserConfiguration;

namespace StackOverflowEF.Entities.Configurations;

public class PointConfiguration : IEntityTypeConfiguration<Point>
{
    public void Configure(EntityTypeBuilder<Point> builder)
    {
        builder.Property(p => p.Value).HasDefaultValue(0);
        builder.HasCheckConstraint("CK_Points_AnswerId_QuestionId_NotBothNull", "([QuestionId] IS NULL OR [AnswerId] IS NULL) AND NOT ([QuestionId] IS NULL AND [AnswerId] IS NULL)");

        builder.HasOne(p => p.Question)
            .WithMany(q => q.Points)
            .HasForeignKey(q => q.QuestionId)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.HasOne(p => p.Answer)
            .WithMany(a => a.Points)
            .HasForeignKey(a => a.AnswerId)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.HasOne(p => p.User)
            .WithMany(u=>u.Points)
            .HasForeignKey(p=>p.UserId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.HasData(
            new Point() { Id = 1, QuestionId = 1, UserId = Guid.Parse(FirstUserId), Value = -1},
            new Point() { Id = 2, QuestionId = 2, UserId = Guid.Parse(FirstUserId), Value = 1},
            new Point() { Id = 3, QuestionId = 3, UserId = Guid.Parse(FirstUserId), Value = -1},
            new Point() { Id = 4, QuestionId = 2, UserId = Guid.Parse(SecondUserId), Value = 1},
            new Point() { Id = 5, QuestionId = 3, UserId = Guid.Parse(SecondUserId), Value = -1},
            new Point() { Id = 7, QuestionId = 4, UserId = Guid.Parse(ThirdUserId), Value = 1},
            new Point() { Id = 8, QuestionId = 2, UserId = Guid.Parse(ThirdUserId), Value = 1 },
            new Point() { Id = 9, QuestionId = 1, UserId = Guid.Parse(ThirdUserId), Value = -1 },
            new Point() { Id = 10, QuestionId = 3, UserId = Guid.Parse(ThirdUserId), Value = -1 }
        );
    }
}