using CleanArchitecture.Application.Abstractions.Data;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Domain.Repositories;
using MediatR;

namespace CleanArchitecture.Application.Products.Commands.DeactivateProduct;

public class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand, Result<bool>>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (product is null)
        {
            return Result<bool>.Failure($"Product with ID {request.Id} was not found");
        }

        product.Deactivate();
        _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
