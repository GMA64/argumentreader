using ArgumentMarshalerLib;
using BooleanMarshalerLib;
using System;
using System.Collections.Generic;
using Xunit;

namespace BooleanMarshalerLibTest
{
    public class BooleanMarshalerTest
    {
        private List<string> testData = new List<string>()
            {
                "-a",
                "-b"
            };

        [Fact]
        public void CreateReferenceAndNoSet_PassingTest()
        {
            BooleanArgumentMarshaler m = new BooleanArgumentMarshaler();

            Assert.Null(m.Value);
        }

        [Fact]
        public void CreateReferenceAndSetNull_PassingTest()
        {
            BooleanArgumentMarshaler m = new BooleanArgumentMarshaler();

            m.Set(null);

            Assert.True((bool)m.Value);
        }

        [Fact]
        public void CreateReferenceAndSetTrue_PassingTest()
        {
            BooleanArgumentMarshaler m = new BooleanArgumentMarshaler();
            Iterator<string> i = new Iterator<string>(testData);

            m.Set(i);

            Assert.True((bool)m.Value);
            Assert.Equal(testData[0], i.Current);
        }
    }
}
