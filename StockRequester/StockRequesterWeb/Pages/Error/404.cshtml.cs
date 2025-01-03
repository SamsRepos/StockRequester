using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StockRequesterWeb.Pages.Error
{
    public class _404Model : PageModel
    {
        public void OnGet()
        {
            Response.StatusCode = 404;
        }
    }
} 