using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Models;
using WEB.Data;
using WEB.ViewModel;

namespace WEB.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;
        private readonly IProviderRepository providerRepository;

        public ProductController(IMapper mapper, IProductRepository productRepository, IProviderRepository providerRepository)
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.providerRepository = providerRepository;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = mapper.Map<IEnumerable<ProductViewModel>>(await productRepository.GetProductsAndProviders());
            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productViewModel = mapper.Map<ProductViewModel>(await productRepository.GetById(id.Value));
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }


        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            var productViewmodel = await GetProvidersInProduct(new ProductViewModel());
            return View(productViewmodel);
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            var productViewmodel = await GetProvidersInProduct(productViewModel);
            if (ModelState.IsValid)
            {
                productViewModel.Id = Guid.NewGuid();

                var imgPrefix = Guid.NewGuid().ToString();

                if (productViewModel.ImageUpload != null)
                {
                    if (!(await UploadImage(imgPrefix, productViewModel.ImageUpload)))
                    {
                        return View(productViewModel);
                    }
                    productViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;
                }
                else 
                {
                    productViewModel.Image = imgPrefix + "default.jpg";
                }
                await productRepository.Add(mapper.Map<Product>(productViewModel));

                return RedirectToAction(nameof(Index));
            }

            return View(productViewModel);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productViewModel = mapper.Map<ProductViewModel>(await productRepository.GetById(id.Value));
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var product = await productRepository.GetById(id);

                var imgPrefix = Guid.NewGuid().ToString();
                if (productViewModel.ImageUpload != null)
                {
                    if (!(await UploadImage(imgPrefix, productViewModel.ImageUpload)))
                    {
                        return View(productViewModel);
                    }
                    product.Image = imgPrefix + productViewModel.ImageUpload.FileName;
                }

                product.Name = productViewModel.Name;
                product.Description = productViewModel.Description;
                product.Value = productViewModel.Value;        
                product.Active = productViewModel.Active;

                await productRepository.Update(product);

                return RedirectToAction(nameof(Index));
            }

            return View(productViewModel);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productViewModel = mapper.Map<ProductViewModel>(await productRepository.GetById(id.Value));
            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await productRepository.Remove(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductViewModelExists(Guid id)
        {
            return (await productRepository.GetById(id)) != null ? true : false;
        }

        private async Task<ProductViewModel> GetProvidersInProduct(ProductViewModel product)
        {
            product.Providers = mapper.Map<IEnumerable<ProviderViewModel>>(await providerRepository.GetAll());
            return product;
        }

        private async Task<bool> UploadImage(string imgPrefix, IFormFile file) 
        {
            if (file == null || file.Length <= 0)
            {
                return false;
            }

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgPrefix+file.FileName);

            if (System.IO.File.Exists(fullPath)) 
            {
                throw new Exception("File already Exists.");
            }

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            
            return true;
        }
    }
}
