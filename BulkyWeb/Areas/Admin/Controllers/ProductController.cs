using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;
namespace BulkyBookWeb.Areas.Admin.Controllers;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        List<Product> objProductsList=_unitOfWork.Product.GetAll().ToList();
        return View(objProductsList);
    }
    public IActionResult Upsert(int? id)
    {       
        ProductVM productVM = new()
        {
                CategoryList = _unitOfWork.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                }),
                Product=new Product()
        };
        if(id==null||id==0)
        {
            return View(productVM);
        }
        else
        {
            productVM.Product = _unitOfWork.Product.Get(u => u.ProductId == id);
            return View(productVM);
               
        }
        
    }

    [HttpPost]
    public IActionResult Upsert(ProductVM productVM,IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Product.Add(productVM.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product created successfylly";
            return RedirectToAction("Index");
        }
        else
        {
            productVM.CategoryList=_unitOfWork.Category.GetAll().Select(u=>new SelectListItem()
            {
                Text=u.Name,
                Value = u.CategoryId.ToString()
            });

            return View(productVM);
        }
        

    }
    

    
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        Product? productFromDb = _unitOfWork.Product.Get(u => u.ProductId == id);
        if (productFromDb == null)
        {
            return NotFound();
        }
        return View(productFromDb);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Product obj = _unitOfWork.Product.Get(u => u.ProductId == id);
        if (obj == null)
        {
            return NotFound();
        }
        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Product deleted successfully";
        return RedirectToAction("Index");

    }
}
