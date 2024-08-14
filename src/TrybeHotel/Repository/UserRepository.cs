using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var users = _context.Users;
            var user = users.FirstOrDefault(user => user.Password == login.Password && user.Email == login.Email);
            if (user == null) return null;

            var response = new UserDto() {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };
            return response;

        }
        public UserDto Add(UserDtoInsert user)
        {
           var toCreate = new User()
           {
               Name = user.Name,
               Password = user.Password,
               Email = user.Email,
               UserType = "client"
           };

           _context.Users.Add(toCreate);
           _context.SaveChanges();

            var result = new UserDto() {
            UserId = toCreate.UserId,
            Email = toCreate.Email,
            Name = toCreate.Name,
            UserType = toCreate.UserType,
           };

           return result;
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) return null;

            var result = new UserDto {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                UserType = user.UserType
            };

            return result;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = from user in _context.Users
                       select new UserDto {
                           Email = user.Email,
                           Name = user.Name,
                           UserId = user.UserId,
                           UserType = user.UserType
                       };
            return users.ToList();
        }

    }
}