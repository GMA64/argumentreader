using ArgumentMarshalerLib;
using System;

namespace IntegerMarshalerLib
{
    public class IntegerArgumentMarshaler : ArgumentMarshaler
    {
        public override string Schema => "#";

        public override void Set(Iterator<string> currentArgument)
        {
            string parameter = null;

            try
            {
                parameter = currentArgument.Next();
                Value = int.Parse(parameter);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new IntegerMarshalerException(ErrorCode.MISSING);
            }
            catch (FormatException)
            {
                throw new IntegerMarshalerException(ErrorCode.INVALID, parameter);
            }
        }

        public class IntegerMarshalerException : ArgumentMarshalerLib.ArgumentsException
        {
            public IntegerMarshalerException(ErrorCode errorCode) : base(errorCode) { }

            public IntegerMarshalerException(ErrorCode errorCode, string errorParameter) : base(errorCode, errorParameter) { }

            public override string ErrorMessage()
            {
                switch (ErrorCode)
                {
                    case ErrorCode.MISSING:
                        return $"Could not find integer parameter for -{ErrorArgumentId}";
                    case ErrorCode.INVALID:
                        return $"Argument -{ErrorArgumentId} expects an integer but was '{ErrorParameter}'";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
