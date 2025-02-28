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
    public class UpdateProductCommandHanlder : IRequestHandler<UpdateProductCommand, BaseApiResponse<object>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProductCommandHanlder(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseApiResponse<object>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new NotFoundException("محصول", request.Id);
            }

            _mapper.Map(request , product);
            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.Complete();
            return new BaseApiResponse<object>("محصول با موفقیت آپدیت شد.");
        }
    }



    public class UpdateProductCommand : IRequest<BaseApiResponse<object>>
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }


    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.ProductName)
                .NotNull()
                .NotEmpty().WithMessage("نام محصول الزامی است.");
        }
    }


}
