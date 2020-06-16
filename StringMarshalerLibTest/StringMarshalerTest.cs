using ArgumentMarshalerLib;
using StringMarshalerLib;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Xunit;

namespace StringMarshalerLibTest
{
    public class StringMarshalerTest
    {
        private readonly string testSchema = "*";
        private readonly string testString = "test";
        private List<string> testData = new List<string>()
            {
                "Das ist ein Text",
                "-nextcommand"
            };

        [Fact]
        public void CreateReferenceAndSetNull_FailingTest()
        {
            StringArgumentMarshaler m = new StringArgumentMarshaler();

            Assert.Throws<NullReferenceException>(() => m.Set(null));
        }

        [Fact]
        public void CreateReferenceAndSetIteratorToFirst_PassingTest()
        {
            StringArgumentMarshaler m = new StringArgumentMarshaler();
            Iterator<string> i = new Iterator<string>(testData);

            m.Set(i);

            Assert.Equal(testData[0], m.Value);
            Assert.Equal(testData[1], i.Current);
        }

        [Fact]
        public void CreateReferenceAndSetIteratorToLast_FailingTest()
        {
            StringArgumentMarshaler m = new StringArgumentMarshaler();
            Iterator<string> i = new Iterator<string>(testData);

            for (int j = 0; j < this.testData.Count; j++)
            {
                i.Next();
            }

            ArgumentsException ex = Assert.Throws<StringArgumentMarshaler.StringMarshalerException>(() => m.Set(i));
            ex.ErrorArgumentId = this.testString;
            
            Assert.Equal(ErrorCode.MISSING, ex.ErrorCode);
            Assert.Equal($"Could not find string parameter for -{this.testString}", ex.ErrorMessage());
        }

        [Fact]
        public void CreateRefernceAndTestSchema_PassingTest()
        {
            StringArgumentMarshaler m = new StringArgumentMarshaler();
            Assert.Equal(testSchema, m.Schema);
        }

        [Fact]
        public void CreateException_PassingTest()
        {
            ArgumentsException ex = new StringArgumentMarshaler.StringMarshalerException(ErrorCode.MISSING);
            ex.ErrorArgumentId = this.testString;

            Assert.Null(ex.ErrorParameter);
            Assert.Equal($"Could not find string parameter for -{this.testString}", ex.ErrorMessage());
            Assert.NotEqual(string.Empty, ex.ErrorMessage());
        }

        [Fact]
        public void CreateException_FailingTest()
        {
            ArgumentsException ex = new StringArgumentMarshaler.StringMarshalerException(ErrorCode.OK);

            Assert.Null(ex.ErrorArgumentId);
            Assert.Null(ex.ErrorParameter);

            Assert.Equal(string.Empty, ex.ErrorMessage());
            Assert.NotEqual($"Could not find string parameter for -{this.testString}", ex.ErrorMessage());
        }
    }
}
