using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<CityDto> GetCities()
        {
            var response = from cities in _context.Cities
                         select new CityDto()
                         {
                             CityId = cities.CityId,
                             Name = cities.Name,
                             State = cities.State,
                         };
            return response;
        }

        public CityDto AddCity(City city)
        {
            var createdCity = _context.Cities.Add(city);
            _context.SaveChanges();

            return new CityDto {
                CityId = createdCity.Entity.CityId, 
                Name = createdCity.Entity.Name,
                State = createdCity.Entity.State
            };
        }

        public CityDto UpdateCity(City city)
        {
            _context.Cities.Update(city);
           _context.SaveChanges();
            var updated = _context.Cities.First(c => c.CityId == city.CityId);
            return new CityDto {
                    CityId = updated.CityId,
                    Name = updated.Name,
                    State = updated.State
            };
        }
    }
}