using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace ValidationPOC.Domains
{
    public class Entity
    {
        private readonly List<string> errors = new List<string>();

        public JObject EnhancedData { get; set; }

        public void AddError(string error)
        {
            if (string.IsNullOrWhiteSpace(error)) return;
            errors.Add(error);
        }

        public string GetError()
        {
            if (errors.Count == 0) return string.Empty;
            return string.Join(Environment.NewLine, errors);
        }
    }
}
