using AutoMapper;
using BaseCore.Application.Contracts.Persistance;
using BaseCore.Application.Exeptions;
using BaseCore.Application.Responses;
using BaseCore.Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Application.Features.Products.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, BaseApiResponse<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteProductCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseApiResponse<Guid>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(request.ProductId);

            if(product == null)
            {
                throw new NotFoundException("محصول" , request.ProductId);
            }

            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.Complete();

            return new BaseApiResponse<Guid>(product.Id , "محصول با موفقیت حذف شد");
        }
    }



    public class DeleteProductCommand : IRequest<BaseApiResponse<Guid>>
    {
        public Guid ProductId { get; set; }
    }


    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(p => p.ProductId)
                .NotNull()
                .NotEmpty().WithMessage("شناسه محصول الزامی است.");
        }
    }
}
