using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TranscriptionService.models.validators
{
    class Mp3FileFormatValidator : IFileValidator
    {
        public Mp3FileFormatValidator() { }
        public bool Validate(object o)
        {
            if (o is FileInfo file)
            {
                string url = file.FullName;

                try
                {
                    byte[] bytes = new byte[3];
                    using (FileStream fs = File.OpenRead(url))
                    {
                        fs.Read(bytes, 0, 3);
                    }

                    bool isMp3 = bytes[0] == 0x49 && bytes[1] == 0x44 && bytes[2] == 0x33;

                    return isMp3;
                }
                catch (Exception oops)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
