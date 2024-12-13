using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize]
public class MyProfileModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public MyProfileModel(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty]
    public string Login { get; set; }

    [BindProperty]
    public string Email { get; set; }

    public string AvatarUrl { get; set; } = "/images/default-avatar.jpg"; // Путь к изображению по умолчанию

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToPage("/Login");
        }

        Login = user.UserName;
        Email = user.Email;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToPage("/Login");
        }

        if (user.UserName != Login)
        {
            var setUserNameResult = await _userManager.SetUserNameAsync(user, Login);
            if (!setUserNameResult.Succeeded)
            {
                foreach (var error in setUserNameResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }

        if (user.Email != Email)
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, Email);
            if (!setEmailResult.Succeeded)
            {
                foreach (var error in setEmailResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }

        TempData["SuccessMessage"] = "Изменения успешно сохранены.";
        return RedirectToPage();
    }
}
