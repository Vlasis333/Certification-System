using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using ModelLibrary.Models;
using ModelLibrary.Models.DTO.Accounts;

namespace Assignment4Final.Data.Repositories;

public interface IAccountsRepository
{
    Task<List<AppUser>> ListUsers();
    Task<AppUser> GetAppUser(string email);
    List<string> GetAllClaims();
    Task<IList<Claim>> GetClaims(AppUser user);
    Task<Dictionary<string, string>> GetUserRoles(List<AppUser> appUsers);
    string? GetUserRole(string email);
    Task<IdentityResult> UpdateRole(string userEmail, string newRole);
    Task<IdentityResult> MakeAdmin(string email);
    Task<IdentityResult> RemoveAdmin(string email);
    Task<IdentityResult> MakeMarker(string email);
    Task<IdentityResult> RemoveMarker(string email);
    Task<IdentityResult> MakeQualityControl(string email);
    Task<IdentityResult> RemoveQualityControl(string email);
    Task<IdentityResult> MakeCandidate(string email);
    Task<IdentityResult> RemoveCandidate(string email);
    Task<IdentityResult> Create(AppUser user, string password);
    Task<SignInResult> Login(string email, string password);
    Task<IdentityResult> Update(string email, AppUser user, UserDto userDto);
    Task<IdentityResult> Delete(string email);
}
