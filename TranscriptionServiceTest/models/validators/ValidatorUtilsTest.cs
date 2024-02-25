using NUnit.Framework;
using TranscriptionService.models.validators;

namespace TranscriptionServiceTest
{
    [TestFixture]
    public class ValidatorUtilsTest
    {
        [Test]
        public void ParseMinSizeSuccessfully()
        {
            string minSize = "minSize(5k)";
            int result = ValidatorUtils.ParseMinSize(minSize);
            Assert.AreEqual(result, 5000);
        }

        [Test]
        public void ParseMinSizeInvalidInputRetunsDefault()
        {
            string minSize = "5k";
            int result = ValidatorUtils.ParseMinSize(minSize);
            Assert.AreEqual(result, 50000);
        }

        [Test]
        public void ParseMaxSizeSuccessfully()
        {
            string minSize = "maxSize(5k)";
            int result = ValidatorUtils.ParseMaxSize(minSize);
            Assert.AreEqual(result, 5000);
        }

        [Test]
        public void ParseMaxSizeInvalidInputRetunsDefault()
        {
            string minSize = "5k";
            int result = ValidatorUtils.ParseMaxSize(minSize);
            Assert.AreEqual(result, 3000000);
        }
    }
}