using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETChallengeWeb.Controllers
{
    public class ResponseBase
    {
        public string ValidationMessage { get; set; }
        public bool IsValid()
        {
            return string.IsNullOrWhiteSpace(ValidationMessage);
        }
    }
}
