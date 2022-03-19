using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi
{
    public class Metadatum
    {
        public int MetadatumId { get; set; }
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public int ReleaseYear { get; set; }

        public Metadatum(int metadatumId, int movieId, string title, string language, string duration, int releaseYear)
        {
            MetadatumId = metadatumId;
            MovieId = movieId;
            Title = title;
            Language = language;
            Duration = duration;
            ReleaseYear = releaseYear;
        }
    }
}
