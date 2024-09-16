using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using ExpertClinicTestTask.Domain.Core.Account;
using ExpertClinicTestTask.Domain.Core.Dtos.Account;
using ExpertClinicTestTask.Domain.Interfaces.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ExpertClinicTestTask.API.Controllers.Account;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreationDto userDto)
    {
        string device = Request.Headers["x-Device"];

        if (string.IsNullOrEmpty(device))
        {
            return BadRequest("Missing 'x-Device' header.");
        }
 
        if (!ValidateUser(userDto, device))
        {
            return BadRequest("Invalid user data.");
        }

        try
        {
            var user = await _accountService.CreateUser(userDto);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await (await _accountService.GetUsersAsync()).FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            return NotFound("User was not found.");
        }

        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> SearchUsers([FromQuery] string? lastName, [FromQuery] string? firstName,
        [FromQuery] string? patronymic, [FromQuery] string? phoneNumber, [FromQuery] string? email)
    {
        var users = await (await _accountService.GetUsersAsync())
            .Where(u => string.IsNullOrEmpty(lastName) || u.LastName.Contains(lastName))
            .Where(u => string.IsNullOrEmpty(firstName) || u.FirstName.Contains(firstName))
            .Where(u => string.IsNullOrEmpty(patronymic) || u.Patronymic.Contains(patronymic))
            .Where(u => string.IsNullOrEmpty(phoneNumber) || u.PhoneNumber.Contains(phoneNumber))
            .Where(u => string.IsNullOrEmpty(email) || u.Email.Contains(email))
            .ToListAsync();

        return Ok(users);
    }

    private bool ValidateUser(UserCreationDto userDto, string device)
    {
        switch (device)
        {
            case "mail":
                return !string.IsNullOrEmpty(userDto.FirstName) && !string.IsNullOrEmpty(userDto.Email) &&
                       IsValidEmail(userDto.Email);
            case "mobile":
                return !string.IsNullOrEmpty(userDto.PhoneNumber) && IsValidPhoneNumber(userDto.PhoneNumber);
            case "web":
                return !string.IsNullOrEmpty(userDto.LastName) && !string.IsNullOrEmpty(userDto.FirstName) &&
                       !string.IsNullOrEmpty(userDto.Patronymic) && userDto.DateOfBirth != null &&
                       !string.IsNullOrEmpty(userDto.PassportNumber) &&
                       IsValidPassportNumber(userDto.PassportNumber) &&
                       !string.IsNullOrEmpty(userDto.PlaceOfBirth) &&
                       !string.IsNullOrEmpty(userDto.PhoneNumber) && IsValidPhoneNumber(userDto.PhoneNumber) &&
                       !string.IsNullOrEmpty(userDto.RegistrationAddress);
            default:
                return false;
        }
    }

    private bool IsValidEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }

    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return Regex.IsMatch(phoneNumber, @"^7\d{10}$");
    }

    private bool IsValidPassportNumber(string passportNumber)
    {
        return Regex.IsMatch(passportNumber, @"^\d{4}\s\d{6}$");
    }
}