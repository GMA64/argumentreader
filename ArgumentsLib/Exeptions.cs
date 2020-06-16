using ArgumentMarshalerLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArgumentsLib
{
    public class LibraryArgumentException : ArgumentMarshalerLib.ArgumentsException
    {
        public LibraryArgumentException(ErrorCode errorCode, string errorParameter) : base(errorCode, errorParameter) { }

        public override string ErrorMessage()
        {
            switch (ErrorCode)
            {
                case ErrorCode.OK:
                    return "TILT: Should not be reached!";
                case ErrorCode.UNEXPECTED_ARGUMENT:
                    return $"Argument -{ErrorArgumentId} unexpected";
                case ErrorCode.INVALID_ARGUMENT_NAME:
                    return $"'-{ErrorArgumentId}' is not a valid argument name";
                case ErrorCode.INVALID_PARAMETER:
                    return $"'{ErrorParameter}' is not a valid parameter";
                case ErrorCode.GLOBAL:
                    return $"There was an ERROR with '{ErrorParameter}'";
                default:
                    return string.Empty;
            }
        }
    }
}
