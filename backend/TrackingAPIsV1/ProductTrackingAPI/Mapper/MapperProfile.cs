using AutoMapper;
using ProductTrackingAPI.DTOs;
using ProductTrackingAPI.Models.Products;
using ProductTrackingAPI.Models.Social;
using ProductTrackingAPI.Models.Users;

namespace ProductTrackingAPI.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile() { 
            CreateMap<UserDetail, UserMinInfoView>().ReverseMap();
            CreateMap<UserDetail, UserFullInfoView>().ReverseMap();
            CreateMap<UserDetail, SupplierModel>().ReverseMap();


            CreateMap<EditProductInfoModel, ProductDetail>().ReverseMap();
            CreateMap<AddProductModel, ProductDetail>().ReverseMap();
            CreateMap<ProductView, ProductDetail>().ReverseMap();
            CreateMap<ProductDetailView, ProductDetail>().ReverseMap();


            CreateMap<AddPostModel, Post>().ReverseMap();
            CreateMap<PostView, Post>().ReverseMap();
        }
    }
}
