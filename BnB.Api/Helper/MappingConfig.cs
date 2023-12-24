using AutoMapper;
using BnB.Api.Dto;
using BnB.Api.Dto.Brand;
using BnB.Api.Dto.Category;
using BnB.Api.Dto.Order;
using BnB.Api.Dto.OrderItem;
using BnB.Api.Dto.Product;
using BnB.Api.Dto.Review;
using BnB.Api.Models;

namespace BnB.Api.Helper;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<User, UserDto>().ForMember(dest => dest.Phone, u => u.MapFrom(src => src.PhoneNumber));
            config.CreateMap<UserDto, User>().ForMember(dest => dest.PhoneNumber, u => u.MapFrom(src => src.Phone));
            config.CreateMap<UpdateUserDto, User>().ForMember(dest => dest.PhoneNumber, u => u.MapFrom(src => src.Phone));
            config.CreateMap<Brand, BrandDto>().ReverseMap();
            config.CreateMap<Category, CategoryDto>().ReverseMap();
            config.CreateMap<Order, OrderDto>().ReverseMap();
            config.CreateMap<CreateOrderDto, Order>().ReverseMap();
            config.CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            config.CreateMap<OrderProduct, OrderItem>().ReverseMap();
            config.CreateMap<DetailOrderItem, OrderItemDto>().ReverseMap();
            config.CreateMap<Product, ProductDto>().ForMember(dest => dest.Images, u => u.MapFrom(src => new List<string>()));
            config.CreateMap<ProductDto, Product>();
            config.CreateMap<Product, CreateProductDto>().ReverseMap();
            config.CreateMap<Review, ReviewDto>().ReverseMap();
            config.CreateMap<DetailReview, ReviewDto>().ReverseMap();
            config.CreateMap<CreateReviewDto, Review>();
        });
        return mappingConfig;
    }
}