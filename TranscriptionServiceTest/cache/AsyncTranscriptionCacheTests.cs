using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TranscriptionService.cache;
using TranscriptionService.mock;
using TranscriptionService.models;

namespace TranscriptionServiceTest.cache
{
    [TestFixture]
    class AsyncTranscriptionCacheTests
    {
        private AsyncTranscriptionCache cache;
        private ITranscriptionRequest request;
        private byte[] file;
        

        [OneTimeSetUp]
        public void Setup()
        {
            file = null;
            request = new TranscriptionRequest("C:\\mp3\audio1.mp3", file);
        }

        [Test]
        public async Task SendSuccessfulAsync()
        {
            bool eventRaised = false;
            
            Mock<IMockTranscriptionService> mockService = new Mock<IMockTranscriptionService>();
            mockService.Setup(x => x.TranscriptMp3FileAsync(It.IsAny<string>()))
                .ReturnsAsync("Hola!");

            cache = new AsyncTranscriptionCache(mockService.Object);
            cache.TranscriptionDone += (result) =>
            {
                eventRaised = true;
            };

            await cache.Send(request);

            Assert.IsTrue(eventRaised);
        }

        [Test]
        public async Task SendErrorAsync()
        {
            bool eventRaised = false;

            Mock<IMockTranscriptionService> mockService = new Mock<IMockTranscriptionService>();
            mockService.Setup(x => x.TranscriptMp3FileAsync(It.IsAny<string>()))
               .ThrowsAsync(new Exception());
            
            cache = new AsyncTranscriptionCache(mockService.Object);
            cache.TranscriptionError += (result) =>
            {
                eventRaised = true;
            };
           

            await cache.Send(request);

            Assert.IsTrue(eventRaised);
        }
    }
}
