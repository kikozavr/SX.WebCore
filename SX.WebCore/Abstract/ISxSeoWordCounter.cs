using System;

namespace SX.WebCore.Abstract
{
    public interface ISxSeoWordCounter
    {
        int GetWordCount(SxSeoPhrase phrases);
    }
}
