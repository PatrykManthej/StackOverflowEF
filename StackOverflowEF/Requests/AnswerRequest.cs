using Microsoft.EntityFrameworkCore;
using StackOverflowEF.Dto;
using StackOverflowEF.Entities;

namespace StackOverflowEF.Requests;

public class AnswerRequest
{
    private const string SecondUserId = "0B72E7C5-6C7A-42CA-B6C4-687CDC937D98";

    public static WebApplication RegisterEndpoints(WebApplication app)
    {
        app.MapGet("answers/{id}", GetAnswerById)
            .WithTags("Answers");

        app.MapPost("questions/{questionId}/answers", CreateAnswer)
            .WithTags("Answers");

        app.MapPut("answers/{answerId}", UpdateAnswer)
            .WithTags("Answers");

        app.MapDelete("answers/{answerId}", DeleteAnswer)
            .WithTags("Answers");

        return app;
    }

    public static IResult GetAnswerById(StackOverflowContext db, int id)
    {
        var answer = db.Answers.FirstOrDefault(a => a.Id == id);

        if (answer == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(answer);
    }

    public static IResult CreateAnswer(StackOverflowContext db, int questionId, AnswerDto answerDto)
    {
        var userId = Guid.Parse(SecondUserId);

        var newAnswer = new Answer()
        {
            Content = answerDto.Content,
            UserId = userId,
        };

        var question = db.Questions
        .Include(q => q.Answers)
        .FirstOrDefault(q => q.Id == questionId);

        if (question == null)
        {
            return Results.NotFound();
        }

        question.Answers.Add(newAnswer);
        db.SaveChanges();

        return Results.Created($"answers/{newAnswer.Id}", newAnswer);
    }

    public static IResult UpdateAnswer(StackOverflowContext db, int answerId, AnswerDto answerDto)
    {
        var answer = db.Answers.FirstOrDefault(a => a.Id == answerId);

        if (answer == null)
        {
            return Results.NotFound();
        }

        answer.Content = answerDto.Content;
        db.SaveChanges();

        return Results.NoContent();
    }

    public static IResult DeleteAnswer(StackOverflowContext db, int answerId)
    {
        var answer = db.Answers
            .Include(a => a.Comments)
            .Include(a => a.Points)
            .FirstOrDefault(a => a.Id == answerId);

        if (answer == null)
        {
            return Results.NotFound();
        }

        db.Remove(answer);
        db.SaveChanges();

        return Results.NoContent();
    }
}