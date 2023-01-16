using Microsoft.EntityFrameworkCore;
using StackOverflowEF.Dto;
using StackOverflowEF.Entities;

namespace StackOverflowEF.Requests;

public class CommentRequest
{
    private const string ThirdUserId = "1b55d748-2ed4-4092-a1cc-a26c430d9d5e";

    public static WebApplication RegisterEndpoints(WebApplication app)
    {
        app.MapGet("comments/{id}", GetCommentById)
            .WithTags("Comments");

        app.MapPost("questions/{questionId}/comments", CreateQuestionComment)
            .WithTags("Comments");

        app.MapPost("answers/{answerId}/comments", CreateAnswerComment)
            .WithTags("Comments");

        app.MapPut("comments/{commentId}", UpdateComment)
            .WithTags("Comments");

        app.MapDelete("comments/{commentId}", DeleteComment)
            .WithTags("Comments");

        return app;
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

        return Results.Created($"comments/{newComment.Id}", newComment);
    }

    public static IResult CreateAnswerComment(StackOverflowContext db, int answerId, CommentDto commentDto)
    {
        var userId = Guid.Parse(ThirdUserId);

        var newComment = new Comment()
        {
            Content = commentDto.Content,
            UserId = userId,
            AnswerId = answerId
        };

        var answer = db.Answers
            .Include(a => a.Comments)
            .FirstOrDefault(a => a.Id == answerId);

        if (answer == null)
        {
            return Results.NotFound();
        }

        answer.Comments.Add(newComment);
        db.SaveChanges();

        return Results.Created($"comments/{newComment.Id}", newComment);
    }

    public static IResult GetCommentById(StackOverflowContext db, int id)
    {
        var comment = db.Comments.FirstOrDefault(c => c.Id == id);

        if (comment == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(comment);
    }

    public static IResult UpdateComment(StackOverflowContext db, int commentId, CommentDto commentDto)
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

    public static IResult DeleteComment(StackOverflowContext db, int commentId)
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

