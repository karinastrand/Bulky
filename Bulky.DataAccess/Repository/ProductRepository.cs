﻿

using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess.Repository;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private ApplicationDbContext _db;
    public ProductRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Product obj)
    {
        var objFromDb=_db.Products.FirstOrDefault(u=>u.ProductId == obj.ProductId);
        if (objFromDb != null) 
        {
            objFromDb.Title = obj.Title;
            objFromDb.Description = obj.Description;
            objFromDb.CategoryId = obj.CategoryId;
            objFromDb.ISBN = obj.ISBN;
            objFromDb.Price = obj.Price;
            objFromDb.ListPrice = obj.ListPrice;
            objFromDb.Price100 = obj.Price100;
            objFromDb.Price50 = obj.Price50;
            objFromDb.Author = obj.Author;
            objFromDb.ProductImages = obj.ProductImages;
            
           
        }
    }
}
