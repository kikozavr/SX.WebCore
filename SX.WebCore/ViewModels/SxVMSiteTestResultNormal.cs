namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTestResultNormal
    {
        public string SubjectTitle { get; set; }

        public string QuestionText { get; set; }

        public bool IsCorrect { get; set; }

        public SxVMSiteTestStepNormal Step { get; set; }

        public int SecondCount(int lettersInSecond)
        {
            return Step.LettersCount / lettersInSecond;
        }
    }
}
