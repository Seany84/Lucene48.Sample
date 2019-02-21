using Lucene.Net.Search;

namespace Lucene48.Sample
{
    public abstract class QueryHandler
    {
        private readonly IndexSearcher _searcher;

        protected QueryHandler(IndexSearcher searcher)
        {
            _searcher = searcher;
        }

        public abstract TopDocs HandleQuery(string search, int? recordCount);
    }
}