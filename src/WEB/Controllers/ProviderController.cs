using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB.ViewModel;

namespace WEB.Controllers
{
    [Authorize]
    public class ProviderController : Controller
    {
        private readonly IMapper mapper;
        private readonly IProviderRepository providerRepository;
        private readonly IAddressRepository addressRepository;

        public ProviderController(IMapper mapper, IProviderRepository providerRepository, IAddressRepository addressRepository)
        {
            this.mapper = mapper;
            this.providerRepository = providerRepository;
            this.addressRepository = addressRepository;
        }
    

        // GET: Provider
        public async Task<IActionResult> Index()
        {
            var products = mapper.Map<IEnumerable<ProviderViewModel>>(await providerRepository.GetAll());
            return View(products);
        }

        // GET: Provider/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerViewModel = mapper.Map<ProviderViewModel>(await providerRepository.GetById(id.Value));

            if (providerViewModel == null)
            {
                return NotFound();
            }

            return View(providerViewModel);
        }

        // GET: Provider/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Provider/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProviderViewModel providerViewModel)
        {
            if (ModelState.IsValid)
            {
                providerViewModel.Id = Guid.NewGuid();
                await providerRepository.Add(mapper.Map<Provider>(providerViewModel));
                return RedirectToAction(nameof(Index));
            }
            return View(providerViewModel);
        }

        // GET: Provider/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerViewModel = mapper.Map<ProviderViewModel>(await providerRepository.GetProviderAndAddress(id.Value));

            if (providerViewModel == null)
            {
                return NotFound();
            }
            return View(providerViewModel);
        }

        // POST: Provider/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProviderViewModel providerViewModel)
        {
            if (id != providerViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await providerRepository.Update(mapper.Map<Provider>(providerViewModel));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!(await ProviderViewModelExists(providerViewModel.Id)) )
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
            return View(providerViewModel);
        }

        // GET: Provider/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var providerViewModel = mapper.Map<ProviderViewModel>(await providerRepository.GetById(id.Value));

            if (providerViewModel == null)
            {
                return NotFound();
            }

            return View(providerViewModel);
        }

        // POST: Provider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await providerRepository.Remove(id);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> EditAddress(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var addressViewModel = mapper.Map<AddressViewModel>(await addressRepository.GetById(id.Value));
            if (addressViewModel == null)
            {
                return NotFound();
            }
            return PartialView("_EditAddress", addressViewModel);
        }


        // POST: Address/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(Guid id, AddressViewModel addressViewModel)
        {
            if (id != addressViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var provider = await providerRepository.GetById(addressViewModel.ProviderId);
                provider.Address = mapper.Map<Address>(addressViewModel);
                await providerRepository.Update(provider);

                return PartialView("_ListAddress", addressViewModel);
            }
            return BadRequest();
        }

        private async Task<bool> ProviderViewModelExists(Guid id)
        {
            return ( await providerRepository.GetById(id) ) != null ? true : false;
        }
    }
}
