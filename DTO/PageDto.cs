namespace ToDoApi.DTO
{
    public class PageDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
    }
}
