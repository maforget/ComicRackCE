using System;

namespace cYo.Projects.ComicRack.Engine.IO.Provider.Readers.Archive
{
    public class WriteErrorException : Exception
    {
        public WriteErrorException() : base()
        {
        }

        public WriteErrorException(string message) : base(message)
        {
        }

        public WriteErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
