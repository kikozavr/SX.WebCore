namespace SX.WebCore.ViewModels
{
    public sealed class SxVMEmployee
    {
        public string Id { get; set; }

        public SxVMAppUser User { get; set; }

        public string Email
        {
            get
            {
                return User != null ? User.Email : null;
            }
            set { }
        }

        public string NikName
        {
            get
            {
                return User != null ? User.NikName : null;
            }
            set { }
        }
    }
}
