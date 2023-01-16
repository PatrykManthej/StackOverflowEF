using Microsoft.EntityFrameworkCore;
using StackOverflowEF.Dto;
using StackOverflowEF.Entities;

namespace StackOverflowEF.Requests
{
    public class CommentRequest
    {
        private const string ThirdUserId = "1b55d748-2ed4-4092-a1cc-a26c430d9d5e";

        public static IResult GetQuestionCommentById(StackOverflowContext db, int id)
        {
            var comment = db.Comments.FirstOrDefault(c => c.Id == id);

            if (comment == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(comment);
        }

        public static IResult CreateQuestionComment(StackOverflowContext db, int questionId, CommentDto commentDto)
        {
            var userId = Guid.Parse(ThirdUserId);

            var newComment = new Comment()
            {
                Content = commentDto.Content,
                UserId = userId,
                QuestionId = questionId
            };

            var question = db.Questions
            .Include(q => q.Comments)
            .FirstOrDefault(q => q.Id == questionId);

            if (question == null)
            {
                return Results.NotFound();
            }

            question.Comments.Add(newComment);
            db.SaveChanges();

            return Results.Created($"questions/comments/{newComment.Id}", newComment);
        }

        public static IResult UpdateQuestionComment(StackOverflowContext db, int commentId, CommentDto commentDto)
        {
            var comment = db.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return Results.NotFound();
            }

            comment.Content = commentDto.Content;
            db.SaveChanges();

            return Results.NoContent();
        }

        public static IResult DeleteQuestionComment(StackOverflowContext db, int commentId)
        {
            var comment = db.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return Results.NotFound();
            }

            db.Remove(comment);
            db.SaveChanges();

            return Results.NoContent();
        }

        public static IResult GetAnswerCommentById(StackOverflowContext db, int id)
        {
            var comment = db.Comments.FirstOrDefault(c => c.Id == id);

            if (comment == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(comment);
        }

        public static IResult CreateAnswerComment(StackOverflowContext db, int answerId, CommentDto commentDto)
        {
            var answer = db.Answers
            .Include(a => a.Comments)
            .FirstOrDefault(a => a.Id == answerId);

            if (answer == null)
            {
                return Results.NotFound();
            }

            var userId = Guid.Parse(ThirdUserId);

            var newComment = new Comment()
            {
                Content = commentDto.Content,
                UserId = userId,
                AnswerId = answerId
            };

            answer.Comments.Add(newComment);
            db.SaveChanges();

            return Results.Created($"answers/comments/{newComment.Id}", newComment);
        }

        public static IResult UpdateAnswerComment(StackOverflowContext db, int commentId, CommentDto commentDto)
        {
            var comment = db.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return Results.NotFound();
            }

            comment.Content = commentDto.Content;
            db.SaveChanges();

            return Results.NoContent();
        }

        public static IResult DeleteAnswerComment(StackOverflowContext db, int commentId)
        {
            var comment = db.Comments.FirstOrDefault(c => c.Id == commentId);

            if (comment == null)
            {
                return Results.NotFound();
            }

            db.Remove(comment);
            db.SaveChanges();

            return Results.NoContent();
        }
    }
}
