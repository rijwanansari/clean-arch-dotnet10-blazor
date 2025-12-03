using CleanArchitecture.Application.Common;
using MediatR;

namespace CleanArchitecture.Application.Products.Commands.UpdateProductStock;

public record UpdateProductStockCommand(Guid Id, int Quantity) : IRequest<Result<bool>>;
