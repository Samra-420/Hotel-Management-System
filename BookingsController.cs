using HotelManagementSystem.Models;
using HotelManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly BookingService _bookingService;
        private readonly RoomService _roomService;
        private readonly GuestService _guestService;

        public BookingsController(BookingService bookingService, RoomService roomService, GuestService guestService)
        {
            _bookingService = bookingService;
            _roomService = roomService;
            _guestService = guestService;
        }

        public async Task<IActionResult> Index()
        {
            var bookings = await _bookingService.GetAllAsync();
            return View(bookings);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Rooms = await _roomService.GetAvailableRoomsAsync();
            ViewBag.Guests = await _guestService.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (ModelState.IsValid)
            {
                await _bookingService.CreateAsync(booking);
                return RedirectToAction("Index");
            }
            ViewBag.Rooms = await _roomService.GetAvailableRoomsAsync();
            ViewBag.Guests = await _guestService.GetAllAsync();
            return View(booking);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _bookingService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}