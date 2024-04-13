using Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Bulky.Models;
using Bulky.DataAccess.Repository.IRepository;


namespace BulkyWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepo;
    public CategoryController(ICategoryRepository categoryrepo)
    {
        _categoryRepo = categoryrepo;
    }
    public IActionResult Index()
    {
        List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
        return View(objCategoryList);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category obj) 
    {
        if (ModelState.IsValid) 
        {
            _categoryRepo.Add(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category created successfylly";
            return RedirectToAction("Index");
        }
        return View();
       
    }
    public IActionResult Edit(int? id)
    {
        if (id==null || id==0) 
        {
            return NotFound();
        }
        Category? categoryFromDb = _categoryRepo.Get(u=>u.CategoryId==id);
        if (categoryFromDb==null) 
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    [HttpPost]
    public IActionResult Edit(Category obj)
    {
        if (ModelState.IsValid)
        {
            _categoryRepo.Update(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category updated successfylly";
            return RedirectToAction("Index");
        }
        return View();

    }
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        Category? categoryFromDb = _categoryRepo.Get(u=>u.CategoryId==id);
        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }

    [HttpPost,ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Category obj= _categoryRepo.Get(u => u.CategoryId == id);
        if (obj==null)
        {
            return NotFound();
        }
        _categoryRepo.Remove(obj);
        _categoryRepo.Save();
        TempData["success"] = "Category deleted successfylly";
        return RedirectToAction("Index");
        
    }
}
