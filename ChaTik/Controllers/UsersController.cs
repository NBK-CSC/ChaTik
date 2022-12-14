using ChaTik.Models;
using ChaTik.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChaTik.Controllers;

 public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;

    public UsersController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index() => View(_userManager.Users.ToList());

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = new User { Email = model.Email, UserName = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(string id)
    {
        User user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        EditUserViewModel model = new EditUserViewModel {Id = user.Id, Email = user.Email, Name = user.UserName};
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if(user!=null)
            {
                user.Email = model.Email;
                user.UserName = model.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<ActionResult> Delete(string id)
    {
        User user = await _userManager.FindByIdAsync(id);
        if (user != null)
        {
            IdentityResult result = await _userManager.DeleteAsync(user);
        }
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> ChangePassword(string id)
    {
        User user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Name = user.UserName};
        return View(model);
    }
 
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                var passwordValidator = 
                    HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                var passwordHasher =
                    HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
     
                IdentityResult result = 
                    await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                if(result.Succeeded)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Пользователь не найден");
            }
        }
        return View(model);
    }
}