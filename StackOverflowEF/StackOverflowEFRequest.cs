using StackOverflowEF.Entities;
using StackOverflowEF.Requests;

namespace StackOverflowEF;
public static class StackOverflowEFRequest
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        QuestionRequest.RegisterEndpoints(app);
        AnswerRequest.RegisterEndpoints(app);
        CommentRequest.RegisterEndpoints(app);
        PointRequest.RegisterEndpoints(app);
        return app;
    }
}

