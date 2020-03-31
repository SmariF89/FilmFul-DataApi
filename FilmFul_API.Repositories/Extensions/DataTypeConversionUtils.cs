using System.Collections.Generic;
using System.Linq;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Models.Entities;

namespace FilmFul_API.Repositories.Extensions
{
    public static class DataTypeConversionUtils
    {
        public static IEnumerable<ActorDto> ActorToActorDto(IEnumerable<Actor> actors) => actors.Select(actor => new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name
        });

        public static ActorDto ActorToActorDto(Actor actor) => new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name
        };

        public static IEnumerable<DirectorDto> DirectorToDirectorDto(IEnumerable<Director> directors) => directors.Select(director => new DirectorDto
        {
            Id = director.Id,
            Name = director.Name
        });

        public static DirectorDto DirectorToDirectorDto(Director director) => new DirectorDto
        {
            Id = director.Id,
            Name = director.Name
        };

        public static IEnumerable<MovieDto> MovieToMovieDto(IEnumerable<Movie> movies, bool poster) => movies.Select(movie => new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Poster = poster ? movie.Poster : null,
            Description = movie.Description,
            Duration = movie.Duration,
            ReleaseYear = movie.ReleaseYear,
            RatingImdb = movie.RatingImdb,
            RatingMetascore = movie.RatingMetascore,
            Certificate = movie.Certificate,
            Gross = movie.Gross,
            VoteCount = movie.VoteCount
        });

        public static MovieDto MovieToMovieDto(Movie movie) => new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Poster = movie.Poster,
            Description = movie.Description,
            Duration = movie.Duration,
            ReleaseYear = movie.ReleaseYear,
            RatingImdb = movie.RatingImdb,
            RatingMetascore = movie.RatingMetascore,
            Certificate = movie.Certificate,
            Gross = movie.Gross,
            VoteCount = movie.VoteCount
        };
    }
}