using System;
using System.Collections.Generic;
using System.Linq;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Repositories.Extensions;

namespace FilmFul_API.Repositories.Repositories
{
    public class MovieRepository
    {
        private readonly FilmFulDbContext filmFulDbContext = new FilmFulDbContext();

        public (IEnumerable<MovieDto>, int) GetAllMovies(int pageSize, int pageIndex, bool poster, List<string> genres)
        {
            var moviesAndGenres = 
            (
                from movie in filmFulDbContext.Movie
                    join genre in filmFulDbContext.Genre on movie.Id equals genre.MovieId
                    select new { movie, genre, movieCount = filmFulDbContext.Movie.Count() }
            ).ToList();

            int rangeOkay = Utilities.checkRange(pageSize, pageIndex, moviesAndGenres.First().movieCount);
            if(rangeOkay != Utilities.ok) { return (null, rangeOkay); }

            var moviesWithGenres = moviesAndGenres
                                   .Select(m => m.movie)
                                   .Distinct()
                                   .Skip(pageIndex * pageSize)
                                   .Take(pageSize);

            // At this point the selected range (page) of films has been gathered. 
            // If genres contains list of valid genres, the selected range is filtered for movies
            // which are of the genres present in the genres list. The sanitization of the input
            // genre list happens in the Movie service layer.
            return
            (
                DataTypeConversionUtils.MovieToMovieDto
                    (genres != null ?
                        moviesWithGenres.Where(m => !genres.Except(m.Genre.Select(g => g.Genre1)).Any()) :      // Movies where the genres list is a subset of each movie's genre list.
                        moviesWithGenres,                                                                       // No genre filtering.
                        poster),                                                                                // Denotes whether to return the poster or not.
                rangeOkay                                                                                       // Code whether the pagination was a success. Should be 200 (ok).
            );
        }

        public MovieDto GetMovieById(int id)
        { 
            // Get movie genre(s) and movie itself in a single request.
            var movieAndGenresById = 
            (
                from movie in filmFulDbContext.Movie
                    join genre in filmFulDbContext.Genre on movie.Id equals genre.MovieId
                    where movie.Id == id
                    select new { movie, genre.Genre1 }
            ).ToList();

            if (movieAndGenresById == null || !movieAndGenresById.Any()) { return null; }

            var movieGenres = movieAndGenresById.Select(g => g.Genre1).ToList();
            var movieResult = movieAndGenresById.Select(m => m.movie).First();

            return DataTypeConversionUtils.MovieToMovieDto(movieResult, movieGenres);
        }

        public IEnumerable<ActorDto> GetMovieActorsByMovieId(int id)
        {
            var movieActors = (from actor in filmFulDbContext.Actor
                                   join action in filmFulDbContext.Action on actor.Id equals action.ActorId
                                       join movie in filmFulDbContext.Movie on action.MovieId equals movie.Id
                                       where movie.Id == id
                                       select actor)
                                           .OrderBy(ac => ac.Name);

            return (movieActors == null || !movieActors.Any()) ? null : DataTypeConversionUtils.ActorToActorDto(movieActors);
        }

        public IEnumerable<DirectorDto> GetMovieDirectorsByMovieId(int id)
        {
            var movieDirectors = (from director in filmFulDbContext.Director
                                      join direction in filmFulDbContext.Direction on director.Id equals direction.DirectorId
                                          join movie in filmFulDbContext.Movie on direction.MovieId equals movie.Id
                                          where movie.Id == id
                                          select director)
                                              .OrderBy(di => di.Name);

            return (movieDirectors == null || !movieDirectors.Any()) ? null : DataTypeConversionUtils.DirectorToDirectorDto(movieDirectors);
        }
    }
}