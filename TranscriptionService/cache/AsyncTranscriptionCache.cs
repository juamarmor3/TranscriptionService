using log4net;
using System;
using System.Threading.Tasks;
using TranscriptionService.mock;
using TranscriptionService.models;

namespace TranscriptionService.cache
{
    public class AsyncTranscriptionCache
    {
        public delegate void TranscriptionDelegate(ITranscription transcription);

        #region Events
        public event TranscriptionDelegate TranscriptionDone;
        public event TranscriptionDelegate TranscriptionError;
        #endregion

        # region Constants 
        public const int MAX_RETRIES = 3;
        #endregion

        #region Properties
        private int currentThreads = 0;
        private readonly IMockTranscriptionService mockService;

        public int CurrentThreads { get => currentThreads; set => currentThreads = value; }
        #endregion


        #region Logger
        private static readonly ILog logger = LogManager.GetLogger(typeof(AsyncTranscriptionCache));
        #endregion

        public AsyncTranscriptionCache(IMockTranscriptionService mock)
        {
            this.mockService = mock;
        }

        public async Task Send(ITranscriptionRequest request)
        {
            CurrentThreads++;
            bool success = false;
            int currentRetries = 0;
            do
            {
                try
                {
                    string transcriptionFile = await mockService.TranscriptMp3FileAsync(request.GetName());
                    success = true;
                    ITranscription transcription = new Transcription();
                    transcription.SetOk(transcriptionFile.Length > 0);
                    transcription.SetResquest(request);
                    transcription.SetTranscription(transcriptionFile);
                    CurrentThreads--;
                    logger.Debug(String.Format("File: {0} successfully transcripted!", request.GetName()));
                    this.TranscriptionDone(transcription);
                }
                catch (Exception oops)
                {
                    currentRetries++;
                    logger.Debug(String.Format("Error transcripting file: {0}. {1} of {2}", request.GetName(), currentRetries, MAX_RETRIES));
                }
            } while (currentRetries < MAX_RETRIES && !success);
           
            if (success || currentRetries < MAX_RETRIES) return;
            
            ITranscription error = new Transcription();
            error.SetOk(false);
            error.SetResquest(request);
            error.SetErrorMessage("reties error");
            CurrentThreads--;
            logger.Debug(String.Format("Max retries reached for file: {0}", request.GetName()));
            TranscriptionError(error);
        }
    }
}
