using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    [BindProperty]
    public string? Login { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public string? ErrorMessage { get; set; }

    public LoginModel(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public void OnGet()
    {
        // Очистка сообщения об ошибке при первом запросе
        ErrorMessage = null;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Логин и пароль обязательны для заполнения.";
            return Page();
        }

        // Попытка входа
        var result = await _signInManager.PasswordSignInAsync(Login, Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        else
        {
            ErrorMessage = "Неверный логин или пароль.";
            return Page();
        }
    }
}
