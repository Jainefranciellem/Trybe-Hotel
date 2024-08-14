using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<HotelDto> GetHotels()
        {
              return from hotel in _context.Hotels
                            select new HotelDto
                            {
                                HotelId = hotel.HotelId,
                                Name = hotel.Name,
                                Address = hotel.Address,
                                CityId = hotel.CityId,
                                State = hotel.City.State,
                            };
        }
        
        public HotelDto AddHotel(Hotel hotel)
        {
            _context.Hotels.Add(hotel);
            _context.SaveChanges();

            var createdHotel = from h in _context.Hotels
                               where h.HotelId == hotel.HotelId
                               join city in _context.Cities on h.CityId equals city.CityId
                               select new HotelDto {
                                    CityId = city.CityId,
                                    HotelId = hotel.HotelId,
                                    Address = hotel.Address,
                                    CityName = city.Name,
                                    Name = hotel.Name,
                                    State = (from cities in _context.Cities
                                        where cities.CityId == hotel.CityId
                                        select cities.State).FirstOrDefault(),
                                    };
            return createdHotel.First();
        }
    }
}