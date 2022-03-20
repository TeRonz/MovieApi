using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MetadataController : Controller
    {
        public List<Movie> database = new List<Movie>();

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
                    !String.IsNullOrWhiteSpace(splitRecord[5].ToString()) && splitRecord.Length == 6)
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
    }
}
