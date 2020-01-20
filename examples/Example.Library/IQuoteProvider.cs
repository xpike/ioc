using System.Collections.Generic;

namespace Example.Library
{
    public interface IQuoteProvider
    {
        IEnumerable<string> GetQuotes();
    }
}