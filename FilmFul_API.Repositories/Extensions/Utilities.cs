using System.Collections.Generic;
using System.Linq;

namespace FilmFul_API.Repositories.Extensions
{
    public static class Utilities
    {
        public const int maxPageSize = 100;
        public const int notFound = 404;
        public const int payloadTooLarge = 413;
        public const int badRequest = 400;
        public const int ok = 200;

        // This is a general range-check function which is required to sanity-check all pagings.
        // These include GetAllActors, GetAllDirectors and GetAllMovies.
        public static int checkRange(int pageSize, int pageIndex, int itemCount)
        {
            if (pageSize == 0) { return 400; }                  // This check is specificially to avoid zero-divison as well as to detect bad input.

            // If the division returns a natural number the max index must be lowered by one.
            // -> E.g. itemCount=250 / pageSize=50 == 5. The max index should be 4 because the index starts at zero.
            //    If it was 5, it would mean that the 250 items could be divided into 6 pages (0-5), each containing 50 items.
            //    That would make 300 items, which is incorrect as the itemCount is 250. Thus, the max index should be lowered
            //    to 4 as the 250 items can only be divided into 5 pages (0-4) if each page is to contain 50 items.
            // If the division returns a real number, the integer division makes sure to mathematically floor the number.
            // There is no need to lower the max index by one.
            // -> E.g. itemCount=250 / pageSize=60 == 4.1666... == 4. The max index is 4. That means that 250 items can be
            //    fit in 5 pages (0-4) where each contains 60 items except the last page which obviously has to contain fewer items:
            //      0: 1    -   60      (60 items)
            //      1: 61   -   120     (60 items)
            //      2: 121  -   180     (60 items)
            //      3: 181  -   240     (60 items)
            //      4: 241  -   250     (10 items)
            int maxIndex = (itemCount % pageSize == 0) ? ((itemCount / pageSize) - 1) : (itemCount / pageSize);

            if      (itemCount == 0)            { return notFound; }        // Not found - Resource is empty for some reason.
            else if (pageSize > maxPageSize)    { return payloadTooLarge; } // Payload too large - Max #items is 100.
            else if (pageSize < 1 ||
                     pageIndex < 0 || 
                     pageIndex > maxIndex)      { return badRequest; }      // Bad request. - Requests that don't make sense.

            return ok;   // Nothing is wrong.
        }

        private static readonly HashSet<string> validGenres = new HashSet<string>
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

        // User input sanitization - This function capitalizes genre strings from the query parameters
        // such that they will match the way they are capitalized in the database.
        private static string correctGenreCaps(string genre)
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

        public static bool genresOkay(ref List<string> genres)
        {
            // Genre query parameter not provided - That is okay.
            if (genres == null) { return true; }

            // This list will contain the genres with fixed capitalization.
            List<string> genresFixed = new List<string>();

            // If some string in genres is not a key in the validGenres HashSet, we have a bad request.
            foreach (string genre in genres)
            {
                string genreFixed = correctGenreCaps(genre);
                if (!validGenres.Contains(genreFixed)) { return false; }
                genresFixed.Add(genreFixed);
            }

            // If we get here, all genre capitalization has been corrected and the genres are present
            // in the validGenres HashSet. The original genres list is emptied and filled with the corrected genres.
            genres.Clear();
            genres.AddRange(genresFixed);

            return true;
        }
    }
}