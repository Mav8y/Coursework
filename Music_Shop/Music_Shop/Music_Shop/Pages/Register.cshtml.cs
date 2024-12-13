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
            // �������� ���������� ������
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // �������� ���������� �������
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "������ �� ���������.");
                return Page();
            }

            // �������� ������������� ������������
            var existingUser = await _userManager.FindByNameAsync(Login);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "������������ � ����� ������� ��� ����������.");
                return Page();
            }

            var existingEmail = await _userManager.FindByEmailAsync(Email);
            if (existingEmail != null)
            {
                ModelState.AddModelError(string.Empty, "������������ � ����� email ��� ����������.");
                return Page();
            }

            // �������� ������ ������������
            var user = new IdentityUser
            {
                UserName = Login,
                Email = Email
            };

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                // ����� ����� ������������ ������������ ����� �����������
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/My Profile");
            }

            // ���������� ������, ���� �������� �� �������
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            

            return Page();


        }
    }
}


