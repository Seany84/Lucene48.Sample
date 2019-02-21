using System;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace Lucene48.Sample
{
    public class FuzzyQueryHandler : QueryHandler
    {
        private readonly IndexSearcher _searcher;

        public FuzzyQueryHandler(IndexSearcher searcher) : base(searcher)
        {
            _searcher = searcher;
        }

        public override TopDocs HandleQuery(string search, int? recordCount)
        {
            var bq = new BooleanQuery(disableCoord:true);
            var terms = search.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var term in terms)
            {
                bq.Add(new FuzzyQuery(new Term("ProductName", term), 2), Occur.SHOULD);
            }

            return _searcher.Search(bq, recordCount ?? 10);
        }
    }
}