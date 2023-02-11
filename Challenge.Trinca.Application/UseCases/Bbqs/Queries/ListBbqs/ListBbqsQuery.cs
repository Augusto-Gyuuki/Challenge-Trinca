using Challenge.Trinca.Application.Common.Queries;
using ErrorOr;
using MediatR;

namespace Challenge.Trinca.Application.UseCases.Bbqs.Queries.ListBbqs;

public sealed record ListBbqsQuery : BasePaginatedListQuery, IRequest<ErrorOr<ListBbqsQueryResult>>;
