using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Util;

namespace Lucene48.Sample
{
    public class CaseInsensitiveWhiteSpaceAnalyser : Analyzer
    {
        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
        {
            var tokenizer = new WhitespaceTokenizer(LuceneVersion.LUCENE_48, reader);
            var lowercaseFilter = new LowerCaseFilter(LuceneVersion.LUCENE_48, tokenizer);

            return new TokenStreamComponents(tokenizer, lowercaseFilter);
        }
    }
}