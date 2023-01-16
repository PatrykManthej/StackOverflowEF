using FluentValidation;
using Microsoft.EntityFrameworkCore;
using StackOverflowEF.Dto;
using StackOverflowEF.Entities;

namespace StackOverflowEF.Requests;

public class QuestionRequest
{
    private const string SecondUserId = "0B72E7C5-6C7A-42CA-B6C4-687CDC937D98";

    public static WebApplication RegisterEndpoints(WebApplication app)
    {
        app.MapGet("questions/score", (StackOverflowContext db) =>
        {
            var questions = db.ViewQuestionsWithScore.ToList();
            return questions;
        })
            .WithTags("Questions");

        app.MapGet("questions", GetAllQuestions)
            .WithTags("Questions");

        app.MapGet("questions/{id}", GetQuestionById)
            .WithTags("Questions");

        app.MapPost("questions", CreateQuestion)
            .WithTags("Questions");

        app.MapPut("questions/{id}", UpdateQuestion)
            .WithTags("Questions");

        app.MapDelete("questions/{id}", DeleteQuestion)
            .WithTags("Questions");

        return app;
    }

    public static IResult GetAllQuestions(StackOverflowContext db)
    {
        var questions = db.Questions
            .Include(q => q.Answers)
            .Include(q => q.Tags)
            .Include(q => q.User)
            .Select(q => new
            {
                q.Id,
                q.Title,
                CreatedDate = q.CreatedDate.ToString("g"),
                NumberOfAnswers = q.Answers.Count,
                AuthorName = q.User.Name,
                q.Tags
            })
            .ToList();

        return Results.Ok(questions);
    }

    public static IResult GetQuestionById(StackOverflowContext db, int id)
    {
        var question = db.Questions
        .Include(q => q.Answers).ThenInclude(a => a.Comments).ThenInclude(c => c.User)
        .Include(q => q.Answers).ThenInclude(a => a.User)
        .Include(q => q.Tags)
        .Include(q => q.Comments).ThenInclude(c => c.User)
        .Include(q => q.User)
        .Select(q => new
        {
            q.Id,
            q.Title,
            q.Content,
            q.Score,
            CreatedDate = q.CreatedDate.ToString("g"),
            AuthorName = q.User.Name,
            Tags = q.Tags.Select(t => t.Name),
            Comments = q.Comments.Select(c => new
            {
                c.Content,
                AuthorName = c.User.Name,
                CreatedDate = c.CreatedDate.ToString("g")
            }),
            Answers = q.Answers.Select(answer => new
            {
                answer.Content,
                AuthorName = answer.User.Name,
                CreatedDate = answer.CreatedDate.ToString("g"),
                answer.Score,
                Comments = answer.Comments.Select(c => new
                {
                    c.Content,
                    AuthorName = c.User.Name,
                    CreatedDate = c.CreatedDate.ToString("g")
                })
            })
        })
        .FirstOrDefault(q => q.Id == id);

        if (question == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(question);
    }

    public static IResult CreateQuestion(StackOverflowContext db, QuestionDto questionDto, IValidator<QuestionDto> validator)
    {
        var validationResult = validator.Validate(questionDto);

        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        var userId = Guid.Parse(SecondUserId);

        var tag = db.Tags.First(t => t.Id == 1);

        var question = new Question()
        {
            Title = questionDto.Title,
            Content = questionDto.Content,
            UserId = userId,
            Tags = new List<Tag>()
        };
        question.Tags.Add(tag);

        db.Questions.Add(question);
        db.SaveChanges();

        return Results.Created($"questions/{question.Id}", question);
    }

    public static IResult UpdateQuestion(StackOverflowContext db, int id, QuestionUpdateDto questionDto)
    {
        var questionDb = db.Questions
          .Include(q => q.Tags)
          .FirstOrDefault(q => q.Id == id);

        if (questionDb == null)
        {
            return Results.NotFound();
        }

        if (questionDto.QuestionDto != null)
        {
            if (questionDto.QuestionDto.Title != null)
            {
                questionDb.Title = questionDto.QuestionDto.Title;
            }
            if (questionDto.QuestionDto.Content != null)
            {
                questionDb.Content = questionDto.QuestionDto.Content;
            }
        }

        var dbTags = db.Tags.ToList();

        if (questionDto.Tags != null)
        {
            foreach (var tag in questionDto.Tags)
            {
                if (questionDb.Tags.FirstOrDefault(t => t.Name == tag.Name) is null)
                {
                    var dbTag = dbTags.FirstOrDefault(t => t.Name == tag.Name);
                    if (dbTag is null)
                    {
                        var newTag = new Tag() { Name = tag.Name };
                        questionDb.Tags.Add(newTag);
                    }
                    else
                    {
                        questionDb.Tags.Add(dbTag);
                    }
                }
            }
        }

        db.SaveChanges();

        return Results.NoContent();
    }

    public static IResult DeleteQuestion(StackOverflowContext db, int id)
    {
        var question = db.Questions
           .Include(q => q.Answers).ThenInclude(a => a.Comments)
           .Include(q => q.Comments)
           .Include(q => q.Points)
           .Include(q => q.Tags)
           .FirstOrDefault(q => q.Id == id);

        if (question == null)
        {
            return Results.NotFound();
        }

        db.Questions.Remove(question);
        db.SaveChanges();

        return Results.NoContent();
    }
}

