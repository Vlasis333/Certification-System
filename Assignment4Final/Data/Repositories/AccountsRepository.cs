using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModelLibrary.Models;
using ModelLibrary.Models.DTO.Accounts;

namespace Assignment4Final.Data.Repositories;

public class AccountsRepository : IAccountsRepository
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public AccountsRepository(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ApplicationDbContext context
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<List<AppUser>> ListUsers()
    {
        var queryable = _context.Users.AsQueryable();
        return await queryable.OrderBy(x => x.Email).ToListAsync();
    }

    public async Task<AppUser> GetAppUser(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public List<string> GetAllClaims()
    {
        return _context.UserClaims.ToList().Select(c => c.ClaimValue).Distinct().ToList();
    }

    public async Task<IList<Claim>> GetClaims(AppUser user)
    {
        return await _userManager.GetClaimsAsync(user);
    }

    public async Task<Dictionary<string, string>> GetUserRoles(List<AppUser> appUsers)
    {
        var userRoles = new Dictionary<string, string>();
        var userEmails = appUsers.Select(u => u.Email).ToList();

        foreach (var user in appUsers)
        {
            var claims = await GetClaims(user);
            var role = claims?.FirstOrDefault(c => c.Type == "role");
            if (role != null)
            {
                userRoles[user.Email] = role.Value;
            }
        }

        return userRoles;
    }

    public string? GetUserRole(string email)
    {
        return GetClaims(GetAppUser(email).Result).Result
            ?.FirstOrDefault(c => c.Type == "role")
            ?.Value;
    }

    public async Task<IdentityResult> MakeAdmin(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return await _userManager.AddClaimAsync(user, new Claim("role", "admin"));
    }

    public async Task<IdentityResult> RemoveAdmin(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return await _userManager.RemoveClaimAsync(user, new Claim("role", "admin"));
    }

    public async Task<IdentityResult> MakeMarker(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return await _userManager.AddClaimAsync(user, new Claim("role", "marker"));
    }

    public async Task<IdentityResult> RemoveMarker(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return await _userManager.RemoveClaimAsync(user, new Claim("role", "marker"));
    }

    public async Task<IdentityResult> MakeQualityControl(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return await _userManager.AddClaimAsync(user, new Claim("role", "qualitycontrol"));
    }

    public async Task<IdentityResult> RemoveQualityControl(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return await _userManager.RemoveClaimAsync(user, new Claim("role", "qualitycontrol"));
    }

    public async Task<IdentityResult> MakeCandidate(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return await _userManager.AddClaimAsync(user, new Claim("role", "candidate"));
    }

    public async Task<IdentityResult> RemoveCandidate(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return await _userManager.RemoveClaimAsync(user, new Claim("role", "candidate"));
    }

    public async Task<IdentityResult> UpdateRole(string userEmail, string newRole)
    {
        string? oldRole = GetUserRole(userEmail);

        var identityResult = new IdentityResult();

        if (!string.IsNullOrWhiteSpace(oldRole))
        {
            switch (oldRole)
            {
                case "admin":
                    identityResult = await RemoveAdmin(userEmail);
                    break;
                case "marker":
                    identityResult = await RemoveMarker(userEmail);
                    break;
                case "qualitycontrol":
                    identityResult = await RemoveQualityControl(userEmail);
                    break;
                case "candidate":
                    identityResult = await RemoveCandidate(userEmail);
                    break;
                default:
                    return IdentityResult.Failed(
                        new IdentityError()
                        {
                            Code = "Role",
                            Description =
                                $"Failed to remove {oldRole} to user with email {userEmail}"
                        }
                    );
            }
        }

        if (!string.IsNullOrWhiteSpace(newRole))
        {
            switch (newRole)
            {
                case "admin":
                    return await MakeAdmin(userEmail);
                case "marker":
                    return await MakeMarker(userEmail);
                case "qualitycontrol":
                    return await MakeQualityControl(userEmail);
                case "candidate":
                    return await MakeCandidate(userEmail);
                default:
                    return IdentityResult.Failed(
                        new IdentityError()
                        {
                            Code = "Role",
                            Description = $"Failed to add {newRole} to user with email {userEmail}"
                        }
                    );
            }
        }

        return identityResult;
    }

    public async Task<IdentityResult> Create(AppUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<SignInResult> Login(string email, string password)
    {
        var user = await GetAppUser(email);

        if (user == null)
        {
            return SignInResult.Failed;
        }

        return await _signInManager.PasswordSignInAsync(
            user,
            password,
            isPersistent: false,
            lockoutOnFailure: false
        );
    }

    public async Task<IdentityResult> Update(string email, AppUser user, UserDto userDto)
    {
        var dbUser = await GetAppUser(email);

        var identityErrors = new List<IdentityError>();

        if (dbUser != null)
        {
            var usernameResult = await _userManager.SetUserNameAsync(dbUser, user.UserName);
            if (!usernameResult.Succeeded)
            {
                identityErrors.AddRange(usernameResult.Errors);
                return IdentityResult.Failed(identityErrors.ToArray());
            }

            var emailResult = await _userManager.SetEmailAsync(dbUser, user.Email);
            if (!emailResult.Succeeded)
            {
                identityErrors.AddRange(emailResult.Errors);
                return IdentityResult.Failed(identityErrors.ToArray());
            }
            else
            {
                dbUser.EmailConfirmed = true;
            }

            var phoneNumberResult = await _userManager.SetPhoneNumberAsync(
                dbUser,
                user.PhoneNumber
            );
            if (!phoneNumberResult.Succeeded)
            {
                identityErrors.AddRange(phoneNumberResult.Errors);
                return IdentityResult.Failed(identityErrors.ToArray());
            }

            if (userDto.Role != null)
            {
                var updateRoleResult = await UpdateRole(dbUser.Email, userDto.Role);
                if (!updateRoleResult.Succeeded)
                {
                    identityErrors.AddRange(updateRoleResult.Errors);
                    return IdentityResult.Failed(identityErrors.ToArray());
                }
            }

            if (userDto.Credentials != null && !string.IsNullOrEmpty(userDto.Credentials.Password))
            {
                // NOTE:(akotro): Reset password without oldPassword
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(dbUser);
                var passwordResult = await _userManager.ResetPasswordAsync(
                    dbUser,
                    resetToken,
                    userDto.Credentials.Password
                );

                if (!passwordResult.Succeeded)
                {
                    identityErrors.AddRange(passwordResult.Errors);
                    return IdentityResult.Failed(identityErrors.ToArray());
                }
                else
                {
                    dbUser.LockoutEnabled = false;
                }
            }

            return await _userManager.UpdateAsync(dbUser);
        }

        return IdentityResult.Failed(
            new IdentityError
            {
                Code = "User",
                Description = $"Could not find User with email {email}"
            }
        );
    }

    public async Task<IdentityResult> Delete(string email)
    {
        var user = await _context.Users
            .Include(u => u.Candidate)
            .Include(u => u.Marker)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return IdentityResult.Failed(
                new IdentityError
                {
                    Code = "User",
                    Description = $"Could not find User with email {email}"
                }
            );
        }

        if (user.Candidate != null)
        {
            var candidate = await _context.Candidates
                .AsSplitQuery()
                .Include(a => a.Gender)
                .Include(a => a.Language)
                .Include(a => a.PhotoIdType)
                .Include(a => a.Address)
                .ThenInclude(a => a.Country)
                .Include(a => a.CandidateExams)
                .FirstOrDefaultAsync(c => c.AppUserId == user.Candidate.AppUserId);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
            }
        }

        if (user.Marker != null)
        {
            var marker = await _context.Markers
                .Include(m => m.CandidateExams)
                .FirstOrDefaultAsync(q => q.AppUserId == user.Marker.AppUserId);

            if (marker != null)
            {
                // NOTE:(akotro) Check if marker has assigned CandidateExams that have not been moderated
                if (marker.CandidateExams?.Any(ce => ce.IsModerated != true) == true)
                {
                    return IdentityResult.Failed(
                        new IdentityError
                        {
                            Code = "Marker",
                            Description =
                                $"Marker with email {email} has assigned candidate exams that have not been moderated."
                        }
                    );
                }

                _context.Markers.Remove(marker);
                await _context.SaveChangesAsync();
            }
        }

        return await _userManager.DeleteAsync(user);
    }
}
