namespace LibraryApp.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
