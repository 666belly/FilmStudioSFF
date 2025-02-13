namespace FilmStudioSFF.Models
{
    public class FilmStudioDTO
    {
        public int FilmStudioId { get; set; }
        public required string Username { get; set; }
        public required string Name { get; set; }
        public required string City { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public List<FilmCopyDTO> RentedFilms { get; set; } = new List<FilmCopyDTO>();
    }

    public class FilmCopyDTO
    {
        public int FilmCopyId { get; set; }
        public required string Title { get; set; }
        public bool IsRented { get; set; }
    }
}