using CleanArchitecture.Application.Common;
using MediatR;

namespace CleanArchitecture.Application.Products.Commands.ActivateProduct;

public record ActivateProductCommand(Guid Id) : IRequest<Result<bool>>;
