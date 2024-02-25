namespace TranscriptionService.models
{
    public interface ITranscriptionRequest
    {
        string GetName();
        void SetName(string name);
        byte[] GetFile();
        void SetFile(byte[] file);
    }
}
