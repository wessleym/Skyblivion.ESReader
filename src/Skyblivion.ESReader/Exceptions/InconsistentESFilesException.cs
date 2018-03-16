using System;

namespace Skyblivion.ESReader.Exceptions
{
    class InconsistentESFilesException : Exception
    {
        public InconsistentESFilesException(string message)
            : base(message)
        { }
    }
}