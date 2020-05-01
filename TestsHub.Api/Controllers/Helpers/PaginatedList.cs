using System.Collections.Generic;
using System.Linq;

namespace TestHub.Api.Controllers.Helpers
{

    public static class PaginatedListBuilder
    {
        private const int DFAULT_PAGE_SIZE = 20;

        public static PaginatedList<T> CreatePaginatedList<T>(IQueryable<T> list, int? pageNumber, int? pageSize,  string baseUrl)
        {            
            return CreatePaginatedList(list,
                pageNumber.HasValue? pageNumber.Value : 1,
                pageSize.HasValue? pageSize.Value : DFAULT_PAGE_SIZE, 
                baseUrl);
        }


        public static PaginatedList<T> CreatePaginatedList<T>(IQueryable<T> list, int pageNumber, int pageSize, string baseUrl)
        {
            if (pageNumber < 1)
                pageNumber = 1;
            
            if (pageSize < 1 )
                pageSize = DFAULT_PAGE_SIZE;

            var count = list.Count();
            var r = new PaginatedList<T>(list.Skip((pageNumber-1) * pageSize).Take(pageSize));
            r.Meta.Pagination.CurrentPage = pageNumber;
            r.Meta.Pagination.PageSize = pageSize;
            r.Meta.Pagination.ItemsCount = count;
            r.Meta.Pagination.TotalPages = count % pageSize == 0 ? count / pageSize : (count / pageSize) + 1;
            r.Meta.Pagination.Links = new Links() { };
            if (r.Meta.Pagination.CurrentPage  < r.Meta.Pagination.TotalPages)
            {
                r.Meta.Pagination.Links.Next = $"{baseUrl}?page={r.Meta.Pagination.CurrentPage + 1}&pageSize={pageSize}";
            }
            return r;
        }
    }
    public class PaginatedList<T>
    {
        

        public PaginatedList(IEnumerable<T> list)
        {
            Data = list;
        }        

        public IEnumerable<T> Data { get; set; }
        public Meta Meta { get; set; } = new Meta() { Pagination = new Pagination() };
    }

    public class Meta 
    {
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public int ItemsCount { get; set; }        
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public Links Links { get; set; }
    }

    public class Links
    {
        public string Next { get; set; }
    }

}
