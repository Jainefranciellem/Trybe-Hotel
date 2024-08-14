using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomDto> GetRooms(int HotelId) {
              return from room in _context.Rooms
                   where room.HotelId == HotelId
                   join hotel in _context.Hotels on room.HotelId equals hotel.HotelId
                   join city in _context.Cities on hotel.CityId equals city.CityId
                   select new RoomDto {
                       RoomId = room.RoomId,
                       Capacity = room.Capacity,
                       Image = room.Image,
                       Name = room.Name,
                       Hotel = new HotelDto {
                           HotelId = hotel.HotelId,
                           Address = hotel.Address,
                           CityId = city.CityId,
                           Name = hotel.Name,
                           CityName = city.Name,
                           State = hotel.City.State,
                       }
                   };
        }

        public RoomDto AddRoom(Room room) {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            var createdRoom = from rooms in _context.Rooms
                              where rooms.RoomId == room.RoomId
                              join hotel in _context.Hotels on rooms.HotelId equals hotel.HotelId
                              join city in _context.Cities on hotel.CityId equals city.CityId
                              select new RoomDto() {
                                  RoomId = rooms.RoomId,
                                  Name = rooms.Name,
                                  Image = rooms.Image,
                                  Capacity = rooms.Capacity,
                                  Hotel = new HotelDto() {
                                        HotelId = hotel.HotelId,
                                        CityId = city.CityId,
                                        Address = hotel.Address,
                                        Name = hotel.Name,
                                        CityName = city.Name,
                                        State = hotel.City.State,
                                  }
                              };

            return createdRoom.First();
        }

        public void DeleteRoom(int RoomId) {
            var findRoom = _context.Rooms.Find(RoomId);
            if (findRoom != null) _context.Rooms.Remove(findRoom);
        }
    }
}