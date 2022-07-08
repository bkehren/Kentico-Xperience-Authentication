using Microsoft.AspNetCore.Mvc;

namespace KXAuthentication.Components.ViewComponents.Footer
{
    public class FooterViewComponent : ViewComponent
    {
        public FooterViewComponent() { }

        public IViewComponentResult Invoke()
        {
            return View("~/Components/ViewComponents/MasterPage/Footer/Default.cshtml");
        }
    }
}