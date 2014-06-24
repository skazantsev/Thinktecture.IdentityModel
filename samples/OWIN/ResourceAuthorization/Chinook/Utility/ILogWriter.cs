
namespace Chinook.Utility
{
    public interface ILogWriter
    {
        void Write(string message);
        void Write(string message, string details);
    }
}
