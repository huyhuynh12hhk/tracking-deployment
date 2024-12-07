using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductTrackingAPI.Constants;
using ProductTrackingAPI.Data;
using ProductTrackingAPI.DTOs;

namespace ProductTrackingAPI.Services
{
    public class SocialService
    {
        private readonly TrackingManagementContext context;
        private readonly IMapper mapper;

        public SocialService(TrackingManagementContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public async Task<List<UserRelationshipView>> GetFollowingUsers(string ownerId)
        {
            var list = context.Relationships
                .Include(e => e.ToUser)
                .Include(e => e.FromUser)
                .Where(e => e.FromUserId == ownerId)
                .OrderBy(e => e.ToUser.FullName)
                .Select(e => new UserRelationshipView
                    {
                        User = mapper.Map<UserMinInfoView>(e.ToUser),
                        Type = e.Type
                    }
                ).ToList() ;


            return list;
        }

        public async Task<bool> FollowUser(string fromId, string toId, bool onSave = true)
        {
            await context.Relationships.AddAsync(
                new()
                {
                    FromUserId = fromId,
                    ToUserId = toId,
                    
                }    
            );

            if (onSave)
            {
                await SaveAllChange();
            }

            return true;
        }

        public async Task<bool> EditFollowingType(string fromId, string toId, string types, bool onSave = true)
        {
            var rela = await context.Relationships.FirstOrDefaultAsync(e => e.ToUserId == toId && e.FromUserId == fromId);

            if(rela == null) { 
                return false;
            }

            rela.Type = types;
            rela.LastModified = DateTime.Now;

            context.Relationships.Update(rela);

            if (onSave)
            {
                await SaveAllChange();

            }

            return true;
        }

        public async Task<bool> SaveAllChange()
        {
            return (await context.SaveChangesAsync()) > 0;
        }
    }
}
