using ArgumentMarshalerLib;
using System;

namespace BooleanMarshalerLib
{
    public class BooleanArgumentMarshaler : ArgumentMarshaler
    {
        public override string Schema => string.Empty;

        public override void Set(Iterator<string> currentArgument)
        {
            Value = true;
        }

        public class BooleanMarshalerException : ArgumentsException
        {
            public override string ErrorMessage()
            {
                switch (ErrorCode)
                {
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
