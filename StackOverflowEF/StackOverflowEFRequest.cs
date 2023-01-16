using StackOverflowEF.Entities;
using StackOverflowEF.Requests;

namespace StackOverflowEF;
public static class StackOverflowEFRequest
    {
        public static WebApplication RegisterEndpoints(this WebApplication app)
        {
            app.MapGet("questionsscore", (StackOverflowContext db) =>
            {
                var questions = db.ViewQuestionsWithScore.ToList();
                return questions;
            });

            app.MapPost("questions/points/{questionId}", PointRequest.AddQuestionPoint)
                .WithTags("Points");
            app.MapPost("questions/answers/points/{answerId}", PointRequest.AddAnswerPoint)
                .WithTags("Points");

            app.MapGet("questions", QuestionRequest.GetAllQuestions)
                .WithTags("Questions");
            app.MapGet("questions/{id}", QuestionRequest.GetQuestionById)
                .WithTags("Questions");
            app.MapPost("questions", QuestionRequest.CreateQuestion)
                .WithTags("Questions");
            app.MapPut("questions/{id}", QuestionRequest.UpdateQuestion)
                .WithTags("Questions");
            app.MapDelete("questions/{id}", QuestionRequest.DeleteQuestion)
                .WithTags("Questions");

            app.MapGet("questions/answers/{id}", AnswerRequest.GetAnswerById)
                .WithTags("Answers");
            app.MapPost("questions/answers/{questionId}", AnswerRequest.CreateAnswer)
                .WithTags("Answers");
            app.MapPut("questions/answers/{answerId}", AnswerRequest.UpdateAnswer)
                .WithTags("Answers");
            app.MapDelete("questions/answers/{answerId}", AnswerRequest.DeleteAnswer)
                .WithTags("Answers");

            app.MapGet("questions/comments/{id}", CommentRequest.GetQuestionCommentById)
                .WithTags("Question comments");
            app.MapPost("questions/comments/{questionId}", CommentRequest.CreateQuestionComment)
                .WithTags("Question comments");
            app.MapPut("questions/comments/{commentId}", CommentRequest.UpdateQuestionComment)
                .WithTags("Question comments");
            app.MapDelete("questions/comments/{commentId}", CommentRequest.DeleteQuestionComment)
                .WithTags("Question comments");

            app.MapGet("questions/answers/comments/{id}", CommentRequest.GetAnswerCommentById)
                .WithTags("Answer comments");
            app.MapPost("questions/answers/comments/{answerId}", CommentRequest.CreateAnswerComment)
                .WithTags("Answer comments");
            app.MapPut("questions/answers/comments/{commentId}", CommentRequest.UpdateAnswerComment)
                .WithTags("Answer comments");
            app.MapDelete("questions/answers/comments/{commentId}", CommentRequest.DeleteAnswerComment)
                .WithTags("Answer comments");
            return app;
        }
    }

