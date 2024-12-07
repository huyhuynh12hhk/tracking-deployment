using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductTrackingAPI.Constants;
using ProductTrackingAPI.Data;
using ProductTrackingAPI.DTOs;
using ProductTrackingAPI.Models.Users;
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
        private readonly IMapper mapper;

        public AccountService(TrackingManagementContext context,
            TokenWriter tokenWriter,
            ILogger<AccountService> logger,
            IMapper mapper)
        {
            this.context = context;
            this.tokenWriter = tokenWriter;
            this.logger = logger;
            this.mapper = mapper;
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
        
        public async Task<UserFullInfoView?> FindUserInfo(Expression<Func<UserDetail, bool>> filter, string fromId = "")
        {
            var user = await context.DetailUsers.FirstOrDefaultAsync(filter);
            
            if (user == null) { return null; }


            //var userClaims = await GetUserClaim(user);
            var info = mapper.Map<UserFullInfoView>(user);

            if (!string.IsNullOrWhiteSpace(fromId))
            {
                var rela = await context.Relationships.FirstOrDefaultAsync(e => e.FromUserId == fromId && e.ToUserId == user.Id);
                info.Relationship = rela?.Type ?? "";
            }

            

            return info;
            
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

            var result = mapper.Map<UserMinInfoView>(user);
            result.Token = tokenWriter.GenerateToken(totalClaims);

            return result;
        }

        public async Task<List<UserMinInfoView>> GetUsers(Expression<Func<UserDetail, bool>>? filter = null)
        {
            var users =  await context.DetailUsers
                .OrderBy(e => e.FullName)
                .Where(filter==null?u=>true:filter).ToListAsync();
            return users.Select(user=>
            {
                //var account = FindAccountsByEmail(user.Email, provider: AccountProviders.None).Result.First();
                //var userClaims = GetUserClaim(user).Result;

                return mapper.Map<UserMinInfoView>(user);
            }).ToList();


        }



        public async Task<List<Claim>> GetUserClaim(UserDetail user)
        {
            //add role??

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
                UserId = userId
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

            await CreateUserAccount(user.Id, email, password, onSave:false);


            if (onSave) {
                return await SaveAllChange();
            }

            return true;
        }

        

        public async Task<bool> SaveAllChange()
        {
            return (await context.SaveChangesAsync()) > 0;
        }
    }

}
