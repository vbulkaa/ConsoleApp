using FlightManagement.DAL.models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.ASPnet.Controllers
{
    //отвечает за управление процессами аутентификации и регистрации пользователей
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUser model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    if (user.IsAdmin)
                    {
                        return RedirectToAction("Index", "Home"); // Перенаправление на страницу администратора
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); // Перенаправление для обычного пользователя
                    }
                }
                ModelState.AddModelError(string.Empty, "Неверный вход.");
            }
            return View(model);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginUser model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);
        //        if (result.Succeeded)
        //        {
        //            var user = await _userManager.FindByNameAsync(model.UserName);
        //            if (user != null)
        //            {
        //                // Проверка роли
        //                if (await _userManager.IsInRoleAsync(user, "Admin"))
        //                {
        //                    return RedirectToAction("Index", "Home"); // Перенаправление на страницу админа
        //                }
        //                return RedirectToAction("Index", "Home"); // Для обычных пользователей
        //            }
        //        }
        //        ModelState.AddModelError(string.Empty, "Неверный вход.");
        //    }
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUser model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Surname = model.Surname,
                    Name = model.Name,
                    MiddleName = model.MiddleName,
                    IsAdmin = false // регристрирующийся пользователь по умолчанию становится обычным
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded && user.IsAdmin == false)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                if (result.Succeeded)
                {
                    if (user.IsAdmin)
                    {
                        // Назначение роли Admin
                        await _userManager.AddToRoleAsync(user, "Admin");
                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
