using AMSaiian.Shared.Application.Models.Filtering;
using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Web.Contract.Queries;
using AMSaiian.Shared.Web.Options;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AMSaiian.Shared.Web.Extensions;

public static class ApiControllerExtensions
{
    public static PageContext ProcessPageQuery(this ControllerBase controller,
                                               PageQuery query,
                                               IMapper mapper,
                                               RequestQueryOptions? requestOptions = null)
    {
        requestOptions ??= new RequestQueryOptions();

        if (query.PageNumber is null
         || query.PageSize is null)
        {
            query = new PageQuery
            {
                PageNumber = query.PageNumber ?? requestOptions.DefaultPageNumber,
                PageSize = query.PageSize ?? requestOptions.DefaultPageSize
            };
        }

        PageContext context = mapper.Map<PageContext>(query);

        return context;
    }

    public static OrderContext ProcessOrderQuery(this ControllerBase controller,
                                                 OrderQuery query,
                                                 string defaultPropertyName,
                                                 IMapper mapper,
                                                 RequestQueryOptions? requestOptions = null)
    {
        requestOptions ??= new RequestQueryOptions();

        if (query.PropertyName is null)
        {
            query = new OrderQuery
            {
                PropertyName = defaultPropertyName,
                IsDescending = requestOptions.IsDescendingDefault
            };
        }

        OrderContext? context = mapper.Map<OrderContext>(query);

        return context;
    }

    public static FilterContext? ProcessFilterQuery(this ControllerBase controller,
                                                    FilterQuery query,
                                                    IMapper mapper)
    {
        FilterContext? context = null;

        if (query.PropertyName is not null
         && query.Values is not null)
        {
            context = mapper.Map<FilterContext>(query);
        }

        return context;
    }

    public static RangeContext? ProcessRangeQuery(this ControllerBase controller,
                                                  RangeQuery query,
                                                  IMapper mapper)
    {
        RangeContext? context = null;

        if (query.PropertyName is not null
         && query.Start is not null
         && query.End is not null)
        {
            context = mapper.Map<RangeContext>(query);
        }

        return context;
    }
}
