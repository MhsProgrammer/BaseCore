using AutoMapper;
using BaseCore.Application.Contracts.Persistance;
using BaseCore.Application.Features.Products;
using BaseCore.Application.Responses;
using BaseCore.Domain.Entities;
using BaseCore.Domain.Specifications.ProductSpec;
using FluentValidation;
using MediatR;


namespace BaseCore.Application.Features.Products.Commands
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand , BaseApiResponse<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddProductCommandHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseApiResponse<Guid>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = _mapper.Map<Product>(request);
            _unitOfWork.Repository<Product>().Add(productEntity);
            await _unitOfWork.Complete();
            return new BaseApiResponse<Guid>(productEntity.Id , "محصول با موفقیت ساخته شد.");
        }
    }



    public class AddProductCommand : IRequest<BaseApiResponse<Guid>>
    {
        public string ProductName { get; set; } = string.Empty;

    }


    public class AddProductValidatior : AbstractValidator<AddProductCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProductValidatior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(p => p.ProductName)
                .NotNull()
                .NotEmpty().WithMessage("نام محصول الزامی است.");
            RuleFor(p => p)
                .MustAsync(IsProductNameUniqe).WithMessage("نام محصول تکراری است.");
        }


        private async Task<bool> IsProductNameUniqe(AddProductCommand request, CancellationToken cancellationToken)
        {
            var spec = new IsProductNameExistSpecification(request.ProductName);
            var conditon = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);

            if(conditon != null)
            {
                return false;
            }

            return true;
        }

       
    }


}
