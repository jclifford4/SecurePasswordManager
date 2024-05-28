using Xunit;
using SPM;
using EncryptionUtility;

namespace SPM.Tests
{
    public class EncryptionUnitTests
    {
        public EncryptionUnitTests()
        {
            // Only for tests
            Environment.SetEnvironmentVariable("ENCRYPTION_KEY", "+BMnIcKdA/q8ie2jlP3sCmjni9dEAWiYGZyn7gNPa3A=");

        }


        [Theory]
        [InlineData("mypassword")]
        [InlineData("anotherpassword")]
        public void EncryptPlainText_ShouldNotBeEmpty(string plainText)
        {
            // Arrange & Act
            string encryptedString = EncryptionUtil.EncryptString(plainText);

            // Assert
            Assert.NotEmpty(encryptedString);
        }

        [Theory]
        [InlineData("testpassword", "testpassword")]
        public void EncryptPlainText_ShouldNotBeUnique(string plainText, string plainText1)
        {
            // Arrange & Act
            string encryptedString = EncryptionUtil.EncryptString(plainText);
            string encryptedString1 = EncryptionUtil.EncryptString(plainText1);

            // Assert
            Assert.Equal(encryptedString, encryptedString1);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EncryptPlainText_WithEmptyorNull_ShouldThrowException(string plainText)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => EncryptionUtil.EncryptString(plainText));

        }

        [Theory]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        [InlineData("123456")]
        [InlineData("1234567")]
        [InlineData("012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678")]
        public void EncryptPlainText_WithImproperLength_ShouldThrowException(string plainText)
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentException>(() => EncryptionUtil.EncryptString(plainText));

        }
    }
}
