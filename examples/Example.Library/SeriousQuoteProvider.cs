using System.Collections.Generic;

namespace Example.Library
{
    public class SeriousQuoteProvider
        : ISeriousQuoteProvider
    {
        public IEnumerable<string> GetQuotes() => 
            new[] 
            {
                "The only thing you have to fear is fear itself.",
                "Don't try to be a great man, just be a man."
            };
    }
}