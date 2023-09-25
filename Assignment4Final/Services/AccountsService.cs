using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Assignment4Final.Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ModelLibrary.Models;
using ModelLibrary.Models.DTO.Accounts;

namespace Assignment4Final.Services;

public class AccountsService
{
    private readonly IAccountsRepository _repository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AccountsService(
        IAccountsRepository repository,
        IMapper mapper,
        IConfiguration configuration
    )
    {
        _repository = repository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public List<string> GetAllClaims()
    {
        return _repository.GetAllClaims();
    }

    public async Task<List<UserDto>> GetListUsers()
    {
        var users = await _repository.ListUsers();
        var userDtos = _mapper.Map<List<UserDto>>(users);

        // TODO:(akotro) Benchmark this
        var userRoles = await _repository.GetUserRoles(users);
        foreach (var user in userDtos)
        {
            if (userRoles.ContainsKey(user.Email))
            {
                user.Role = userRoles[user.Email];
            }
        }
        return userDtos;
    }

    public async Task<UserDto> GetUser(string email)
    {
        var user = await _repository.GetAppUser(email);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Role = _repository.GetUserRole(email);
        return userDto;
    }

    public async Task MakeAdmin(string email)
    {
        await _repository.MakeAdmin(email);
    }

    public async Task RemoveAdmin(string email)
    {
        await _repository.RemoveAdmin(email);
    }

    public async Task MakeMarker(string email)
    {
        await _repository.MakeMarker(email);
    }

    public async Task RemoveMarker(string email)
    {
        await _repository.RemoveMarker(email);
    }

    public async Task MakeQualityControl(string email)
    {
        await _repository.MakeQualityControl(email);
    }

    public async Task RemoveQualityControl(string email)
    {
        await _repository.RemoveQualityControl(email);
    }

    public async Task MakeCandidate(string email)
    {
        await _repository.MakeCandidate(email);
    }

    public async Task RemoveCandidate(string email)
    {
        await _repository.RemoveCandidate(email);
    }

    public async Task<AuthenticationResponseDto?> Create(UserDto userDto)
    {
        if (userDto.Credentials != null)
        {
            var user = new AppUser
            {
                UserName = userDto.UserName != null ? userDto.UserName : userDto.Credentials.Email,
                Email = userDto.Credentials.Email,
                PhoneNumber = userDto.PhoneNumber,
                LockoutEnabled = false,
                EmailConfirmed = true,
            };

            var createResult = await _repository.Create(user, userDto.Credentials.Password);
            if (createResult.Succeeded)
            {
                if (
                    userDto.Credentials.IsCandidate != null
                    && userDto.Credentials.IsCandidate == true
                )
                {
                    await MakeCandidate(userDto.Credentials.Email);
                }
                else if (!string.IsNullOrWhiteSpace(userDto.Role))
                {
                    await _repository.UpdateRole(user.Email, userDto.Role);
                }

                return await BuildToken(userDto.Credentials);
            }
            else
            {
                return new AuthenticationResponseDto { Errors = createResult.Errors };
            }
        }

        return null;
    }

    public async Task<AuthenticationResponseDto?> Login(LoginDto userCredentials)
    {
        var loginResult = await _repository.Login(userCredentials.Email, userCredentials.Password);
        if (loginResult.Succeeded)
        {
            return await BuildToken(userCredentials);
        }
        return null;
    }

    public async Task<IdentityResult> Update(string email, UserDto userDto)
    {
        var user = _mapper.Map<AppUser>(userDto);
        var updateResult = await _repository.Update(email, user, userDto);
        return updateResult;
    }

    public async Task<IdentityResult> Delete(string email)
    {
        var deleteResult = await _repository.Delete(email);
        return deleteResult;
    }

    private async Task<AuthenticationResponseDto> BuildToken(LoginDto userCredentials)
    {
        var user = await _repository.GetAppUser(userCredentials.Email);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("email", userCredentials.Email),
            new Claim("userId", user.Id)
        };

        var claimsDB = await _repository.GetClaims(user);

        claims.AddRange(claimsDB);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyjwt"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddYears(1);

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new AuthenticationResponseDto()
        {
            Email = user.Email,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}
