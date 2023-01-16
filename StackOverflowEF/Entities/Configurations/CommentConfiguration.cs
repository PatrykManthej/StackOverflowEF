using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static StackOverflowEF.Entities.Configurations.UserConfiguration;

namespace StackOverflowEF.Entities.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(c => c.Content).IsRequired().HasMaxLength(600);
            builder.Property(c => c.CreatedDate).HasDefaultValueSql("getutcdate()");
            builder.Property(c => c.UpdatedDate).ValueGeneratedOnUpdate();
            builder.HasCheckConstraint("CK_Comments_AnswerId_QuestionId_NotBothNull", "(([QuestionId] IS NULL OR [AnswerId] IS NULL) AND NOT ([QuestionId] IS NULL AND [AnswerId] IS NULL))");

            builder.HasData(
                new Comment() { Id = 1, Content = "Good", UserId = Guid.Parse(FirstUserId), AnswerId = 1 },
                new Comment() { Id = 2, Content = "Wrong", UserId = Guid.Parse(SecondUserId), AnswerId = 2 },
                new Comment() { Id = 3, Content = "Nice", UserId = Guid.Parse(ThirdUserId), QuestionId = 2 },
                new Comment() { Id = 4, Content = "Comment 4", UserId = Guid.Parse(ThirdUserId), QuestionId = 2 },
                new Comment() { Id = 5, Content = "Comment", UserId = Guid.Parse(ThirdUserId), QuestionId = 2 }, 
                new Comment() { Id = 6, Content = "Comment 6", UserId = Guid.Parse(ThirdUserId), QuestionId = 2 },
                new Comment() { Id = 7, Content = "Comment 7", UserId = Guid.Parse(ThirdUserId), QuestionId = 1 },
                 new Comment() { Id = 8, Content = "Wrong 2", UserId = Guid.Parse(SecondUserId), AnswerId = 2 }
                );
        }
    }
}
