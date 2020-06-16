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
    public class ReflectorTest
    {
        private string marshalers = @"Marshaler";
        private string[] schemas = { "", "*", "#", "##" };

        [Fact]
        public void CreateReferenceAndSetDirectory_PassingTest()
        {
            Reflector r = new Reflector(this.marshalers);

            foreach (string schema in schemas)
            {
                Assert.Equal(schema, r.GetInstanceBySchema(schema).Schema);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void CreateReferenceAndSetDirectory_FailingTest(string directory)
        {
            Reflector r;

            ArgumentsException ex = Assert.Throws<LibraryArgumentException>(() => r = new Reflector(directory));

            Assert.Equal(ErrorCode.GLOBAL, ex.ErrorCode);
            Assert.Equal($"Marshaler Directory: {string.Empty} not found!", ex.ErrorParameter);

            Assert.Equal($"There was an ERROR with 'Marshaler Directory: {string.Empty} not found!'", ex.ErrorMessage());
        }

        [Fact]
        public void CreateReferenceAndSetDirectoryToWrongPath_FailingTest()
        {
            string wrongPath = @"..\";
            Reflector r;

            ArgumentsException ex = Assert.Throws<LibraryArgumentException>(() => r = new Reflector(wrongPath));

            Assert.Equal(ErrorCode.GLOBAL, ex.ErrorCode);
            Assert.Equal($"Marshaler Directory: {wrongPath} does not contain *MarshalerLib.dll files!", ex.ErrorParameter);

            Assert.Equal($"There was an ERROR with 'Marshaler Directory: {wrongPath} does not contain *MarshalerLib.dll files!'", ex.ErrorMessage());
        }
    }
}
