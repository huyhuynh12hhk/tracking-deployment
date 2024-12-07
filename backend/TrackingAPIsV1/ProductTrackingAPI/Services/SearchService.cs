using Microsoft.EntityFrameworkCore;
using ProductTrackingAPI.Constants;
using ProductTrackingAPI.Data;
using ProductTrackingAPI.DTOs;

namespace ProductTrackingAPI.Services
{
    public class SearchService
    {
        private readonly TrackingManagementContext context;

        public SearchService(TrackingManagementContext context)
        {
            this.context = context;
        }

        public async Task<List<SearchResult>> SearchAll(string keyword)
        {
            List<SearchResult> results = new();

            
            ///*
            var products = context.Products
                .Include(e => e.Supplier)
                .Where(e =>
                    e.Name.ToLower().Contains(keyword.ToLower())
                    //|| e.Supplier.FullName.ToLower().Contains(keyword.ToLower())
                )
                .Select(e=>new SearchResult
                {
                    Key = e.Id,
                    Label = e.Name,
                    Type = SearchResultTypes.product.ToString(),
                    ImagePath = e.GalleryPaths.Any() ? e.GalleryPaths[0]:""
                });

            results.AddRange(products);

            var users = context.DetailUsers
                .Where(e =>
                    e.FullName.ToLower().Contains(keyword.ToLower())
                    || e.Email.ToLower().Contains(keyword.ToLower())
                )
                .Select(e=>new SearchResult
                {
                    Key = e.Id,
                    Label = e.FullName,
                    Type = SearchResultTypes.user.ToString(),
                    ImagePath = e.AvatarImage
                });

            results.AddRange(users);
            //*/

            return results;
        }
    }
}
