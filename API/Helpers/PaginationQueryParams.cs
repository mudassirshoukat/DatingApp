namespace API.Helpers
{
    public class PaginationQueryParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 2;
        private int _PageSize = 2;


        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
