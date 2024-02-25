using System;
using System.Collections.Generic;
using System.Text;

namespace TranscriptionService.models.validators
{
    interface IFileValidator
    {
        bool Validate(object o);
    }
}
