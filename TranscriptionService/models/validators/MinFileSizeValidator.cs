using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TranscriptionService.models.validators
{
    class MinFileSizeValidator : IFileValidator
    {
        private int minSize; 

        public MinFileSizeValidator(int minSize)
        {
            this.minSize = minSize;
        }

        public bool Validate(object o)
        {

            if (o is FileInfo file)
            {
                return file.Length >= this.minSize;
            }
            return false;
        }
    }
}
