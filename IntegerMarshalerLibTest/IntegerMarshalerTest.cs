using ArgumentMarshalerLib;
using IntegerMarshalerLib;
using System;
using System.Collections.Generic;
using Xunit;

namespace IntegerMarshalerLibTest
{
    public class IntegerMarshalerTest
    {
        private readonly string testSchema = "#";
        private readonly string testArgument = "argument";
        private readonly string testParameter = "parameter";
        private List<string> testData = new List<string>()
            {
                "1234",
                "-nextcommand",
                "NotANumber",
            };

        [Fact]
        public void CreateReferenceAndSetNull_FailingTest()
        {
            IntegerArgumentMarshaler m = new IntegerArgumentMarshaler();

            Assert.Throws<NullReferenceException>(() => m.Set(null));
        }

        [Fact]
        public void CreateReferenceAndSetIterator_PassingTest()
        {
            IntegerArgumentMarshaler m = new IntegerArgumentMarshaler();
            Iterator<string> i = new Iterator<string>(testData);

            m.Set(i);

            Assert.Equal(testData[0], m.Value.ToString());
            Assert.Equal(testData[1], i.Current.ToString());
        }

        [Fact]
        public void CreateReferenceAndSetIteratorToOutOfRange_FailingTest()
        {
            IntegerArgumentMarshaler m = new IntegerArgumentMarshaler();
            Iterator<string> i = new Iterator<string>(testData);

            for (int j = 0; j < this.testData.Count; j++)
            {
                i.Next();
            }

            ArgumentsException ex = Assert.Throws<IntegerArgumentMarshaler.IntegerMarshalerException>(() => m.Set(i));
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(ErrorCode.MISSING, ex.ErrorCode);
            Assert.Equal(this.testArgument, ex.ErrorArgumentId);

            Assert.Equal($"Could not find integer parameter for -{this.testArgument}", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndSetIteratorWithWrongFormat_FailingTest()
        {
            IntegerArgumentMarshaler m = new IntegerArgumentMarshaler();
            Iterator<string> i = new Iterator<string>(testData);

            for (int j = 0; j < (this.testData.Count - 1); j++)
            {
                i.Next();
            }

            ArgumentsException ex = Assert.Throws<IntegerArgumentMarshaler.IntegerMarshalerException>(() => m.Set(i));
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(ErrorCode.INVALID, ex.ErrorCode);
            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Equal(this.testData[(this.testData.Count - 1)], ex.ErrorParameter);

            Assert.Equal($"Argument -{this.testArgument} expects an integer but was '{this.testData[(this.testData.Count - 1)]}'", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndTestSchema_PassingTest()
        {
            IntegerArgumentMarshaler m = new IntegerArgumentMarshaler();
            Assert.Equal(testSchema, m.Schema);
        }

        [Fact]
        public void CreateExceptionWithErrorCode_PassingTest()
        {
            ArgumentsException ex = new IntegerArgumentMarshaler.IntegerMarshalerException(ErrorCode.MISSING);
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);

            Assert.Equal($"Could not find integer parameter for -{this.testArgument}", ex.ErrorMessage());
        }

        [Fact]
        public void CreateExceptionWithErrorCodeAndParameter_PassingTest()
        {
            ArgumentsException ex = new IntegerArgumentMarshaler.IntegerMarshalerException(ErrorCode.INVALID, this.testParameter);
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Equal(this.testParameter, ex.ErrorParameter);

            Assert.Equal($"Argument -{this.testArgument} expects an integer but was '{this.testParameter}'", ex.ErrorMessage());
        }

        [Fact]
        public void CreateException_FailingTest()
        {
            ArgumentsException ex = new IntegerArgumentMarshaler.IntegerMarshalerException(ErrorCode.OK);
           
            Assert.Null(ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);

            Assert.Equal(string.Empty, ex.ErrorMessage());
        }
    }
}
