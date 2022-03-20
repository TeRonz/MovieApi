using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
namespace MovieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        public List<Statistic> MovieStatistics { get; set; }

        public MoviesController()
        {
            var movieStats = System.IO.File.ReadLines("MovieData/stats.csv").Skip(1);
            List<Statistic> movieStatistics = new List<Statistic>();
            for (var i = 3; i < 8; i++)
            {
                var countedViews = countViews(movieStats, i);
                var views = countedViews[0];
                var averageWatchDurationS = Math.Ceiling(double.Parse((countedViews[1] / countedViews[0]).ToString()));
                var movieRecords = System.IO.File.ReadLines("MovieData/metadata.csv").Skip(1);
                foreach (var record in movieRecords)
                {
                    var splitRecord = record.Split(",");
                    if (int.Parse(splitRecord[1]) == i)
                    {
                        movieStatistics.Add(new Statistic(int.Parse(splitRecord[1]), splitRecord[2], int.Parse(splitRecord[5]),
                            views, int.Parse(averageWatchDurationS.ToString())));
                        break;
                    }
                }

            }
            var orderedMovieStatistics = movieStatistics.OrderBy(x => x.Views).ThenBy(x => x.ReleaseYear).ToList<Statistic>();
            MovieStatistics = orderedMovieStatistics;
        }

        public List<int> countViews(IEnumerable<String> movieStats, int movieId)
        {
            int views = 0;
            int totalViewingTime = 0;
            foreach (var view in movieStats)
            {
                var viewSplit = view.Split(",");
                if (int.Parse(viewSplit[0]) == movieId)
                {
                    views++;
                    totalViewingTime += int.Parse(Math.Ceiling(double.Parse(viewSplit[1])/1000).ToString());
                }
            }
            var results = new List<int>();
            results.Add(views);
            results.Add(totalViewingTime);
            return results;
        }
       
        [HttpGet("stats")]
        public IActionResult Stats()
        {
            return Ok(MovieStatistics);
        }
    }
}

