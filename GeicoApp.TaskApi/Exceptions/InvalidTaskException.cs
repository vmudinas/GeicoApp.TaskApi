using System.Runtime.Serialization;

namespace GeicoApp.TaskApi.Exceptions
{
    [Serializable]
    public class InvalidTaskException : Exception
    {
        public InvalidTaskException()
        {
        }

        public InvalidTaskException(string message) : base(message)
        {
        }

        public InvalidTaskException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
