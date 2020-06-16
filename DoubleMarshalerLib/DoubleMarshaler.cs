using ArgumentMarshalerLib;
using System;

namespace DoubleMarshalerLib
{
    public class DoubleArgumentMarshaler : ArgumentMarshaler
    {
        public override string Schema => "##";

        public override void Set(Iterator<string> currentArgument)
        {
            string parameter = null;

            try
            {
                parameter = currentArgument.Next();
                Value = double.Parse(parameter);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new DoubleMarshalerException(ErrorCode.MISSING);
            }
            catch (FormatException)
            {
                throw new DoubleMarshalerException(ErrorCode.INVALID, parameter);
            }
        }

        public class DoubleMarshalerException : ArgumentsException
        {
            public DoubleMarshalerException(ErrorCode errorCode) : base(errorCode) { }

            public DoubleMarshalerException(ErrorCode errorCode, string parameter) : base(errorCode, parameter) { }

            public override string ErrorMessage()
            {
                switch (ErrorCode)
                {
                    case ErrorCode.MISSING:
                        return $"Could not find double parameter for -{ErrorArgumentId}";
                    case ErrorCode.INVALID:
                        return $"Argument -{ErrorArgumentId} expects an double but was '{ErrorParameter}'";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
