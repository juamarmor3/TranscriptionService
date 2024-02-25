using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using TranscriptionService.models;
using TranscriptionService.models.validators;

namespace TranscriptionService.services
{
    public class Mp3FileService
    {
        #region
        private string url;
        private string[] validators = new string[] { "minSize(5k)", "maxSize(3M)", "format" };
        #endregion

        #region Logger
        private static readonly ILog logger = LogManager.GetLogger(typeof(Mp3FileService));
        #endregion

        public Mp3FileService(string url, string[] validators)
        {
            this.url = url;
            this.validators = validators;
        }

        #region Properties
        private int numberOfRequests = 0;

        public int NumberOfRequests { get => numberOfRequests; set => numberOfRequests = value; }
        #endregion
        public Queue<ITranscriptionRequest> BuildRequestQueue()
        {
            Queue<ITranscriptionRequest> queue = new Queue<ITranscriptionRequest>();
            try
            {
                logger.Debug("Initializing...");
                if (!Directory.Exists(url))
                {
                    logger.Error(String.Format("The directory: {0} does not exists.", this.url));
                    return null;
                }
                string[] filesOnDirectory = Directory.GetFiles(url, "*.mp3");
                foreach (string fileName in filesOnDirectory)
                {

                    FileInfo fileInfo = new FileInfo(fileName);
                    byte[] file = File.ReadAllBytes(fileName);
                    if (this.ValidateFile(fileInfo))
                    {
                        ITranscriptionRequest request = new TranscriptionRequest(fileName, file);
                        queue.Enqueue(request);
                        numberOfRequests++;
                    }
                    else
                    {
                        logger.Error(String.Format("The file: {0} is not valid! ", fileName));
                    }
                }
                logger.Debug("MP3 files queue... built!");
            }
            catch (DirectoryNotFoundException oops)
            {
                logger.Error(String.Format("The directory: {0} does not exists. Message: {1}", this.url, oops.Message));
                logger.Debug(String.Format("{0}: {1}", oops.Message, oops.StackTrace));
            }
            catch (UnauthorizedAccessException oops)
            {
                logger.Error(String.Format("Access not authorized to: {0}. Message: {1}", this.url, oops.Message));
                logger.Debug(String.Format("{0}: {1}", oops.Message, oops.StackTrace));
            }
            catch (Exception oops)
            {
                logger.Error(String.Format("Message: {0}", oops.Message));
                logger.Debug(String.Format("{0}: {1}", oops.Message, oops.StackTrace));
            }
            return queue;
        }

        private bool ValidateFile(FileInfo fileInfo)
        {
            List<IFileValidator> fileValidatorList= new List<IFileValidator>();
            foreach(var v in validators)
            {
                if (v.Contains("minSize"))
                    fileValidatorList.Add(new MinFileSizeValidator(ValidatorUtils.ParseMinSize(v)));
                if(v.Contains("maxSize"))
                    fileValidatorList.Add(new MaxFileSizeValidator(ValidatorUtils.ParseMaxSize(v)));

                if (v.Contains("format"))
                    fileValidatorList.Add(new Mp3FileFormatValidator());
            }

            foreach (var fileValidator in fileValidatorList) {
                if (!fileValidator.Validate(fileInfo)) return false;
            }


            return true;
        }
    }
}
