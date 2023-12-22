using Microsoft.EntityFrameworkCore;
using WebAppForBD.Data;
using WebAppForBD.Pages;

namespace WebAppForBD.Services
{
    public interface IMovieService
    {
        Movie GetMovieByTitle(string title);       
    }

    public class MovieService
    {
        public ApplicationContext db;

        public MovieService(ApplicationContext db)
        {
            this.db = db;
        }

        public Movie GetMoviesByTitle(string tittle)
        {
            var movies = db.Movies.FirstOrDefault(m => m.tittle == tittle);
            //Console.WriteLine(tittle);
            return movies;
        }
    
        
        public IEnumerable<Human> GetActors(Movie movie)
        {
            var actorNames = db.Movies
                                .Include(t => t.actorsSetConnection)
                                .Where(mov => mov.tittle == movie.tittle)
                                .AsParallel()
                                .SelectMany(mov => mov.actorsSetConnection);
            return actorNames;
        }


        public IEnumerable<Tag> GetTags(Movie movie)
        {
            var tagTittles = db.Movies
                            .Include(t => t.tagssSetConnection)
                            .Where(mov => mov.tittle == movie.tittle)
                            .AsParallel()
                            .SelectMany(mov => mov.tagssSetConnection)
                            .ToList();
            return tagTittles;
        }

        public Dictionary<string, double> GetSimilaryMovies(Movie currentMovie, IEnumerable<Human> actorMovies, IEnumerable<Tag> tagMovies)
        {
            //идея для сравнения фильмов: давайте если у фильмов одинаковые режиссеры то добавляем к рейтингу схожести 0,05
            //если одинаковый актер то +0,025, если одинаковый тег, то +0,01, затем в конце выбрать min{0.5, сумма добавок}
            Dictionary<string, double> rankedSimilaryMovie = new Dictionary<string, double>();
            var newActorsMovies = db.Humans.Include(b => b.currentMoviesConnection).Where(hum => actorMovies.Contains(hum));

            foreach (var actor in newActorsMovies)
            {
                //получаем список фильмов данного актера (который принял участие в этом фильме
                currentMovie.potentialSimilaryMovie.UnionWith(actor.currentMoviesConnection.Select(t => t.tittle));
            };
            var newTagsMovies = db.Tags.Include(b => b.currentMoviesConnection).Where(hum => tagMovies.Contains(hum));

            foreach (var tag in tagMovies)
            {
                //получаем список фильмов данного тега 
                currentMovie.potentialSimilaryMovie.UnionWith(tag.currentMoviesConnection.Select(m => m.tittle));
            };
            currentMovie.potentialSimilaryMovie.Remove(currentMovie.tittle);


            var newMoviesActors = db.Movies.Include(b => b.actorsSetConnection).Where(hum => currentMovie.potentialSimilaryMovie.Contains(hum.tittle));
            var newMoviesTags = db.Movies.Include(b => b.tagssSetConnection).Where(hum => currentMovie.potentialSimilaryMovie.Contains(hum.tittle));
            newMoviesTags.Union(newMoviesActors);
            foreach (var simMovie in currentMovie.potentialSimilaryMovie)
            {
                double rank = 0;
                var compareMovie1 = newMoviesTags.FirstOrDefault(m => m.tittle == simMovie);
                HashSet<string> commonActorMovie = new HashSet<string>();
                HashSet<string> commonTagMovie = new HashSet<string>();
                foreach (var ttt in compareMovie1.actorsSetConnection)
                {
                    commonActorMovie.Add(ttt.name);
                }
                foreach (var ttt in compareMovie1.tagssSetConnection)
                {
                    commonTagMovie.Add(ttt.tittle);
                }

                commonActorMovie.Intersect(currentMovie.actorsSet);
                commonTagMovie.Intersect(currentMovie.tagSet);

                var t = commonActorMovie.Count;
                var tt = commonTagMovie.Count;

                if ((compareMovie1.director == currentMovie.director))
                {
                    rank = 0.05;
                }
                rank = Math.Min(rank + t * 0.01 + tt * 0.001, 0.5);
                if (compareMovie1.movieRating != null)
                {
                    rankedSimilaryMovie.Add(simMovie, rank + 0.05 * Convert.ToDouble(compareMovie1.movieRating.Replace('.', ',')));
                }
                else
                {
                    rankedSimilaryMovie.Add(simMovie, rank);

                }
            };
            var sortedDictMovies = from mov in rankedSimilaryMovie orderby mov.Value descending select mov;
            return sortedDictMovies.Take(10).ToDictionary();
        }
    
    
        public IEnumerable<Movie> GetMoviesForActor(string humanName)
        {
            var moviesTitles = from human in db.Humans
                               where human.name == humanName
                               from movie in human.currentMoviesConnection
                               select movie;
            return moviesTitles;
        }

        public IEnumerable<Movie> GetMoviesForTag(string tagName)
        {
            var moviesTitlesTags = from tag in db.Tags
                                   where tag.tittle == tagName
                                   from movie in tag.currentMoviesConnection
                                   select movie;
            return moviesTitlesTags;
        }

    }
}
