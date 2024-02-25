using System;
using System.Collections.Generic;
using System.Text;

namespace TranscriptionService.models
{
    public interface ITranscription
    {
        bool Ok();
        bool Error();
        ITranscriptionRequest GetResquest();
        string GetTranscription();
        string GetError();

        void SetOk(bool ok);
        void SetResquest(ITranscriptionRequest request);
        void SetTranscription(string transcription);
        void SetErrorMessage(string error);
    }
}
