namespace API.Helpers.QueryParams
{
    public class LikeQueryParams : PaginationQueryParams
    {
        public int UserId { get; set; }
        public string Predicate { get; set; }
    }
}
