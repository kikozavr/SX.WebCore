using System.ComponentModel.DataAnnotations;

namespace SX.WebCore.Attrubutes
{
    public class MinWordsCountAttribute : ValidationAttribute
    {
        public MinWordsCountAttribute() { }

        private int _count;
        public MinWordsCountAttribute(int count)
        {
            _count = count;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format("Значение не может быть меньше {0} слов", _count);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            var wordsCount = value.ToString().Split(' ').Length;

            return wordsCount >= _count;
        }
    }
}
