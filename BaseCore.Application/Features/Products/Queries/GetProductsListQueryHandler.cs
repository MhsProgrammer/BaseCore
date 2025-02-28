using AutoMapper;
using BaseCore.Application.Contracts.Persistance;
using BaseCore.Application.Models;
using BaseCore.Application.Responses;
using BaseCore.Domain.Entities;
using BaseCore.Domain.Specifications.ProductSpec;
using MediatR;

namespace BaseCore.Application.Features.Products.Queries
{
    public class GetProductsListQueryHandler : IRequestHandler<GetProductListQuery, BaseApiResponse<GetProductListQueryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetProductsListQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseApiResponse<GetProductListQueryResponse>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductSpecification(request.specParams);

            var products = await _unitOfWork.Repository<Product>().ListAsync(spec);

            var productDtos = _mapper.Map<List<ProductDto>>(products);

            var getProductListQueryResponse = new GetProductListQueryResponse();
            getProductListQueryResponse.Products = new Pagination<ProductDto>(request.specParams.PageIndex,
                request.specParams.PageSize , products.Count , productDtos);

            return new BaseApiResponse<GetProductListQueryResponse>(getProductListQueryResponse);
        }
    }


    public class GetProductListQuery :  IRequest<BaseApiResponse<GetProductListQueryResponse>>
    {
        public ProductSpecParams specParams { get; set; } = default!;
    }


    public class GetProductListQueryResponse 
    {
        public Pagination<ProductDto> Products { get; set; } = default!;
    }
}
