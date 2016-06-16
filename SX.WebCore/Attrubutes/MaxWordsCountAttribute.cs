using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.Attrubutes
{
    public class MaxWordsCountAttribute : ValidationAttribute
    {
        public MaxWordsCountAttribute() { }

        private int _count;
        public MaxWordsCountAttribute(int count)
        {
            _count = count;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Значение не может превышать {0} слов", _count);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var wordsCount = value.ToString().Split(' ').Length;

            return wordsCount <= _count;
        }
    }
}
