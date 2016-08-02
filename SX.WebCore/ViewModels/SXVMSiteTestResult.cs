namespace SX.WebCore.ViewModels
{
    public sealed class SXVMSiteTestResult<TResult>
    {
        public SXVMSiteTestResult()
        {
            Results = new TResult[0];
        }

        public string SiteTestTitle { get; set; }
        public string SiteTestUrl { get; set; }
        public TResult[] Results { get; set; }
        public int BallsCount { get; set; }
    }
}
