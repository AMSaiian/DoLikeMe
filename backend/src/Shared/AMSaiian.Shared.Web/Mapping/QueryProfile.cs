using AMSaiian.Shared.Application.Models.Filtering;
using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Web.Contract.Queries;
using AutoMapper;

namespace AMSaiian.Shared.Web.Mapping;

public sealed class QueryProfile : Profile
{
    public QueryProfile()
    {
        CreateMap<FilterQuery, FilterContext>();

        CreateMap<OrderQuery, OrderContext>();

        CreateMap<RangeQuery, RangeContext>();

        CreateMap<PageQuery, PageContext>();
    }
}
