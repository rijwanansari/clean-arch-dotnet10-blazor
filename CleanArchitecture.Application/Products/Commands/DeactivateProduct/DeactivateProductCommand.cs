using CleanArchitecture.Application.Common;
using MediatR;

namespace CleanArchitecture.Application.Products.Commands.DeactivateProduct;

public record DeactivateProductCommand(Guid Id) : IRequest<Result<bool>>;
