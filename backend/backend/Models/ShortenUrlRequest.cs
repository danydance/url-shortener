using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class ShortenUrlRequest
    {
        public string Url { get; set; } = string.Empty;
    }
}