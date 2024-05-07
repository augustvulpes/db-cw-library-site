using LibraryApp.Dto;

namespace LibraryApp.Interfaces.ServiceInterfaces
{
    public interface IAuthorService
    {
        public List<AuthorDto> GetAuthors();
        public AuthorDto GetAuthor(int authorId);
        public List<BookDto> GetBooksByAuthorId(int authorId);
        public string CreateAuthor(AuthorDto authorCreate);
        public string UpdateAuthor(int authorId, AuthorDto authorUpdate);
        public string DeleteAuthor(int authorId);
    }
}
