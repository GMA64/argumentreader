using ArgumentMarshalerLib;
using System;

namespace StringMarshalerLib
{
    public class StringArgumentMarshaler : ArgumentMarshaler
    {
        public override string Schema => "*";

        public override void Set(Iterator<string> currentArgument)
        {
            try
            {
                Value = currentArgument.Next();
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new StringMarshalerException(ErrorCode.MISSING);
            }
        }

        public class StringMarshalerException : ArgumentsException
        {
            public StringMarshalerException() { }

            public StringMarshalerException(ErrorCode errorCode) : base(errorCode) { }

            public override string ErrorMessage()
            {
                switch (ErrorCode)
                {
                    case ErrorCode.MISSING:
                        return $"Could not find string parameter for -{ErrorArgumentId}";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
