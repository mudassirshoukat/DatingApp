namespace API.Helpers.QueryParams
{
    public class MessageQueryParams:PaginationQueryParams
    {
        public string UserName { get; set; }
        public string Container { get; set; } = "UnRead";
    }
}
