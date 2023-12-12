using Microsoft.EntityFrameworkCore;

namespace API.Helpers
{
    public class PagedList<T>:List<T>

    {
        public PagedList(IEnumerable<T> Items, int Count, int PageNumber, int PageSize)
        {
            this.CurrentPage = PageNumber;
            this.TotalPages = (int) Math.Ceiling((Count/ (double)PageSize));
            this.PageSize = PageSize;
            this.TotalCount = Count;
            AddRange(Items); 
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> Source,int PageNumber,int PageSize)
        {
            var count = await Source.CountAsync();
            var items = await Source.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
            return new  PagedList<T>(items,count,PageNumber,PageSize);

        }
    }
}
