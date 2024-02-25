using System;
using System.ServiceProcess;
using System.Timers;
using TranscriptionService.controllers;

namespace TranscriptionService
{
    class TransciptionWindowsService
    {
        private Timer timer;

        public TransciptionWindowsService() {}

        protected void OnStart(string[] args)
        {
            DateTime now = DateTime.Now;
            DateTime midnight = now.Date.AddDays(1);
            TimeSpan timeUntilMidnight = midnight - now;

            timer = new Timer();
            timer.Interval = timeUntilMidnight.TotalMilliseconds;
            timer.Elapsed += GetTimerElapsed;
            timer.Start();
        }
        protected void OnStop()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        private void GetTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TranscriptionController contoller = new TranscriptionController("C:\\mp3", new string[] { "format", "minSize(40K)", "maxSize(5M)" });
            contoller.BeginDailyTranscription();
            timer.Interval = TimeSpan.FromHours(24).TotalMilliseconds;
        }
    }
}
