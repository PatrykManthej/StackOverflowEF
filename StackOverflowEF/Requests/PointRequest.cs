using Microsoft.EntityFrameworkCore;
using StackOverflowEF.Entities;

namespace StackOverflowEF.Requests;

public class PointRequest
{
    private const string SecondUserId = "0b72e7c5-6c7a-42ca-b6c4-687cdc937d98";

    public static WebApplication RegisterEndpoints(WebApplication app)
    {
        app.MapPost("questions/{questionId}points", AddQuestionPoint)
            .WithTags("Points");

        app.MapPost("answers/{answerId}/points", AddAnswerPoint)
            .WithTags("Points");

        return app;
    }

    public static IResult AddQuestionPoint(StackOverflowContext db, int questionId)
    {
        //Ustawia flage w zaleznosci, ktora strzalka zostala kliknieta w glosowaniu up(true), down(false)
        var arrowAttribute = false;
        var exampleUserId = Guid.Parse(SecondUserId);

        var userPoint = db.Points.Include(p => p.Question)
        .FirstOrDefault(p => (p.UserId == exampleUserId && p.QuestionId == questionId));

        if (userPoint == null)
        {
            var pointValue = arrowAttribute ? 1 : -1;
            var newPoint = new Point() { QuestionId = questionId, UserId = exampleUserId, Value = pointValue };
            var question = db.Questions.First(q => q.Id == newPoint.QuestionId);
            question.Score += newPoint.Value;
            db.Points.Add(newPoint);
        }
        else
        {
            switch (userPoint.Value, arrowAttribute)
            {
                case (1, true):
                    break;
                case (1, false):
                    userPoint.Value = -1;
                    userPoint.Question.Score -= 2;
                    break;
                case (-1, true):
                    userPoint.Value = 1;
                    userPoint.Question.Score += 2;
                    break;
                case (-1, false):
                    break;
                default:
                    break;
            }
        }

        db.SaveChanges();

        return Results.NoContent();
    }

    public static IResult AddAnswerPoint(StackOverflowContext db, int answerId)
    {
        var arrowAttribute = true;
        var exampleUserId = Guid.Parse(SecondUserId);

        var userPoint = db.Points.Include(p => p.Answer)
        .FirstOrDefault(p => (p.UserId == exampleUserId && p.AnswerId == answerId));

        if (userPoint == null)
        {
            var pointValue = arrowAttribute ? 1 : -1;
            var newPoint = new Point() { AnswerId = answerId, UserId = exampleUserId, Value = pointValue };
            var answer = db.Answers.First(a => a.Id == newPoint.AnswerId);
            answer.Score += newPoint.Value;
            db.Points.Add(newPoint);
        }
        else
        {
            switch (userPoint.Value, arrowAttribute)
            {
                case (1, true):
                    break;
                case (1, false):
                    userPoint.Value = -1;
                    userPoint.Question.Score -= 2;
                    break;
                case (-1, true):
                    userPoint.Value = 1;
                    userPoint.Question.Score += 2;
                    break;
                case (-1, false):
                    break;
                default:
                    break;
            }
        }

        db.SaveChanges();

        return Results.NoContent();
    }
}