using AutoMapper;
using LibraryApp.Dto;
using LibraryApp.Interfaces.RepositoryInterfaces;
using LibraryApp.Interfaces.ServiceInterfaces;
using LibraryApp.Models;
using LibraryApp.Services.Exceptions;


namespace LibraryApp.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public List<AuthorDto> GetAuthors()
        {
            var authors = _mapper.Map<List<AuthorDto>>(_authorRepository.GetAuthors());

            return authors;
        }

        public AuthorDto GetAuthor(int authorId)
        {
            if (!_authorRepository.AuthorExists(authorId))
                throw new NotFoundException("Author not found");

            var author = _mapper.Map<AuthorDto>(_authorRepository.GetAuthor(authorId));

            return author;
        }

        public List<BookDto> GetBooksByAuthorId(int authorId)
        {
            var books = _mapper.Map<List<BookDto>>(_authorRepository.GetBooksByAuthor(authorId));

            return books;
        }

        public string CreateAuthor(AuthorDto authorCreate)
        {
            if (authorCreate == null)
                throw new BadRequestException();

            var author = _authorRepository.GetAuthors()
                .Where(a => a.Name.Trim().ToUpper() == authorCreate.Name.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (author != null)
                throw new UnprocessableException("Author already exists");

            var authorMap = _mapper.Map<Author>(authorCreate);

            if (!_authorRepository.CreateAuthor(authorMap))
                throw new Exception();

            return "Successfully created";
        }

        public string UpdateAuthor(int authorId, AuthorDto authorUpdate)
        {
            if (authorUpdate == null || authorId != authorUpdate.Id)
                throw new BadRequestException();

            if (!_authorRepository.AuthorExists(authorId))
                throw new NotFoundException("Author not found");

            var authorMap = _mapper.Map<Author>(authorUpdate);

            if (!_authorRepository.UpdateAuthor(authorMap))
                throw new Exception();

            return "Successfully updated";
        }

        public string DeleteAuthor(int authorId)
        {
            var author = _authorRepository.GetAuthor(authorId);

            if (author == null)
                throw new NotFoundException("Author not found");

            if (!_authorRepository.DeleteAuthor(author))
                throw new Exception();

            return "Successfully deleted";
        }
    }
}

