using DemoIdentity.Identity.Account;
using DemoIdentity.Identity;
using DemoIdentity.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoIdentity.Identity.Account;

public class RegisterModel : PageModel
{
    private readonly UserManager<MyUser> _userManager;
    private readonly SignInManager<MyUser> _signInManager;

    [BindProperty]
    public RegisterDTO Register { get; set; }
    public string ReturnUrl { get; set; }

    public RegisterModel(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (Register.Password != Register.Password2)
        {
            ModelState.AddModelError(string.Empty, "Passwords no coinciden");
            return Page();
        }

        var user = new MyUser
        {
            NroLegajo = Register.Legajo,
            Email = Register.Email,
            UserName = Register.Email
        };

        var res = await _userManager.CreateAsync(user, Register.Password);

        if (!res.Succeeded)
        {
            foreach (var err in res.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Description);
            }
            return Page();
        }

        // Opcional: iniciar sesión después de registrarse
        await _signInManager.SignInAsync(user, isPersistent: false);

        if (string.IsNullOrEmpty(ReturnUrl))
        {
            ReturnUrl = "/";
        }

        return LocalRedirect(ReturnUrl);
    }
}