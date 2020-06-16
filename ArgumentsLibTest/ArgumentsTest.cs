using ArgumentMarshalerLib;
using ArgumentsLib;
using DoubleMarshalerLib;
using IntegerMarshalerLib;
using StringMarshalerLib;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xunit;

namespace ArgumentsLibTest
{
    public class ArgumentsTest
    {
        private string marshalers = @"Marshaler";
        private string schema = "text*,int#,double##,bool,booltwo";
        private string[] args =
        {
            "-text",
            "Test",
            "-int",
            "1234",
            "-double",
            Convert.ToDouble("1234,4321", CultureInfo.CreateSpecificCulture("de-AT")).ToString(),
            "-bool"
        };

        [Fact]
        public void CreateReference_PassingTest()
        {
            Arguments a = new Arguments(this.marshalers, this.schema, this.args);
        }

        [Fact]
        public void CreateReferenceAndGetValues_PassingTest()
        {
            Arguments a = new Arguments(this.marshalers, this.schema, this.args);

            string[] schema = a.Schema.ToArray();

            Assert.Equal(args[1], a.GetValue<string>(schema[0]));
            Assert.Equal(args[3], a.GetValue<int>(schema[1]).ToString());
            Assert.Equal(args[5], a.GetValue<double>(schema[2]).ToString());
            Assert.True(a.GetValue<bool>(schema[3]));
            Assert.False(a.GetValue<bool>(schema[4]));
        }

        [Fact]
        public void CreateReferenceAndGetSchema_PassingTest()
        {
            Arguments a = new Arguments(this.marshalers, this.schema, this.args);

            string[] testschema = this.schema.Split(',');
            string[] schema = a.Schema.ToArray();

            Assert.Equal(testschema.Length, schema.Length);

            for (int i = 0; i < schema.Length; i++)
            {
                Assert.Equal(new string(testschema[i].Where(c => char.IsLetter(c)).ToArray()), schema[i]);
            }
        }

        [Fact]
        public void CreateReferenceAndGetFoundArguments_PassingTest()
        {
            Arguments a = new Arguments(this.marshalers, this.schema, this.args);

            string[] arguments = a.ArgumentsFound.ToArray();

            for (int i = 0; i < arguments.Length; i++)
            {
                Assert.Equal(args[(i * 2)].Substring(1), arguments[i]);
            }
        }

        [Fact]
        public void CreateReferenceAndSetDirectoryToNull_FailingTest()
        {
            Arguments a;

            ArgumentsException ex = Assert.Throws<LibraryArgumentException>(() => a = new Arguments(null, this.schema, this.args));

            Assert.Equal(ErrorCode.GLOBAL, ex.ErrorCode);
            Assert.Equal($"Marshaler Directory: {string.Empty} not found!", ex.ErrorParameter);

            Assert.Equal($"There was an ERROR with 'Marshaler Directory: {string.Empty} not found!'", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndSetSchemaToNull_FailingTest()
        {
            Arguments a;

            Assert.Throws<NullReferenceException>(() => a = new Arguments(this.marshalers, null, this.args));
        }

        [Fact]
        public void CreateReferenceAndSetSchemaToEmptyString_FailingTest()
        {
            Arguments a;

            ArgumentsException ex = Assert.Throws<LibraryArgumentException>(() => a = new Arguments(this.marshalers, string.Empty, this.args));

            Assert.Equal(ErrorCode.UNEXPECTED_ARGUMENT, ex.ErrorCode);
            Assert.Equal(this.args[0].Substring(1), ex.ErrorParameter);

            Assert.Equal($"Argument -{string.Empty} unexpected", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndSetArgumentsToNull_FailingTest()
        {
            Arguments a;

            Assert.Throws<ArgumentNullException>(() => a = new Arguments(this.marshalers, this.schema, null));
        }

        [Fact]
        public void CreateReferenceAndSetArgumentsToEmptyArray_PassingTest()
        {
            Arguments a = new Arguments(this.marshalers, this.schema, new string[] { });
        }

        [Fact]
        public void CreateReferenceAndSetArgumentsWithFailingDoubleParameter_FailingTest()
        {
            Array.Resize(ref this.args, (args.Length - 2));
            string testErrorArgumentId = this.args[(args.Length - 1)].Substring(1);

            Arguments a;

            ArgumentsException ex = Assert.Throws<DoubleArgumentMarshaler.DoubleMarshalerException>(() => a = new Arguments(this.marshalers, this.schema, this.args));
            
            Assert.Equal(ErrorCode.MISSING, ex.ErrorCode);
            Assert.Equal(testErrorArgumentId, ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);

            Assert.Equal($"Could not find double parameter for -{testErrorArgumentId}", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndSetArgumentsWithWrongDoubleParameter_FailingTest()
        {
            Array.Resize(ref this.args, (args.Length - 1));
            this.args[(args.Length - 1)] = "NotANumber";

            string testErrorArgumentId = this.args[(args.Length - 2)].Substring(1);

            Arguments a;

            ArgumentsException ex = Assert.Throws<DoubleArgumentMarshaler.DoubleMarshalerException>(() => a = new Arguments(this.marshalers, this.schema, this.args));

            Assert.Equal(ErrorCode.INVALID, ex.ErrorCode);
            Assert.Equal(testErrorArgumentId, ex.ErrorArgumentId);
            Assert.Equal(ex.ErrorParameter, this.args[(args.Length - 1)]);

            Assert.Equal($"Argument -{testErrorArgumentId} expects an double but was '{this.args[(args.Length - 1)]}'", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndSetArgumentsWithFailingIntegerParameter_FailingTest()
        {
            Array.Resize(ref this.args, (args.Length - 4));
            string testErrorArgumentId = this.args[(args.Length - 1)].Substring(1);

            Arguments a;

            ArgumentsException ex = Assert.Throws<IntegerArgumentMarshaler.IntegerMarshalerException>(() => a = new Arguments(this.marshalers, this.schema, this.args));

            Assert.Equal(ErrorCode.MISSING, ex.ErrorCode);
            Assert.Equal(testErrorArgumentId, ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);

            Assert.Equal($"Could not find integer parameter for -{testErrorArgumentId}", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndSetArgumentsWithWrongIntegerParameter_FailingTest()
        {
            Array.Resize(ref this.args, (args.Length - 3));
            this.args[(args.Length - 1)] = "NotANumber";

            string testErrorArgumentId = this.args[(args.Length - 2)].Substring(1);

            Arguments a;

            ArgumentsException ex = Assert.Throws<IntegerArgumentMarshaler.IntegerMarshalerException>(() => a = new Arguments(this.marshalers, this.schema, this.args));

            Assert.Equal(ErrorCode.INVALID, ex.ErrorCode);
            Assert.Equal(testErrorArgumentId, ex.ErrorArgumentId);
            Assert.Equal(ex.ErrorParameter, this.args[(args.Length - 1)]);

            Assert.Equal($"Argument -{testErrorArgumentId} expects an integer but was '{this.args[(args.Length - 1)]}'", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndSetArgumentsWithFailingStringParameter_FailingTest()
        {
            Array.Resize(ref this.args, (args.Length - 6));
            string testErrorArgumentId = this.args[(args.Length - 1)].Substring(1);

            Arguments a;

            ArgumentsException ex = Assert.Throws<StringArgumentMarshaler.StringMarshalerException>(() => a = new Arguments(this.marshalers, this.schema, this.args));

            Assert.Equal(ErrorCode.MISSING, ex.ErrorCode);
            Assert.Equal(testErrorArgumentId, ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);

            Assert.Equal($"Could not find string parameter for -{testErrorArgumentId}", ex.ErrorMessage());
        }
    }
}
