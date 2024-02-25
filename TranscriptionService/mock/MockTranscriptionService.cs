using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TranscriptionService.mock
{
    public class MockTranscriptionService : IMockTranscriptionService
    {

        private Random transcriptionError = new Random();

        private List<string> transcriptedTexts = new List<string>()
        {
            "Paso a paso. Golpe a golpe. Asalto a asalto",
            "Bienvenidos a Jurassic Park",
            "- Dios crea al dinosaurio. Dios destruye al dinosaurio. Dios crea al hombre. El hombre destruye a Dios. El hombre crea al dinosaurio. - El dinosaurio se come al hombre... la mujer hereda la tierra",
            "Estoy a veinte minutos de allí. Llegaré en diez"
        };


        public MockTranscriptionService() { }

        public async Task<string> TranscriptMp3FileAsync(string fileName)
        {
            //Simulating an async wait of [1s-2s]
            //await Task.Delay(new Random().Next(1000, 2000));
            if (transcriptionError.Next(0, 100) < 5)
            {
                throw new Exception(String.Format("Error in the transcription of audio file: {0}", fileName));
            }

            return transcriptedTexts[new Random().Next(0, transcriptedTexts.Count)];
        }
    }
}
