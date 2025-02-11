using System.Collections.Generic;

namespace FilmStudioSFF.Models
{
    public class FilmCopy
    {
        public int FilmCopyId { get; set; }  
        public required string Title { get; set; }
        public bool IsAvailable { get; set; }
    }
}