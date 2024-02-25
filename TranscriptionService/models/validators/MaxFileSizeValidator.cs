using System.IO;

namespace TranscriptionService.models.validators
{
    internal class MaxFileSizeValidator : IFileValidator
    {
        private int maxSize;

        public MaxFileSizeValidator(int maxSize)
        {
            this.maxSize = maxSize;
        }

        public bool Validate(object o)
        {

            if (o is FileInfo file)
            {
                return file.Length <= this.maxSize;
            }
            return false;
        }
    }
}