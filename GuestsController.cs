using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    public class GuestsController : Controller
    {
        private readonly GuestService _service;

        public GuestsController(GuestService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var guests = await _service.GetAllAsync();
            return View(guests);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guest guest)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(guest);
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var guest = await _service.GetByIdAsync(id);
            if (guest == null)
            {
                return NotFound();
            }
            return View(guest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Guest guest)
        {
            if (id != guest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(id, guest);
                return RedirectToAction("Index");
            }
            return View(guest);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}