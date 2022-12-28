using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppWhareHouseSystem.Data.Models;
using WebAppWhareHouseSystem.Models;
using static WebAppWhareHouseSystem.Services.ProductServices;

namespace WebAppWhareHouseSystem.Services
{
    public class ProductServices : IProductServices
    {
        private readonly EFContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public ProductServices(EFContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }


        public ProductViewModel Create(ProductViewModel productViewModel)
        {

            Product product = new Product()
            {
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                ImagePath = productViewModel.ImagePath,
                BuyPrice = productViewModel.BuyPrice,
                SellPrice = productViewModel.SellPrice,
                Quantity = productViewModel.Quantity,
                Category = productViewModel.Category,
                ProductCode = productViewModel.ProductCode
            };

            _context.Add(product);
            _context.SaveChanges();

            productViewModel.Id = product.Id;

            return productViewModel;

        }




        public List<ProductViewModel> GetAllProducts()
        {
            var allProducts = _context.Product.Select(p => new ProductViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImagePath = p.ImagePath,
                BuyPrice = p.BuyPrice,
                SellPrice = p.SellPrice,
                Quantity = p.Quantity,
                Category = p.Category,
                ProductCode = p.ProductCode
            }).ToList();

            return allProducts;

        }      
            public async Task<List<ProductViewModel>> GetAllAsync()
            {
                var result = await _context.Product
                  
                    .ToListAsync();

                return _mapper.Map<List<ProductViewModel>>(result);
            }

            public async Task<ProductViewModel> GetByIdAsync(int id)
            {
                var product = await _context.Product.FirstOrDefaultAsync(c => c.Id == id);
                return _mapper.Map<ProductViewModel>(product);
            }

            public async Task CreateAsync(ProductViewModel product)
            {
                var productToAdd = _mapper.Map<Product>(product);
                if (product.ImageFile != null)
                {
                    using MemoryStream ms = new MemoryStream();
                    await product.ImageFile.CopyToAsync(ms);
                    productToAdd.ImageContent = ms.ToArray();
                    productToAdd.ImageMimeType = product.ImageFile.ContentType;
                }
                _context.Product.Add(productToAdd);
                await _context.SaveChangesAsync();
                product.Id = productToAdd.Id;
            }

            public async Task UpdateAsync(int id, ProductViewModel product)
            {
                var productToEdit = _mapper.Map<Product>(product);

                _context.Attach(new Product() { Id = id })
                    .CurrentValues.SetValues(productToEdit);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var productToDelete = await _context.Product
                    .FirstOrDefaultAsync(c => c.Id == id);
                _context.Remove(productToDelete);
                await _context.SaveChangesAsync();
            }

            public async Task<bool> ExistAsync(int id)
            {
                return await _context.Product.AnyAsync(c => c.Id == id);
            }

            public async Task<FileModel> UploadFileToFileSystem(IFormFile formfile)
            {
                var result = new FileModel();
                if (formfile == null)
                {
                    return result;
                }
                result.OriginalFileName = formfile.FileName;
                result.FileName = Path.GetRandomFileName() + Path.GetExtension(formfile.FileName);
                result.DatabaseValue = Path.Combine("Uploads", result.FileName);
                result.WwwRootPath = Path.Combine(_environment.WebRootPath, result.DatabaseValue);

                //Create upload directory if not exists
                var uploadsDir = Path.Combine(_environment.WebRootPath, "Uploads");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                using FileStream fs = new FileStream(result.WwwRootPath, FileMode.Create, FileAccess.Write);
                await formfile.CopyToAsync(fs);

                return result;
            }

           

        }

}


