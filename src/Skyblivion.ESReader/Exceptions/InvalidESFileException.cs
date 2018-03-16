using System;

namespace Skyblivion.ESReader.Exceptions
{
    class InvalidESFileException : Exception
    {
        public InvalidESFileException(string message)
            : base(message)
        { }
    }
}