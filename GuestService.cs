using HotelManagementSystem.Models;
using MongoDB.Driver;

namespace HotelManagementSystem.Services
{
    public class GuestService
    {
        private readonly IMongoCollection<Guest> _guests;

        public GuestService(MongoDbService db)
        {
            _guests = db.Guests;
        }

        public async Task<List<Guest>> GetAllAsync()
        {
            return await _guests.Find(_ => true).ToListAsync();
        }

        public async Task<Guest?> GetByIdAsync(string id)
        {
            return await _guests.Find(g => g.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Guest guest)
        {
            await _guests.InsertOneAsync(guest);
        }

        public async Task UpdateAsync(string id, Guest guest)
        {
            await _guests.ReplaceOneAsync(g => g.Id == id, guest);
        }

        public async Task DeleteAsync(string id)
        {
            await _guests.DeleteOneAsync(g => g.Id == id);
        }
    }
}