using kxa = CMS.DocumentEngine.Types.KXSamples;

namespace KXAuthentication.Models.Page
{
    public class PageViewModel
    {
        public PageViewModel(kxa.Page pageData)
        {
            this.PageData = pageData;
        }

        public kxa.Page PageData { get; set; }
    }
}
