using AutoMapper;
using SL_Api_Ecommerce.Models;
using SL_Api_Ecommerce.Models.Dtos;

namespace SL_Api_Ecommerce.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
        }
    }
}