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
    public class MovieController : ControllerBase
    {
        public List<Movie> database = new List<Movie>();
        public List<Statistic> MovieStatistics { get; set; }

        public MovieController()
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
        [HttpPost]
        public ActionResult Metadata(Movie movie)
        {
            database.Add(movie);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult Metadata(int id)
        {
            var movieRecords = System.IO.File.ReadLines("MovieData/metadata.csv").Skip(1);
            List<Metadatum> matchedMetadatumRecords = new List<Metadatum>();
            List<Movie> matchedMovieRecords = new List<Movie>();
            foreach (var record in movieRecords)
            {
                var splitRecord = record.Split(",");
                if (int.Parse(splitRecord[1]) == id && !String.IsNullOrWhiteSpace(splitRecord[2].ToString()) &&
                    !String.IsNullOrWhiteSpace(splitRecord[3].ToString()) &&
                    !String.IsNullOrWhiteSpace(splitRecord[4].ToString()) &&
                    !String.IsNullOrWhiteSpace(splitRecord[5].ToString()))
                {
                    foreach (Metadatum metadatum in matchedMetadatumRecords)
                    {
                        if (metadatum.Language == splitRecord[3])
                        {
                            //Only want metadata with highest id if language is the same for a movie.
                            if (metadatum.MetadatumId < int.Parse(splitRecord[0]))
                            {
                                matchedMetadatumRecords.Remove(metadatum);
                                matchedMetadatumRecords.Add(new Metadatum(int.Parse(splitRecord[0]),
                      int.Parse(splitRecord[1]),
                      splitRecord[2], splitRecord[3], splitRecord[4], int.Parse(splitRecord[5])));
                                break;

                            }
                        }
                    }

                    //Add metadata if its language not in list
                    if (matchedMetadatumRecords.Find((metadatum) => metadatum.Language == splitRecord[3]) == null)
                    {
                        matchedMetadatumRecords.Add(new Metadatum(int.Parse(splitRecord[0]),
                      int.Parse(splitRecord[1]),
                      splitRecord[2], splitRecord[3], splitRecord[4], int.Parse(splitRecord[5])));
                    }
                }
            }
            if (matchedMetadatumRecords.Count != 0)
            {
                var orderedMetadatumRecords = matchedMetadatumRecords.OrderBy(x => x.Language);

                foreach (Metadatum metadatum in orderedMetadatumRecords)
                {
                    matchedMovieRecords.Add(new Movie(metadatum.MovieId, metadatum.Title, metadatum.Language,
                   metadatum.Duration, metadatum.ReleaseYear));
                }
                return Ok(matchedMovieRecords);
            }
            return NotFound();
        }

        [HttpGet("stats")]
        public IActionResult Stats()
        {
            return Ok(MovieStatistics);
        }
    }
}

