using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TranscriptionService.services
{
    class TextFileService
    {
        #region Logger
        private static readonly ILog logger = LogManager.GetLogger(typeof(TextFileService));
        #endregion

        public TextFileService() { }


        public void Save(string fileName, string transcription)
        {
            try
            {
                string name = fileName.Replace(".mp3", ".txt");
                using (FileStream fs = new FileStream(name, FileMode.Create))
                {
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(transcription);
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception oops) {
                logger.Error(String.Format("Error saving file. The file: {0} could not be saved", fileName));
                logger.Debug(String.Format("{0}: {1}", oops.Message, oops.StackTrace));
                throw new Exception(String.Format("The file: {0} could not be saved", fileName));
            }
        }
    }
}
