using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripSplit.Domain.Dto;

namespace TripSplit.Domain
{
    public static class MappingProfile
    {
        public static User RegisterDtoToUser(RegisterDto registerDto)
        {
            return new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
            };
        }

        public static UserDto UserToUserDto(User user) 
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public static User UserDtoToUser(UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id,
                Email = userDto.Email,
                UserName = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
            };
        }

        public static Trip CreateTripDtoToTrip(CreateTripDto createTripDto)
        {
            return new Trip
            {
                Name = createTripDto.Name,
                Destination = createTripDto.Destination,
                Description = createTripDto.Description,
                StartDate = createTripDto.StartDate,
                EndDate = createTripDto.EndDate,
            };
        }

        public static TripDto TripToTripDto(Trip trip)
        {
            return new TripDto
            {
                Id = trip.Id,
                Name = trip.Name,
                Destination = trip.Destination,
                Description = trip.Description,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
            };
        }

        public static Expense CreateExpenseDtoToExpense(CreateExpenseDto createExpenseDto)
        {
            return new Expense
            {
                Name = createExpenseDto.Name,
                Amount = createExpenseDto.Amount,
                Description = createExpenseDto.Description,
                Date = createExpenseDto.Date,
                TripId = createExpenseDto.TripId
            };
        }

        public static ExpenseDto ExpenseToExpenseDto(Expense expense)
        {
            return new ExpenseDto
            {
                Id = expense.Id,
                TripId = expense.TripId,
                Name = expense.Name,
                Amount = expense.Amount,
                Description = expense.Description,
                Date = expense.Date,
            };
        }
    }
}
