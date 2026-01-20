using Microsoft.AspNetCore.Mvc;
using RupeeRoute.Web.Models;
using System.Text.Json;

public class AccountController : Controller
{
    private readonly ApiService api;

    public AccountController()
    {
        api = new ApiService();
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await api.LoginUser("users/login", model);

        if (!result.success)
        {
            ViewBag.Error = "Invalid email or password";
            return View(model);
        }

        var json = JsonDocument.Parse(result.response);

        /*bool isVaultEnabled = json.RootElement
            .GetProperty("isVaultEnabled")
            .GetBoolean();

        if (isVaultEnabled)
            return RedirectToAction("Vault", "Savings");*/

        return RedirectToAction("Index", "Dashboard");
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        if(model.Password != model.ConfirmPassword)
        {
            ViewBag.Error = "Passwords do not match";
            return View(model);
        }
        var error = await api.RegisterUser(model);

        if (error == null)
        {
            TempData["Success"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        ViewBag.Error = error;
        return View(model);
    }
}
