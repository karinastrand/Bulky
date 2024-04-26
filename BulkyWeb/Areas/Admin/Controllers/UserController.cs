using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BulkyBook.Utility;
using Microsoft.EntityFrameworkCore;
namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class UserController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public UserController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
    {
       _db=db;
    }

    public IActionResult Index()
    {
      return View();    
    }
   
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        List<ApplicationUser> objUserList = _db.applicationUsers.Include(u=>u.Company).ToList();
        var userRoles = _db.UserRoles.ToList();
        var roles=_db.Roles.ToList();
        foreach (ApplicationUser user in objUserList) 
        {
            var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
             user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            if (user.Company == null) 
            {
                user.Company = new()
                {
                    Name = ""
                };
            }
        
        }

        return Json(new { data = objUserList });
    }
    [HttpPost]
    public IActionResult LockUnlock([FromBody]string id)
    {
        var objFromDb = _db.applicationUsers.FirstOrDefault(u => u.Id == id);
        if(objFromDb == null) 
        {
            return Json(new { success=false,message="Error while Locking/Unlocking" });
        }

        if(objFromDb.LockoutEnd!=null && objFromDb.LockoutEnd>DateTime.Now)
        {
            objFromDb.LockoutEnd = DateTime.Now;
        }
        else
        {
            objFromDb.LockoutEnd= DateTime.Now.AddYears(1000);
        }
        _db.SaveChanges();

        return Json(new { success = true, message = "Locking/Unlocking successful" });

    }

    #endregion
}
