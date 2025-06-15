using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Entities
{
    public class ShortenedUrl
    {
        public Guid Id { get; set; }
        public string LongUrl { get; set; } = string.Empty; // normal url
        public string ShortUrl { get; set; } = string.Empty; // http://localhost:???/???
        public string Code { get; set; } = string.Empty; // The code can be also alias
        public DateTime UTCTime { get; set; } // normal time

    }
}