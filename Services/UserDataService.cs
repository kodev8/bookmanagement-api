using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.Extensions.Configuration;
using WebApplication1.Services.Errors;

namespace WebApplication1.Services
{
    public class UserDataService
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthContext _context;
        private readonly IConfiguration _configuration;

        public UserDataService(UserManager<User> userManager, AuthContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }


        public async Task<string?> CreateUser(UserDTOIn user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            // check if username is already taken
            if (await _userManager.FindByNameAsync(user.UserName!) != null) return null;
            // check if email is already taken
            if (await _userManager.FindByEmailAsync(user.Email!) != null) return null;
            
            try
            {
                User newUser = new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    City = user.City,
                    Country = user.Country,
                    Address = user.Address,
                    Email = user.Email,
                    PostalCode = user.PostalCode,
                    ActiveMember = user.ActiveMember,
                    DateOfBirth = user.DateOfBirth,
                    RegistrationDate = user.RegistrationDate
                };

                var result = await _userManager.CreateAsync(newUser, user.Password!);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                var roleResult = await _userManager.AddToRoleAsync(newUser, user.Role.ToString());
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                await transaction.CommitAsync();
                return newUser.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<UserDTOOut?> GetUser(string id)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            var role = await _userManager.GetRolesAsync(user);
            return new UserDTOOut(
                user.Id,
                user.FirstName,
                user.LastName,
                user.UserName,
                user.Email,
                role.FirstOrDefault(),
                user.Country
            );
        }

        public async Task<List<UserDTOOut>> GetUsers()
        {

            List<User> users = await _userManager.Users.ToListAsync();
            List<UserDTOOut> usersDTOs = users.Select(user => new UserDTOOut(
                user.Id,
                user.FirstName,
                user.LastName,
                user.UserName,
                user.Email,
                _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                user.Country
            )).ToList();
            return usersDTOs;
        }

        public async Task<UserDTOOut?> UpdateUser(string id, UserDTOIn user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                User? userToUpdate = await _userManager.FindByIdAsync(id);
                if (userToUpdate == null)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
                userToUpdate.UserName = user.UserName;
                userToUpdate.City = user.City;
            userToUpdate.Country = user.Country;
                userToUpdate.Address = user.Address;
                userToUpdate.Email = user.Email;
            userToUpdate.PostalCode = user.PostalCode;
            userToUpdate.ActiveMember = user.ActiveMember;
            userToUpdate.DateOfBirth = user.DateOfBirth;
            userToUpdate.RegistrationDate = user.RegistrationDate;

                var result = await _userManager.UpdateAsync(userToUpdate);
                var deleteRoleResult = await _userManager.RemoveFromRoleAsync(userToUpdate, _userManager.GetRolesAsync(userToUpdate).Result.FirstOrDefault()!);
                var roleResult = await _userManager.AddToRoleAsync(userToUpdate, user.Role.ToString());

                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return null;
                }

                UserDTOOut outputUser = new UserDTOOut(
                    userToUpdate.Id,
                    userToUpdate.FirstName,
                    userToUpdate.LastName,
                    userToUpdate.UserName,
                    userToUpdate.Email,
                    _userManager.GetRolesAsync(userToUpdate).Result.FirstOrDefault(),
                    userToUpdate.Country
                    );

                await transaction.CommitAsync();
                return outputUser;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteUser(string id)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return false;
            }
            await _userManager.DeleteAsync(user);
            return true;
        }

        public async Task<bool> UpdateRole(string id, string role)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user == null) return false;
            await _userManager.RemoveFromRoleAsync(user, _userManager.GetRolesAsync(user).Result.FirstOrDefault()!);
            await _userManager.AddToRoleAsync(user, role);
            return true;
        }

        public async Task<ServiceResponse<LoginResponseDTO, LoginError>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return new ServiceResponse<LoginResponseDTO, LoginError>
                {
                    Error = LoginError.InvalidCredentials
                };
            }

            if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return new ServiceResponse<LoginResponseDTO, LoginError>
                {
                    Error = LoginError.InvalidCredentials
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = AuthUtils.GenerateJwtToken(
                user.Email!,
                roles.ToList(),
                _configuration["Jwt:Secret"]!
            );

            return new ServiceResponse<LoginResponseDTO, LoginError>
            {
                Data = new LoginResponseDTO(
                    token,
                    user.Email!,
                    roles.FirstOrDefault() ?? "User"
                ),
                Error = LoginError.None
            };
        }

       
    }
}
