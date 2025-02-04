namespace FilmStudioSFF.Models
{
    public class Film
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public int AvailableCopies { get; set; }
        public List<FilmCopy> FilmCopies { get; set; }

    }
}