namespace Application;

public interface IMyClient
{
    Task<string> AskQuestionAsync(string question, CancellationToken cancellationToken = default);
}
