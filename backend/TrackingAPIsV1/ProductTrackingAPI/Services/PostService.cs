using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductTrackingAPI.Constants;
using ProductTrackingAPI.Data;
using ProductTrackingAPI.DTOs;
using ProductTrackingAPI.Models.Social;
using System.Linq.Expressions;

namespace PostTrackingAPI.Services
{
    public class PostService
    {
        private readonly TrackingManagementContext context;
        private readonly IMapper mapper;

        public PostService(TrackingManagementContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<List<PostView>> GetPosts(string userId, Expression<Func<Post, bool>>? filter = null)
        {
            var user = await context.DetailUsers.FirstOrDefaultAsync(e=>e.Id == userId);

            List<PostView> results = new();

            if (user == null)
            {
                return results;
            }

            var userRelas = context.Relationships
                .Where(e => e.FromUserId == user.Id)
                .AsEnumerable()
                .Select(e => e.ToUserId)
                .ToList();


            var posts = context.Posts
                .Include(e=>e.Author)
                .OrderByDescending(e => e.CreatedDate)
                .Where(e=>userRelas.Contains(e.AuthorId))
                .Where(filter == null ? u => true : filter)
                .ToList()
                .Select(e =>
                {
                    var p = mapper.Map<PostView>(e);

                    return p;
                });

            results.AddRange(posts);

            return results;

        }

        public async Task<Post?> GetPost(Expression<Func<Post, bool>> filter)
        {
            var post = await context.Posts
                .Include(e => e.Reactions)
                .FirstOrDefaultAsync(filter);


            return post;
        }


        public async Task<Post> AddPost(Post post, bool onSave = true)
        {
            await context.Posts.AddAsync(post);


            if (onSave)
            {
                await SaveAllChange();
            }

            return post;
        }


        public async Task<Post?> EditPost(string id, Post content, bool onSave = true)
        {
            var post = await GetPost(e=>e.Id == id);
            if (post != null)
            {
                return null;
            }
            post.AttachmentPaths = content.AttachmentPaths;
            post.Content = content.Content;
            post.LastModified = DateTime.Now;


            context.Posts.Update(post);


            if (onSave)
            {
                await SaveAllChange();
            }

            return post;
        }

        public async Task<Post?> ReactionPost(string id, string userId, ReactionTypes type = ReactionTypes.like, bool onSave = true)
        {
            var post = await GetPost(e => e.Id == id);
            if (post != null)
            {
                return null;
            }

            await context.Reactions.AddAsync(
                new()
                {
                    PostId = id,
                    FromUserId = userId,
                    Type = type.ToString(),
                }
            );

            if (onSave)
            {
                await SaveAllChange();
            }

            return post;
        }





        public async Task<bool> SaveAllChange()
        {
            return (await context.SaveChangesAsync()) > 0;
        }
    }
}
