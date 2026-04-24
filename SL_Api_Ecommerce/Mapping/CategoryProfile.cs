using SL_Api_Ecommerce.Models;
using SL_Api_Ecommerce.Models.Dtos;
using AutoMapper;

namespace SL_Api_Ecommerce.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile() 
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
        }
    }
}