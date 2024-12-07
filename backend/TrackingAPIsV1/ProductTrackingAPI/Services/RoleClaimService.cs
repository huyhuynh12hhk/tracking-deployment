using ProductTrackingAPI.Constants;
using ProductTrackingAPI.Data;
using ProductTrackingAPI.Models.Users;

namespace ProductTrackingAPI.Services
{
    public class RoleClaimService
    {
        private readonly TrackingManagementContext context;

        public RoleClaimService(TrackingManagementContext context)
        {
            this.context = context;
        }

        public async Task CreateNewClaim(string key, string value)
        {
            await context.Claims.AddAsync(new()
            {
                Key = key,
                Value = value
            });

            await SaveAllChange();
        }

        public async Task<bool> AssignRole(UserDetail user, string role, bool onSave = true)
        {
            return false;
        }


        public async Task<bool> SaveAllChange()
        {
            return (await context.SaveChangesAsync())>0;
        }

    }
}
