using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class UserDataService
    {
        private readonly AuthContext _context;

        public UserDataService(AuthContext context)
        {
            _context = context;
        }

        public void CreateUser(UserDTOIn user)
        {
            User newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                City = user.City,
                Country = user.Country,
                Address = user.Address,
                Role = user.Role,
                PasswordHash = user.Password,
                Email = user.Email,
                PostalCode = user.PostalCode,
                IsAdmin = user.IsAdmin,
                ActiveMember = user.ActiveMember,
                DateOfBirth = user.DateOfBirth,
                RegistrationDate = user.RegistrationDate
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();

        }

        public UserDTOOut? GetUser(string id)
        {
            User? user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            UserDTOOut outputUser = new UserDTOOut(user.Id, user.FirstName, user.LastName, user.City, user.Country, user.Address);
            return outputUser;

         }

        public List<UserDTOOut> GetUsers()
        {
            // Implementation here
            List<UserDTOOut> users = new List<UserDTOOut>();
            foreach (var user in _context.Users)
            {
                UserDTOOut userDTOOut = new UserDTOOut(user.Id, user.FirstName, user.LastName, user.City, user.Country, user.Address);
                users.Add(userDTOOut);
            }
            return users;
        }

        public UserDTOOut? UpdateUser(string id, UserDTOIn user)
        {
            // Implementation here
            User? userToUpdate = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (userToUpdate == null)
            {
                return null;
            }
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.City = user.City;
            userToUpdate.Country = user.Country;
            userToUpdate.Address = user.Address;
            userToUpdate.Role = user.Role;
            userToUpdate.PasswordHash = user.Password;
            userToUpdate.Email = user.Email;
            userToUpdate.PostalCode = user.PostalCode;
            userToUpdate.IsAdmin = user.IsAdmin;
            userToUpdate.ActiveMember = user.ActiveMember;
            userToUpdate.DateOfBirth = user.DateOfBirth;
            userToUpdate.RegistrationDate = user.RegistrationDate;

            _context.SaveChanges();

            UserDTOOut outputUser = new UserDTOOut(userToUpdate.Id, userToUpdate.FirstName, userToUpdate.LastName, userToUpdate.City, userToUpdate.Country, userToUpdate.Address);

            return outputUser;
        }

        public void DeleteUser(string id)
        {
            User? user = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                return;
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
