namespace SX.WebCore.ViewModels
{
    public class SxVMSiteSetting
    {
        public string Value { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
