using HotelManagementSystem.Models;
using MongoDB.Driver;

namespace HotelManagementSystem.Services
{
    public class RoomService
    {
        private readonly IMongoCollection<Room> _rooms;

        public RoomService(MongoDbService db)
        {
            _rooms = db.Rooms;
        }

        public async Task<List<Room>> GetAllAsync()
        {
            return await _rooms.Find(_ => true).ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(string id)
        {
            return await _rooms.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Room room)
        {
            await _rooms.InsertOneAsync(room);
        }

        public async Task UpdateAsync(string id, Room room)
        {
            await _rooms.ReplaceOneAsync(r => r.Id == id, room);
        }

        public async Task DeleteAsync(string id)
        {
            await _rooms.DeleteOneAsync(r => r.Id == id);
        }

        public async Task<List<Room>> GetAvailableRoomsAsync()
        {
            return await _rooms.Find(r => r.IsAvailable).ToListAsync();
        }
    }
}