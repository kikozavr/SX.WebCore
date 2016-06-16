using SX.WebCore.Abstract;

namespace SX.WebCore.Managers
{
    public sealed class SxSeoWordCounter : ISxSeoWordCounter
    {
        public int GetWordCount(SxSeoPhrase phrases)
        {
            string[] res = phrases.Text.Trim().Split(' ');
            return res.Length;
        }
    }
}
