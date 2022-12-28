using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebAppWhareHouseSystem.Models;
using WebAppWhareHouseSystem.Services;

namespace WebAppWhareHouseSystem.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;

        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productServices.GetAllAsync();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _productServices.GetByIdAsync(id.Value);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create  ????????
        public async Task<IActionResult> Create()
        {

            return View();


        }


            // POST: Products/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var uplaodResult = await _productServices.UploadFileToFileSystem(product.ImageFile);
                product.ImagePath = uplaodResult.DatabaseValue;
                await _productServices.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

   /*    // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



        }

        */

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productServices.UpdateAsync(id, product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _productServices.ExistAsync(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _productServices.GetByIdAsync(id.Value);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productServices.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        /*   public ProductController(IProductServices productServices)
           {
               _productServices = productServices;
           }


           public IActionResult Index()
           {
               try
               {
                   var allProducts = _productServices.GetAllProducts();
                   return View(allProducts);
               }
               catch (Exception ex)
               {
                   return RedirectToAction("Index", new { Message = ex.Message });
               }


           }
        
        [HttpGet]
        public IActionResult Create()
        {
            var product = new ProductViewModel();
            return View(product);
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel productViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(productViewModel);
                }

                _productServices.Create(productViewModel);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { Message = ex.Message });
            }

        

    
   

         */
    }
}




