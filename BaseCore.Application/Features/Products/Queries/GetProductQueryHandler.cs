using AutoMapper;
using BaseCore.Application.Contracts.Persistance;
using BaseCore.Application.Exeptions;
using BaseCore.Application.Responses;
using BaseCore.Domain.Entities;
using MediatR;

namespace BaseCore.Application.Features.Products.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, BaseApiResponse<GetProductQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetProductQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseApiResponse<GetProductQueryResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(request.ProductId);

            if (product == null) throw new NotFoundException("محصول", request.ProductId);

            var productDto = _mapper.Map<ProductDto>(product);
            var getProductQueryResponse = new GetProductQueryResponse();
            getProductQueryResponse.Product = productDto;

            return new BaseApiResponse<GetProductQueryResponse>(getProductQueryResponse);


        }
    }


    public class GetProductQuery : IRequest<BaseApiResponse<GetProductQueryResponse>>
    {
        public Guid ProductId { get; set; }
    }


    public class GetProductQueryResponse 
    {
        public ProductDto Product { get; set; } = default!;
    }
}
