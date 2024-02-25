using System;
using System.Collections.Generic;
using System.Text;

namespace TranscriptionService.models
{
    public class TranscriptionRequest: ITranscriptionRequest
    {
        private string name;
        private byte[] file;

        public TranscriptionRequest(string name, byte[] file)
        {
            this.name = name;
            this.file = file;
        }

        public byte[] GetFile()
        {
            return file;
        }

        public string GetName()
        {
            return name;
        }

        public void SetFile(byte[] file)
        {
            this.file = file;
        }

        public void SetName(string name)
        {
            this.name = name;
        }
    }
}
