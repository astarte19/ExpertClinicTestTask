using ExpertClinicTestTask.Domain.Core.Account;
using ExpertClinicTestTask.Domain.Core.Dtos.Account;

namespace ExpertClinicTestTask.Domain.Interfaces.Account;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        this._accountRepository = accountRepository;
    }

    public async Task<User> CreateUser(UserCreationDto userCreationDto)
    {
        User user = new User
        {
            LastName = userCreationDto.LastName,
            FirstName = userCreationDto.FirstName,
            Patronymic = userCreationDto.Patronymic,
            DateOfBirth = userCreationDto.DateOfBirth,
            PassportNumber = userCreationDto.PassportNumber,
            PlaceOfBirth = userCreationDto.PlaceOfBirth,
            PhoneNumber = userCreationDto.PhoneNumber,
            Email = userCreationDto.Email,
            RegistrationAddress = userCreationDto.RegistrationAddress,
            LivingAddress = userCreationDto.LivingAddress
        };
        
        return await _accountRepository.CreateUser(user);
    }

    public async Task<IQueryable<User>> GetUsersAsync()
    {
        return await  _accountRepository.GetUsersAsync();
    }
}