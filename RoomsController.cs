using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    public class RoomsController : Controller
    {
        private readonly RoomService _service;

        public RoomsController(RoomService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _service.GetAllAsync();
            return View(rooms);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Room room)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAsync(room);
                return RedirectToAction("Index");
            }
            return View(room);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var room = await _service.GetByIdAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(id, room);
                return RedirectToAction("Index");
            }
            return View(room);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}