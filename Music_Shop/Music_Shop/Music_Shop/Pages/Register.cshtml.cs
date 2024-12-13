using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Music_Shop.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public RegisterModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public string Login { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // ѕроверка валидности модели
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // ѕроверка совпадени€ паролей
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "ѕароли не совпадают.");
                return Page();
            }

            // ѕроверка существующего пользовател€
            var existingUser = await _userManager.FindByNameAsync(Login);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "ѕользователь с таким логином уже существует.");
                return Page();
            }

            var existingEmail = await _userManager.FindByEmailAsync(Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError(string.Empty, "ѕользователь с таким email уже существует.");
                return Page();
            }

            // —оздание нового пользовател€
            var user = new IdentityUser
            {
                UserName = Login,
                Email = Email
            };

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                // ћожно сразу авторизовать пользовател€ после регистрации
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/My Profile");
            }

            // ƒобавление ошибок, если создание не удалось
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            

            return Page();


        }
    }
}


