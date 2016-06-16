using System;

namespace SX.WebCore
{
    public sealed class SxSeoPhrase
    {
        public SxSeoPhrase(string phrase) {
            Text = phrase;
        }

        public string Text { get; set; }
        public int WordCount { get; set; }
    }
}
