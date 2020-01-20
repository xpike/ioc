using System.Collections.Generic;

namespace Example.Library
{
    public class FunnyQuoteProvider
        : IFunnyQuoteProvider
    {
        public IEnumerable<string> GetQuotes() =>
            new[]
            {
                "I am NOT a merry man.",
                "Shut up, Wesley."
            };
    }
}