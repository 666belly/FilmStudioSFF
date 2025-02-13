namespace FilmStudioSFF.Models
{
    public class CreateFilm : ICreateFilm
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int AvailableCopies { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }
    }
}