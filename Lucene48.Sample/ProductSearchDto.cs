using System.Collections.Generic;

namespace Lucene48.Sample
{
    public class ProductSearchDto
    {
        public string ProductId { get; set; }
        public string Description { get; set; }
        public string LargeImageUrl { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<ProductKeyword> ProductKeywords { get; set; }
        public int OrderCount { get; set; }
    }
}