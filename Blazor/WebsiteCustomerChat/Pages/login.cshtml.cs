using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebsiteCustomerChat.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Email { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Process the form submission here
            // For example, you can save the data to a database

            // Redirect to another page after processing
            // return RedirectToPage($"/login",);
            return Redirect($"/login?Name={Name}");
        }
    }
}