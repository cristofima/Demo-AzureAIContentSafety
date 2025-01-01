namespace AzureAIContentSafety.API.DTO.Responses
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalItems { get; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedList(List<T> items, int pageNumber, int totalPages, int totalItems)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = totalPages;
            TotalItems = totalItems;
        }
    }
}
