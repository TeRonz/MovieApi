using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public int ReleaseYear { get; set; }

        public Movie(int movieId, string title, string language, string duration, int releaseYear)
        {
            MovieId = movieId;
            Title = title;
            Language = language;
            Duration = duration;
            ReleaseYear = releaseYear;
        }
    }
}
