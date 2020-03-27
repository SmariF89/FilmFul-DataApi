using System.Collections.Generic;
using System.Linq;
using FilmFul_API.Models.Dtos;
using FilmFul_API.Models.Entities;

namespace FilmFul_API.Repositories.Extensions
{
    public static class DataTypeConversionUtils
    {
        public static IEnumerable<ActorDto> ActorToActorDto(IEnumerable<Actor> Actors) => Actors.Select(actor => new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name
        });

        public static ActorDto ActorToActorDto(Actor actor) => new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name
        };

        public static IEnumerable<DirectorDto> DirectorToDirectorDto(IEnumerable<Director> Directors) => Directors.Select(director => new DirectorDto
        {
            Id = director.Id,
            Name = director.Name
        });

        public static DirectorDto DirectorToDirectorDto(Director director) => new DirectorDto
        {
            Id = director.Id,
            Name = director.Name
        };
    }
}