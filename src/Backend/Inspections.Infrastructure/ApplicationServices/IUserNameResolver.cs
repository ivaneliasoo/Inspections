namespace Inspections.Core
{
    public interface IUserNameResolver
    {
        string UserName { get; }
        bool IsAdmin { get; }
        string FullName { get; }
    }
}
