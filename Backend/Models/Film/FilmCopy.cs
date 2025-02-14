// namespace FilmStudioSFF.Models
// {
//     public class FilmCopy
//     {
//         public int FilmCopyId { get; set; }
//         public required string Title { get; set; }
//         public bool IsRented { get; set; }
//         public required Film Film { get; set; }
//         public required FilmStudio FilmStudio { get; set; }

//         // Foreign key to Film
//         public int FilmId { get; set; }

//         // Foreign key to FilmStudio
//         public int? FilmStudioId { get; set; }
//     }
// }

namespace FilmStudioSFF.Models
{
    public class FilmCopy
    {
        public int FilmCopyId { get; set; }
        public required string Title { get; set; }
        public bool IsRented { get; set; }
        public required Film Film { get; set; }
        public int FilmId { get; set; }
        public int? FilmStudioId { get; set; }
        public FilmStudio? FilmStudio { get; set; }
    }
}