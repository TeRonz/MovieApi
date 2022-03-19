using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
namespace MovieApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        public List<Movie> database;

        [HttpPost]
        public void Metadata(Movie movie)
        {
            database.Add(movie);
            return;
        }

        [HttpGet]
        [Route("Metadata/{id}")]
        public List<Movie> Metadata(int id)
        {
            var movieRecords = System.IO.File.ReadLines("metadata.csv").Skip(1);
            List<Metadatum> matchedMetadatumRecords = new List<Metadatum>();
            List<Movie> matchedMovieRecords = new List<Movie>();
            foreach (var record in movieRecords)
            {
                var splitRecord = record.Split(",");
                if (int.Parse(splitRecord[1]) == id && String.IsNullOrWhiteSpace(splitRecord[2].ToString()) &&
                    String.IsNullOrWhiteSpace(splitRecord[3].ToString()) &&
                    String.IsNullOrWhiteSpace(splitRecord[4].ToString()) &&
                    String.IsNullOrWhiteSpace(splitRecord[5].ToString()))
                {
                    foreach(Metadatum metadatum in matchedMetadatumRecords)
                    {
                        if(metadatum.Language == splitRecord[3])
                        {
                            if(metadatum.MetadatumId < int.Parse(splitRecord[0]))
                            {
                                matchedMetadatumRecords.Remove(metadatum);
                                matchedMetadatumRecords.Add(new Metadatum(int.Parse(splitRecord[0]),
                      int.Parse(splitRecord[1]),
                      splitRecord[2], splitRecord[3], splitRecord[4], int.Parse(splitRecord[5])));

                            }
                        }
                    }

                  

                    matchedMovieRecords.Add(new Movie(int.Parse(splitRecord[1]), splitRecord[2], splitRecord[3],
                        splitRecord[4], int.Parse(splitRecord[5])));
                }
            }
            foreach(Movie movie in matchedMovieRecords)
            {

            }
        }

    }
}
