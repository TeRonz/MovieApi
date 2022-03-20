using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi
{
    public class Statistic
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }

        public int Views { get; set; }

        public int AverageWatchDurationS { get; set; }

        public Statistic(int movieId, string title, int releaseYear, int views, int averageWatchDurationS)

        {
            MovieId = movieId;
            Title = title;
            ReleaseYear = releaseYear;
            Views = views;
            AverageWatchDurationS = averageWatchDurationS;
        }
    }
}
