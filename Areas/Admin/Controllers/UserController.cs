using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagament_MVC.Models;

namespace ProductManagament_MVC.Areas.Admin.Controllers;

[Area("Admin")]
public class UserController : Controller
{
    private readonly PM_Context _context;

    public UserController(PM_Context context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "User";

        var user = await _context.Users
            .Where(u => u.role == "Müşteri")
            .ToListAsync();
        
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Index(string username , string email , string phone , string password , string role , DateTime birthday)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var existUser = await _context.Users.FirstOrDefaultAsync(u => u.email == email);
                if (existUser != null)
                {
                    ViewData["Error"] = "Bu kullanıcı zaten kayıtlı";
                }
                else
                {
                    var newUser = new User()
                    {
                        userName = username,
                        email = email,
                        password = password,
                        phoneNumber = phone,
                        role = "Müşteri",
                        BirthDate = birthday,
                        CreatedAt = DateTime.Now
                    };

                    await _context.AddAsync(newUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
            }
        }

        return View();
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        
        return View(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(int id, string username, string email, string password, string address,
        string phoneNumber , DateTime birthDate)
    {
        try
        {
            var existUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (existUser == null)
            {
                return NotFound();
            }

            existUser.userName = username;
            existUser.email = email;
            existUser.password = password;
            existUser.Address = address;
            existUser.phoneNumber = phoneNumber;
            existUser.BirthDate = birthDate;

            _context.Users.Update(existUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index" , "User");
        }
        catch (Exception e)
        {
            ViewBag.Error = e.Message;
        }
        
        return View();
    }


    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (ModelState.IsValid)
        {
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.Error = "Silme işlemi başarısız oldu.";
            }
        }

        return View(user);
    }
}