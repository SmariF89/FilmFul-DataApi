namespace FilmFul_API.Repositories.Extensions
{
    public static class Utilities
    {
        private const int maxPageSize = 250;
        private const int notFound = 404;
        private const int payloadTooLarge = 413;
        private const int badRequest = 400;

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

            return 0;   // Nothing is wrong.
        }
    }
}