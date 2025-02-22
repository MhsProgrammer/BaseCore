using AutoMapper;
using FluentValidation;
using MediatR;


namespace BaseCore.Application.Features.Product.Commands
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductDto>
    {
        private readonly IMapper _mapper;
        public AddProductCommandHandler(IMapper mapper)
        {
            _mapper = mapper;
        }
        public Task<ProductDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }



    public class AddProductCommand : IRequest<ProductDto>
    {
        public string PorudctName { get; set; } = string.Empty;

    }


    public class AddProductValidatior : AbstractValidator<AddProductCommand>
    {
        public AddProductValidatior()
        {
            RuleFor(p => p.PorudctName)
                .NotNull()
                .NotEmpty().WithMessage("نام محصول الزامی است.");
        }
    }


}
