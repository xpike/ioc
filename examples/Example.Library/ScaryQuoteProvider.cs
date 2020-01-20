using System.Collections.Generic;

namespace Example.Library
{
    public class ScaryQuoteProvider
        : IScaryQuoteProvider
    {
        public IEnumerable<string> GetQuotes() =>
            new[]
            {
                "Resistance is futile.",
                "Is there something wrong with your chair, Captain?"
            };
    }
}