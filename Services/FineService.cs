using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Contexts;
using WebApplication1.Models;
using System.Text.Json.Serialization;
using System.Linq;
using WebApplication1.Services.Errors;

namespace WebApplication1.Services
{
    public class FineService
    {
        private readonly BookDbContext _context;
        private readonly UserDataService _userService;

        public FineService(BookDbContext context, UserDataService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IEnumerable<FineDTOOut>> GetAllFines()
        {
            return await _context.Fines.Select(f => ToDTO(f)).ToListAsync();
        }

        public async Task<FineDTOOut?> GetFine(int id)
        {
            Fine? fine = await _context.Fines.FindAsync(id);
            if (fine == null) return null;
            return ToDTO(fine);
        }

        public async Task<IEnumerable<FineDTOOut>> GetUserFines(string userId)
        {
            return await _context.Fines.Where(f => f.UserId == userId).Select(f => ToDTO(f)).ToListAsync();
        }

        public async Task<ServiceResponse<FineDTOOut, BookLoanError>> AddFine(FineDTOIn fineDTO)
        {

            // check if user exists
            if (await _userService.GetUser(fineDTO.UserId!) == null) return new ServiceResponse<FineDTOOut, BookLoanError> { Error = BookLoanError.UserNotFound };

            Fine? fine = await _context.Fines.FindAsync(fineDTO.BookLoanId);

            // check if we found a fine
            if (fine != null)
                return new ServiceResponse<FineDTOOut, BookLoanError> { Error = BookLoanError.FineAlreadyIssued };

            fine = new Fine 
            {
                BookLoanId = fineDTO.BookLoanId,
                UserId = fineDTO.UserId,
                Amount = fineDTO.Amount,
                IssuedDate = DateTime.Now,
                PaidDate = fineDTO.PaidDate,
                Reason = fineDTO.Reason ?? string.Empty,
            };

            await _context.Fines.AddAsync(fine);
            await _context.SaveChangesAsync();
            return new ServiceResponse<FineDTOOut, BookLoanError>
            {
                Data = ToDTO(fine),
                Error = BookLoanError.None
            };
        }

        public async Task<ServiceResponse<FineDTOOut, BookLoanError>> UpdateFineDetails(int id, FineDTOInPartial fineDTO)
        {
            Fine? fine = await _context.Fines.FindAsync(id);
            if (fine == null) return new ServiceResponse<FineDTOOut, BookLoanError> { Error = BookLoanError.FineNotFound };
            fine.Status = fineDTO.Status.ToString();
            fine.PaidDate = fineDTO.PaidDate;
            await _context.SaveChangesAsync();
            return new ServiceResponse<FineDTOOut, BookLoanError> { Data = ToDTO(fine), Error = BookLoanError.None };
        }


        private static FineDTOOut ToDTO(Fine fine)
        {
            return new FineDTOOut(fine.Id, fine.BookLoanId, fine.UserId, fine.Amount, fine.IssuedDate, fine.PaidDate, Enum.Parse<EFineStatus>(fine.Status), fine.Reason);
        }

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EFineStatus { Pending, Paid, Waived }
}
