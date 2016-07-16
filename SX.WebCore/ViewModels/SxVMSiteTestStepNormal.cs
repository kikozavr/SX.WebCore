namespace SX.WebCore.ViewModels
{
    public sealed class SxVMSiteTestStepNormal
    {
        public int SubjectId { get; set; }
        public int QuestionId { get; set; }
        public int LettersCount { get; set; }

        //bals
        public int BallsSubjectShow { get; set; } = -8;
        public int BallsGoodRead { get; set; }
        public int BallsBadRead { get; set; }
    }
}
