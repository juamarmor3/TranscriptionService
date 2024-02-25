using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TranscriptionService.mock
{
    public interface IMockTranscriptionService
    {
        Task<string> TranscriptMp3FileAsync(string fileName);
    }
}
