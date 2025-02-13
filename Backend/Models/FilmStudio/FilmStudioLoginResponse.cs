namespace FilmStudioSFF.Models
{
    public class FilmStudioLoginResponse
    {
        public int FilmStudioId { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Role { get; set; }
        public required string Email { get; set; }
        public required string City { get; set; }
        public required string Token { get; set; }
    }
}