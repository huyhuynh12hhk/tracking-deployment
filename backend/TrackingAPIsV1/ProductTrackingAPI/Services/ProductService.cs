using AutoMapper;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using PostTrackingAPI.Services;
using ProductTrackingAPI.Data;
using ProductTrackingAPI.DTOs;
using ProductTrackingAPI.Models.Products;
using ProductTrackingAPI.Models.Social;
using ProductTrackingAPI.Models.Users;
using System.Linq;
using System.Linq.Expressions;

namespace ProductTrackingAPI.Services
{
    public class ProductService
    {
        private readonly TrackingManagementContext context;
        private readonly IMapper mapper;
        private PostService postService;

        public ProductService(TrackingManagementContext context, IMapper mapper, PostService postService)
        {
            this.context = context;
            this.mapper = mapper;
            this.postService = postService;
        }

        public async Task<IEnumerable<ProductView>> GetProducts(Expression<Func<ProductDetail, bool>>? filter = null)
        {
            var products = await context.Products
                .Include(x => x.Supplier)
                .OrderBy(e=>e.Name)
                .Where(filter == null ? u => true : filter)
                .ToListAsync();


            return products.Select(e=> {
                var product = mapper.Map<ProductView>(e);
                product.AvatarImage = e.GalleryPaths.Any() ? e.GalleryPaths[0] : "";
                
                

                return product;
            });

        }

        public async Task<ProductDetailView?> GetProduct(Expression<Func<ProductDetail, bool>> filter)
        {
            var product = await context.Products
                //.Include(e=>e.Products)
                .Include(e=>e.Supplier)
                .Include(e=>e.Origins)
                .ThenInclude(e=>e.FromProduct)
                .ThenInclude(e=>e.Supplier)
                .OrderBy(e=>e.Name)
                .FirstOrDefaultAsync(filter);

            if(product == null)
                return null;

            var rs = mapper.Map<ProductDetailView>(product);



            //extract origins and produtions here
            rs.OriginProducts = product.Origins.Select(e=>mapper.Map<ProductView>(e.FromProduct)).ToList();


            return rs;
        }


        public async Task<ProductDetail> AddProduct(AddProductModel product, bool onSave = true, bool isPublish = true)
        {
            var newProduct = mapper.Map<ProductDetail>(product);
            
            await context.Products.AddAsync(newProduct);

            foreach (var item in product.OriginKeys)
            {
                await context.ProductOrigins.AddAsync(
                    new()
                    {
                        FromProductId = item.Key,
                        ToProductId = newProduct.Id,
                        Note = item.Value
                    }
                );
            }


            //Make a Post
            if (isPublish)
            {
                var user = await context.DetailUsers.FirstAsync(e=>e.Id==newProduct.SupplierId);
                var productPost = new Post
                {
                    AuthorId = user.Id,
                    Content = $"{user.FullName} has add a product {newProduct.Name}.",
                    AttachmentPaths = newProduct.GalleryPaths

                };


                await postService.AddPost(
                    productPost
                );
            }

            if (onSave)
            {
                await SaveAllChange();
            }

            return newProduct;
        }

        public async Task<bool> UpdateProductOrigins(string id, IDictionary<string, string> origins, bool onSave = true)
        {
            var existProduct = await context.Products.FirstOrDefaultAsync(e=>e.Id == id);
            if (existProduct == null)
            {
                //throw new Exception();
                return false;
            }

            var oldOrigin = await context.ProductOrigins.Where(e => e.ToProductId == id).ToListAsync();

            context.ProductOrigins.RemoveRange(oldOrigin);

            foreach (var item in origins)
            {
                //if (!oldOrigin.Any(e => e.FromProductId == item.Key))
                //{
                    await context.ProductOrigins.AddAsync(
                        new()
                        {
                            FromProductId = existProduct.Id,
                            ToProductId = item.Key,
                            Note = item.Value
                        }
                    );
                //}
            }

            if (onSave)
            {
                await SaveAllChange();
            }

            return true;
        }

        public async Task<ProductDetail?> UpdateProductInfo(string id, EditProductInfoModel product, bool onSave = true)
        {
            var existProduct = await context.Products.FirstOrDefaultAsync(e => e.Id == id);
            if (existProduct == null)
            {
                //throw new Exception();
                return null;
            }
            
            mapper.Map(product, existProduct );

            context.Products.Update(existProduct);

            if (onSave)
            {
                await SaveAllChange();
            }

            return existProduct;
        }

        public async Task<bool> DeleteProduct(string id, bool onSave = true)
        {
            var existProduct = await context.Products.FirstOrDefaultAsync(e => e.Id == id);
            if (existProduct == null)
            {
                //throw new Exception();
                return false;
            }

            context.Products.Remove(existProduct);

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
