using System;
using System.Collections.Generic;
using System.Text;

namespace TranscriptionService.models
{
    public class Transcription : ITranscription
    {
        private bool ok;
        private string errorMessage;
        private string transcription;
        private ITranscriptionRequest request;

        public Transcription()
        {
        }

        public bool Error()
        {
            return !ok;
        }

        public string GetError()
        {
            return errorMessage;
        }

        public ITranscriptionRequest GetResquest()
        {
            return request;
        }

        public string GetTranscription()
        {
            return transcription;
        }

        public bool Ok()
        {
            return ok;
        }

        public void SetErrorMessage(string error)
        {
            this.errorMessage = error;
        }

        public void SetOk(bool ok)
        {
            this.ok = ok;
        }

        public void SetResquest(ITranscriptionRequest request)
        {
            this.request = request;
        }

        public void SetTranscription(string transcription)
        {
            this.transcription = transcription;
        }
    }
}
