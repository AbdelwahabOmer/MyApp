namespace MyApp.Service
{
    public interface IUserService
    {
        string GetUserId();
        bool IsAuthenticated();
    }
}