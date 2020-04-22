using System.Collections.Generic;
using System.Linq;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories.Repositories;

namespace FilmFul_API.Services.Services
{
    public class MovieService
    {
        private readonly MovieRepository movieRepository = new MovieRepository();

        private readonly HashSet<string> validGenres = new HashSet<string>
        {
            "Action",
            "Adventure",
            "Animation",
            "Biography",
            "Comedy",
            "Crime",
            "Drama",
            "Family",
            "Fantasy",
            "Film-Noir",
            "History",
            "Horror",
            "Music",
            "Musical",
            "Mystery",
            "Romance",
            "Sci-Fi",
            "Sport",
            "Thriller",
            "War",
            "Western"
        };

        private const int badRequest = 400;

        // User input sanitization - This function capitalizes genre strings from the query parameters
        // such that they will match they way they are capitalized in the database.
        private string correctGenreCaps(string genre)
        {
            if (genre.Contains('-'))
            {
                List<string> strSep = genre.Trim()
                                           .ToLower()
                                           .Split('-')
                                           .ToList()
                                           .Aggregate
                                           (
                                                new List<string>(), (lis, s) =>
                                                {
                                                    s = s.First().ToString().ToUpper() + s.Substring(1);
                                                    lis.Add(s);
                                                    return lis;
                                                }
                                           );
                
                genre = string.Join('-', strSep);
            }
            else
            {
                genre = genre.Trim().ToLower();
                genre = genre.First().ToString().ToUpper() + genre.Substring(1);
            }

            return genre;
        }

        private bool genresOkay(ref List<string> genres)
        {
            // Genre query parameter not provided - That is okay.
            if (genres == null) { return true; }

            // If some string in genres is not a key in the validGenres HashSet, we have a bad request.
            foreach (string genre in genres)
            {
                if (!validGenres.Contains(correctGenreCaps(genre))) { return false; }
            }
            return true;
        }

        public (IEnumerable<MovieDto>, int) GetAllMovies(int pageSize, int pageIndex, bool poster, List<string> genres)
        {
            if (!genresOkay(ref genres)) { return (null, badRequest); }
            return movieRepository.GetAllMovies(pageSize, pageIndex, poster, genres);
        }

        public MovieDto GetMovieById(int id)
        {
            return movieRepository.GetMovieById(id);
        }

        public IEnumerable<ActorDto> GetMovieActorsByMovieId(int id)
        {
            return movieRepository.GetMovieActorsByMovieId(id);
        }

        public IEnumerable<DirectorDto> GetMovieDirectorsByMovieId(int id)
        {
            return movieRepository.GetMovieDirectorsByMovieId(id);
        }
    }
}