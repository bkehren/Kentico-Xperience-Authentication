using CMS.DocumentEngine.Types.KXSamples;

namespace KXAuthentication.Models.Home
{
    public class HomeViewModel
    {
        public HomeViewModel(HomePage pageData)
        {
            this.PageData = pageData;
        }

        public HomePage PageData { get; set; }
    }
}
