using Xunit;
using SPM;
using System.Text;
using EncryptionUtility;

namespace SPM.Tests
{
    public class EncryptionIntegrationTests
    {

        public EncryptionIntegrationTests()
        {
            // Only for tests
            Environment.SetEnvironmentVariable("ENCRYPTION_KEY", "+BMnIcKdA/q8ie2jlP3sCmjni9dEAWiYGZyn7gNPa3A=");

        }

        [Theory]
        [InlineData("testpassword")]
        public void EncryptAndDecryptPlainText_ShouldSucceed(string plainText)
        {
            // Arrange & Act
            string encryptedPassword = EncryptionUtil.EncryptString(plainText);
            string decryptedPassword = EncryptionUtil.DecryptString(encryptedPassword);

            // Assert
            Assert.NotEqual(encryptedPassword, decryptedPassword);
            Assert.Equal(plainText, decryptedPassword);
        }
    }
}
