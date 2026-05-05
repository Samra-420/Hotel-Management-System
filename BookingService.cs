using HotelManagementSystem.Models;
using MongoDB.Driver;

namespace HotelManagementSystem.Services
{
    public class BookingService
    {
        private readonly IMongoCollection<Booking> _bookings;
        private readonly RoomService _roomService;
        private readonly GuestService _guestService;

        public BookingService(MongoDbService db, RoomService roomService, GuestService guestService)
        {
            _bookings = db.Bookings;
            _roomService = roomService;
            _guestService = guestService;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            var bookings = await _bookings.Find(_ => true).ToListAsync();
            foreach (var booking in bookings)
            {
                var guest = await _guestService.GetByIdAsync(booking.GuestId);
                var room = await _roomService.GetByIdAsync(booking.RoomId);
                booking.GuestName = guest?.FullName ?? "Unknown";
                booking.RoomNumber = room?.RoomNumber ?? "Unknown";
            }
            return bookings;
        }

        public async Task<Booking?> GetByIdAsync(string id)
        {
            return await _bookings.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Booking booking)
        {
            var room = await _roomService.GetByIdAsync(booking.RoomId);
            if (room != null)
            {
                int days = (booking.CheckOut - booking.CheckIn).Days;
                booking.TotalAmount = days * room.PricePerNight;
            }
            await _bookings.InsertOneAsync(booking);
        }

        public async Task DeleteAsync(string id)
        {
            await _bookings.DeleteOneAsync(b => b.Id == id);
        }
    }
}