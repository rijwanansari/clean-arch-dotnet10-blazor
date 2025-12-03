using CleanArchitecture.Application.Common;
using CleanArchitecture.Application.DTOs.Common;
using CleanArchitecture.Application.DTOs.Products;
using CleanArchitecture.Domain.Repositories;
using MediatR;

namespace CleanArchitecture.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<PagedResponse<ProductDto>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<PagedResponse<ProductDto>>> Handle(GetProductsQuery request, 
        CancellationToken cancellationToken)
    {
        var (products, totalCount) = await _productRepository.GetPagedAsync(
            request.Page, request.PageSize, cancellationToken);

        var productDtos = products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price.Amount,
            p.Price.Currency,
            p.StockQuantity,
            p.Category,
            p.IsActive,
            p.CreatedAt
        )).ToList();

        var response = new PagedResponse<ProductDto>(productDtos, totalCount, request.Page, request.PageSize);
        return Result<PagedResponse<ProductDto>>.Success(response);
    }
}
