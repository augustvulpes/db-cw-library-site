using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Models;

namespace LibraryApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>();
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();
            CreateMap<Collection, CollectionDto>();
            CreateMap<CollectionDto, Collection>();
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<News, NewsDto>();
            CreateMap<NewsDto, News>();
        }
    }
}
