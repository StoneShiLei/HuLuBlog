using System.Collections.Generic;
using System.IO;
using JiebaNet.Segmenter;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Com.Stone.HuLuBlog.Infrastructure.Jieba
{
    public class JiebaAnalyzer : Analyzer
    {
        public TokenizerMode mode;
        public JiebaAnalyzer(TokenizerMode Mode): base()
        {
            this.mode = Mode;
        }

        protected override TokenStreamComponents CreateComponents(string filedName, TextReader reader)
        {
            var tokenizer = new JiebaTokenizer(reader, mode);

            var tokenstream = (TokenStream)tokenizer;

            tokenstream.AddAttribute<ICharTermAttribute>();
            tokenstream.AddAttribute<IOffsetAttribute>();

            return new TokenStreamComponents(tokenizer, tokenstream);
        }
    }
}