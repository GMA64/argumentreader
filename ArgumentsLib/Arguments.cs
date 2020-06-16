using ArgumentMarshalerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ArgumentsLib
{
    public class Arguments
    {
        private Reflector reflector;
        private Dictionary<string, ArgumentMarshaler> marshalers;
        private Iterator<string> currentArgument;
        private List<string> argumentsFound;

        public Arguments(string marshalerDirectory, string schema, IEnumerable<string> args)
        {
            this.reflector = new Reflector(marshalerDirectory);

            this.marshalers = new Dictionary<string, ArgumentMarshaler>();
            this.argumentsFound = new List<string>();

            ParseSchema(schema);
            ParseArgumentStrings(new List<string>(args));
        }

        private void ParseSchema(string schema)
        {
            foreach (string argument in schema.Split(','))
            {
                if (argument.Length > 0 && !string.IsNullOrWhiteSpace(argument))
                {
                    ParseSchemaArgument(argument.Trim());
                }
            }
        }

        private void ParseSchemaArgument(string argument)
        {
            StringBuilder argumentId = new StringBuilder();
            StringBuilder argumentTail = new StringBuilder();

            for (int i = 0; i < argument.Length; i++)
            {
                if (char.IsLetter(argument[i]))
                {
                    argumentId.Append(argument[i]);
                }
                else
                {
                    argumentTail.Append(argument.Substring(i));
                    break;
                }
            }

            ArgumentMarshaler marshaler = reflector.GetInstanceBySchema(argumentTail.ToString()) ?? throw new LibraryArgumentException(ErrorCode.INVALID_ARGUMENT_NAME, argumentId.ToString());
            this.marshalers.Add(argumentId.ToString(), marshaler);
        }

        private void ParseArgumentStrings(List<string> argumentList)
        {
            for (this.currentArgument = new Iterator<string>(argumentList); this.currentArgument.HasNext;)
            {
                string argumentString = currentArgument.Next();

                if (argumentString.Length > 0 && argumentString.ElementAt(0) == '-')
                {
                    ParseArgumentString(argumentString.Substring(1));
                }
                else
                {
                    throw new LibraryArgumentException(ErrorCode.INVALID_PARAMETER, argumentString);
                }
            }
        }

        private void ParseArgumentString(string argumentString)
        {
            if (!this.marshalers.TryGetValue(argumentString, out ArgumentMarshaler m))
                throw new LibraryArgumentException(ErrorCode.UNEXPECTED_ARGUMENT, argumentString);

            this.argumentsFound.Add(argumentString);

            try
            {
                m.Set(currentArgument);
            }
            catch (ArgumentsException exception)
            {
                exception.ErrorArgumentId = argumentString;
                throw exception;
            }
        }

        public T GetValue<T>(string argument)
        {
            return marshalers.ContainsKey(argument) ? this.marshalers[argument].Value == null ? default :
                                                      (T)this.marshalers[argument].Value :
                                                      throw new LibraryArgumentException(ErrorCode.INVALID_PARAMETER, argument);
        }

        public IEnumerable<string> ArgumentsFound => argumentsFound;

        public IEnumerable<string> Schema
        {
            get
            {
                List<string> schemaList = new List<string>();

                foreach (string marshaler in this.marshalers.Keys)
                {
                    schemaList.Add(marshaler);
                }

                return schemaList;
            }
        }

    }
}
