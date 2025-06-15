using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class AliasRequest
    {
        public string Url { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
    }
}