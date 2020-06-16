using ArgumentMarshalerLib;
using DoubleMarshalerLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace DoubleMarshalerLibTest
{
    public class DoubleMarshalerTest
    {
        private readonly string testSchema = "##";
        private readonly string testArgument = "argument";
        private readonly string testParameter = "parameter";
        private List<string> testData = new List<string>()
            {
                "1234.5678",
                "-nextcommand",
                "NotANumber"
            };

        [Fact]
        public void CreateReferenceAndSetNull_FailingTest()
        {
            DoubleArgumentMarshaler m = new DoubleArgumentMarshaler();

            Assert.Throws<NullReferenceException>(() => m.Set(null));
        }

        [Fact]
        public void CreateReferenceAndSetIterator_PassingTest()
        {
            DoubleArgumentMarshaler m = new DoubleArgumentMarshaler();
            Iterator<string> i = new Iterator<string>(testData);

            m.Set(i);

            Assert.Equal(Convert.ToDouble(testData[0]).ToString(), m.Value.ToString());
            Assert.Equal(testData[1], i.Current.ToString());
        }

        [Fact]
        public void CreateReferenceAndSetIteratorToOutOfRange_FailingTest()
        {
            DoubleArgumentMarshaler m = new DoubleArgumentMarshaler();
            Iterator<string> i = new Iterator<string>(testData);

            for (int j = 0; j < this.testData.Count; j++)
            {
                i.Next();
            }

            ArgumentsException ex = Assert.Throws<DoubleArgumentMarshaler.DoubleMarshalerException>(() => m.Set(i));
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(ErrorCode.MISSING, ex.ErrorCode);
            Assert.Equal(this.testArgument, ex.ErrorArgumentId);

            Assert.Equal($"Could not find double parameter for -{this.testArgument}", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndSetIteratorWithWrongFormat_FailingTest()
        {
            DoubleArgumentMarshaler m = new DoubleArgumentMarshaler();
            Iterator<string> i = new Iterator<string>(testData);

            for (int j = 0; j < (this.testData.Count - 1); j++)
            {
                i.Next();
            }

            ArgumentsException ex = Assert.Throws<DoubleArgumentMarshaler.DoubleMarshalerException>(() => m.Set(i));
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(ErrorCode.INVALID, ex.ErrorCode);
            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Equal(this.testData[(this.testData.Count - 1)], ex.ErrorParameter);

            Assert.Equal($"Argument -{this.testArgument} expects an double but was '{this.testData[(this.testData.Count - 1)]}'", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndTestSchema_PassingTest()
        {
            DoubleArgumentMarshaler m = new DoubleArgumentMarshaler();
            Assert.Equal(testSchema, m.Schema);
        }

        [Fact]
        public void CreateExceptionWithErrorCode_PassingTest()
        {
            ArgumentsException ex = new DoubleArgumentMarshaler.DoubleMarshalerException(ErrorCode.MISSING);
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);

            Assert.Equal($"Could not find double parameter for -{this.testArgument}", ex.ErrorMessage());
        }

        [Fact]
        public void CreateExceptionWithErrorCodeAndParameter_PassingTest()
        {
            ArgumentsException ex = new DoubleArgumentMarshaler.DoubleMarshalerException(ErrorCode.INVALID, this.testParameter);
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Equal(this.testParameter, ex.ErrorParameter);

            Assert.Equal($"Argument -{this.testArgument} expects an double but was '{this.testParameter}'", ex.ErrorMessage());
        }

        [Fact]
        public void CreateException_FailingTest()
        {
            ArgumentsException ex = new DoubleArgumentMarshaler.DoubleMarshalerException(ErrorCode.OK);

            Assert.Null(ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);

            Assert.Equal(string.Empty, ex.ErrorMessage());
        }
    }
}
