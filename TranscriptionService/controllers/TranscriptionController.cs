using System;
using System.Collections.Generic;
using TranscriptionService.cache;
using TranscriptionService.models;
using TranscriptionService.services;
using log4net;
using log4net.Config;
using System.Threading.Tasks;
using TranscriptionService.mock;

namespace TranscriptionService.controllers
{
    class TranscriptionController
    {
        #region Attributes
        public string url;
        public string[] validators;
        #endregion

        #region Properties
        private AsyncTranscriptionCache transcriptionCache;
        private Queue<ITranscriptionRequest> transcriptionRequestsQueue;
        private Mp3FileService mp3FileService;
        private TextFileService fileService;
        private int requestCompleted = 0;
        #endregion

        #region Constants
        private const int ASYNC_QUEUE_MAX_LENGTH = 3;
        #endregion

        #region Logger
        private static readonly ILog logger = LogManager.GetLogger(typeof(TranscriptionController));
        #endregion

        private readonly string[] defaultValidators = {"format", "size"};

        public TranscriptionController(string url, string[] validators)
        {
            logger.DebugFormat("Initialize transcription controller...");
            this.url = url;
            this.validators = validators;
            transcriptionCache = new AsyncTranscriptionCache(new MockTranscriptionService());
            mp3FileService = new Mp3FileService(url, validators);
            fileService = new TextFileService();
            logger.DebugFormat("Initialize transcription events...");
            transcriptionCache.TranscriptionDone += this.GetTranscriptionDoneEvent;
            transcriptionCache.TranscriptionError += this.GetTranscriptionErrorEvent;
            logger.DebugFormat("Initalize done!");
        }

        public void BeginDailyTranscription()
        {
            logger.Debug("Initializing daily transcription...");
            try
            {
                transcriptionRequestsQueue = this.mp3FileService.BuildRequestQueue();
                if (transcriptionRequestsQueue.Count == 0)
                {
                    logger.Warn("The are no files to process today. The daily process has finished");
                    return;
                }
                do
                {
                    if (transcriptionCache.CurrentThreads < ASYNC_QUEUE_MAX_LENGTH && transcriptionRequestsQueue.Count > 0)
                    {
                        logger.Debug(String.Format("Used {0} of {1}", transcriptionCache.CurrentThreads + 1, ASYNC_QUEUE_MAX_LENGTH));
                        ITranscriptionRequest request = transcriptionRequestsQueue.Dequeue();
                        transcriptionCache.Send(request);
                    }
                } while (transcriptionRequestsQueue.Count > 0 || requestCompleted != mp3FileService.NumberOfRequests);
                logger.Debug("Daily transcription... Done!");
            }
            catch (Exception oops)
            {
                logger.Error(oops.Message);
                logger.Debug(String.Format("{0}: {1}", oops.Message, oops.StackTrace));
            }
        }

        private void GetTranscriptionDoneEvent(ITranscription transcription)
        {
            try
            {
                requestCompleted++;
                string fileName = transcription.GetResquest().GetName();
                fileService.Save(fileName, transcription.GetTranscription());
                logger.Info(String.Format("File: {0}. Successfully transcripted --> Request {1} of {2}", fileName, requestCompleted, mp3FileService.NumberOfRequests));
            } catch(Exception oops)
            {
                logger.Error(oops.Message);
                logger.Debug(String.Format("{0}: {1}", oops.Message, oops.StackTrace));
            }
        }

        private void GetTranscriptionErrorEvent(ITranscription transcription)
        {
            requestCompleted++;
            string fileName = transcription.GetResquest().GetName();
            logger.Error(String.Format("Error transcripting file: {0}. Request {1} of {2}", fileName, requestCompleted, mp3FileService.NumberOfRequests));
        }
    }
}