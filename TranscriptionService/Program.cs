using System;
using TranscriptionService.controllers;
using log4net.Config;

namespace TranscriptionService
{

    class Program
    {
        static void Main(string[] args)
        {
            TranscriptionController contoller = new TranscriptionController("C:\\mp3", new string[] { "format", "minSize(40K)", "maxSize(5M)" });
            contoller.BeginDailyTranscription();
        }
    }
}
