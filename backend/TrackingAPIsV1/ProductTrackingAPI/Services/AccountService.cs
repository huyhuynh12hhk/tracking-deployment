using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductTrackingAPI.Constants;
using ProductTrackingAPI.Data;
using ProductTrackingAPI.DTOs;
using ProductTrackingAPI.Models;
using ProductTrackingAPI.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ProductTrackingAPI.Services
{
    public class AccountService
    {
        private readonly TrackingManagementContext context;
        private readonly ILogger<AccountService> logger;
        private readonly TokenWriter tokenWriter;

        public AccountService(TrackingManagementContext context,
            TokenWriter tokenWriter, 
            ILogger<AccountService> logger)
        {
            this.context = context;
            this.tokenWriter = tokenWriter;
            this.logger = logger;
        }

        public async Task<bool> CheckUserCredentials(string username, string password)
        {

            var user = await context.UserAccounts.FirstOrDefaultAsync(u => u.Key == username);

            logger.LogInformation("Hello user, " + user?.Key ?? "null");

            if (user != null && AppPasswordHasher.VerifyPassword(password, user.Password))
            {
                logger.LogInformation("Good..");
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<List<UserAccount>> FindAccountsByEmail(string email, ProviderCatalogs? provider = null)
        {
            var user = await context.DetailUsers.FirstOrDefaultAsync(u=> u.Email == email);
            return await context.UserAccounts.
                Where(a => a.UserId == user.Id 
                && (provider==null? a.Provider.Any():a.Provider==provider.ToString()))
                .ToListAsync();
        }
        
        public async Task<UserFullInfoView?> FindUserInfo(Expression<Func<UserDetail, bool>> filter)
        {
            var user = await context.DetailUsers.FirstOrDefaultAsync(filter);
            if (user == null) { return null; }

            //var userClaims = await GetUserClaim(user);

            return new UserFullInfoView
            {
                Id = user.Id,
                Email = user.Email,
                Address = user.Address,
                FullName = user.FullName,
                Gender = user.Gender,
                Avatar = user.AvatarImage
            };
        }

        public async Task<UserMinInfoView> ProductUserToken(string email)
        {
            var totalClaims = new List<Claim>();
            var user = await context.DetailUsers.FirstOrDefaultAsync(u => u.Email == email)??new UserDetail();
            var claims = await GetUserClaim(user)??new();

            totalClaims.AddRange(claims);
            totalClaims.AddRange(new Claim[]
            {
                new(JwtRegisteredClaimNames.Name, user.FullName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(UserClaimTypes.userId.ToString(), user.Id)
            });

            return new UserMinInfoView
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Token = tokenWriter.GenerateToken(totalClaims),
                Avatar = user.AvatarImage,
                BackgroundImage = user.BackgroundImage,
            }; ;
        }

        public async Task<List<UserMinInfoView>> GetUsers(Expression<Func<UserDetail, bool>>? filter = null)
        {
            var users =  await context.DetailUsers.Where(filter==null?u=>true:filter).ToListAsync();
            return users.Select(user=>
            {
                //var account = FindAccountsByEmail(user.Email, provider: AccountProviders.None).Result.First();
                //var userClaims = GetUserClaim(user).Result;

                return new UserMinInfoView
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Token = "",
                    Avatar = user.AvatarImage,
                    BackgroundImage = user.BackgroundImage,
                };
            }).ToList();


        }



        public async Task<List<Claim>> GetUserClaim(UserDetail user)
        {
            return await context.UserClaims
                .Include(e=>e.Claim)
                .Where(c => c.UserId == user.Id)
                .Select(c=>new Claim(c.Claim.Key, c.Claim.Value))
                .ToListAsync();
        }

        public async Task<UserAccount> CreateUserAccount(string userId, string username, string password, 
            AccountTypes accountType = AccountTypes.Member 
            , ProviderCatalogs provider = ProviderCatalogs.None, bool onSave = true)
        {
            var account = new UserAccount
            {
                Key = username,
                AccountType = accountType.ToString(),
                Password = AppPasswordHasher.HashPassword(password),
                Provider = provider.ToString(),
            };

            await context.UserAccounts.AddAsync(account);

            if (onSave) await SaveAllChange();

            return account;
        }

        public async Task<bool> CreateNewUserProfile(string email, string password, string fullName, bool onSave = true)
        {
            var user = new UserDetail
            {
                Email = email,
                FullName = fullName,
            };

            await context.DetailUsers.AddAsync(user);

            await CreateUserAccount(user.Id, password, fullName, onSave:false);


            if (onSave) {
                if (await SaveAllChange()) return false;
            }

            return true;
        }

        

        public async Task<bool> SaveAllChange()
        {
            return (await context.SaveChangesAsync()) > 0;
        }
    }

}
