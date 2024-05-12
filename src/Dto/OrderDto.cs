namespace LibraryApp.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public string State { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
