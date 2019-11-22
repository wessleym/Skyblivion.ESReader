using System;

namespace Skyblivion.ESReader.Extensions
{
    //Thrown to avoid null dereference warnings.  Substitution for null forgiveness.
    public class NullableException : InvalidOperationException
    {
        public NullableException(string variableName) : base(variableName + " was null.") { }
    }
}
