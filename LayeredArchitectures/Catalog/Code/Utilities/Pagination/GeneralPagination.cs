using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities.Pagination;

public static class GeneralPagination
{
    public static Func< IQueryable<T> , IQueryable<T> > CreatePagination<T>(PaginationValues paginationValues)
    {
        int pageIndex = paginationValues.PageIndex < 1 ? 1 : paginationValues.PageIndex;
        int pageSize = paginationValues.RecordsPerPage < 1 ? 10 : paginationValues.RecordsPerPage;
        const int MaxPageSize = 100;
        if (pageSize > MaxPageSize) 
            pageSize = MaxPageSize;
        int skipAmount = (pageIndex - 1) * pageSize;

        IQueryable<T> result(IQueryable<T> source) => source.Skip(skipAmount).Take(pageSize);
        return result;
    }
}
