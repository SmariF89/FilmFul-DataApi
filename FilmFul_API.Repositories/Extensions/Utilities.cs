namespace FilmFul_API.Repositories.Extensions
{
    public static class Utilities
    {
        // This is a general range-check function which is required to sanity-check all pagings.
        // These include GetAllActors, GetAllDirectors and GetAllMovies.
        public static int checkRange(int pageSize, int pageIndex, int itemCount)
        {
            if (pageSize == 0) { return 400; }                  // This check is specificially to avoid zero-divison as well as to detect bad input.
            int maxIndex = (itemCount / pageSize) - 1;          // The index is zero-based.

            if      (itemCount == 0)        { return 404; }     // Not found - Resource is empty for some reason.
            else if (pageSize > 100)        { return 413; }     // Payload too large - Max #items is 100.
            else if (pageSize < 1 ||
                     pageIndex < 0 || 
                     pageIndex > maxIndex)  { return 400; }     // Bad request. - Requests that don't make sense.

            return 0;   // Nothing is wrong.
        }
    }
}