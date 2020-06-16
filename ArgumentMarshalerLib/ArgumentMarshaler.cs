using System;

namespace ArgumentMarshalerLib
{
    public abstract class ArgumentMarshaler
    {
        public object Value { get; protected set; }

        public abstract string Schema { get; }

        public abstract void Set(Iterator<string> currentArgument);
    }
}
