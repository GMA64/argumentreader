using ArgumentMarshalerLib;
using ArgumentsLib;
using DoubleMarshalerLib;
using IntegerMarshalerLib;
using StringMarshalerLib;
using System;
using System.Diagnostics;
using System.Linq;
using Xunit;

namespace ArgumentsLibTest
{
    public class ExceptionsTest
    {
        private readonly string testArgument = "argument";
        private readonly string testParameter = "parameter";

        [Fact]
        public void CreateExceptionErrorCodeOk_PassingTest()
        {
            LibraryArgumentException ex = new LibraryArgumentException(ErrorCode.OK, null);

            Assert.Null(ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);
            Assert.Equal("TILT: Should not be reached!", ex.ErrorMessage());
        }

        [Fact]
        public void CreateExceptionErrorCodeUnexpectedArgument_PassingTest()
        {
            LibraryArgumentException ex = new LibraryArgumentException(ErrorCode.UNEXPECTED_ARGUMENT, null);
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);
            Assert.Equal($"Argument -{this.testArgument} unexpected", ex.ErrorMessage());
        }

        [Fact]
        public void CreateExceptionErrorCodeInvalidArgumentName_PassingTest()
        {
            LibraryArgumentException ex = new LibraryArgumentException(ErrorCode.INVALID_ARGUMENT_NAME, null);
            ex.ErrorArgumentId = this.testArgument;

            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);
            Assert.Equal($"'-{this.testArgument}' is not a valid argument name", ex.ErrorMessage());
        }

        [Fact]
        public void CreateExceptionErrorCodeInvalidParameter_PassingTest()
        {
            LibraryArgumentException ex = new LibraryArgumentException(ErrorCode.INVALID_PARAMETER, null);
            ex.ErrorArgumentId = this.testArgument;
            ex.ErrorParameter = this.testParameter;

            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Equal(this.testParameter, ex.ErrorParameter);
            Assert.Equal($"'{this.testParameter}' is not a valid parameter", ex.ErrorMessage());
        }

        [Fact]
        public void CreateExceptionErrorCodeGlobal_PassingTest()
        {
            LibraryArgumentException ex = new LibraryArgumentException(ErrorCode.GLOBAL, null);
            ex.ErrorArgumentId = this.testArgument;
            ex.ErrorParameter = this.testParameter;

            Assert.Equal(this.testArgument, ex.ErrorArgumentId);
            Assert.Equal(this.testParameter, ex.ErrorParameter);
            Assert.Equal($"There was an ERROR with '{this.testParameter}'", ex.ErrorMessage());
        }

        [Fact]
        public void CreateExceptionErrorCodeNotAllowed_PassingTest()
        {
            LibraryArgumentException ex = new LibraryArgumentException(ErrorCode.MISSING, null);

            Assert.Null(ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);
            Assert.Equal(string.Empty, ex.ErrorMessage());
        }
    }
}
