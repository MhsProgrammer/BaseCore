using BaseCore.Application.Features.Products.Commands;
using BaseCore.Application.Features.Products.Queries;
using BaseCore.Application.Responses;
using BaseCore.Domain.Specifications.ProductSpec;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BaseCore.Api.Controllers
{
    [Authorize]
    public class ProductController : BaseApiController
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        [SwaggerOperation(Summary = "ایجاد محصول جدید", Description = "این API برای ایجاد یک محصول جدید استفاده می‌شود.")]
        [SwaggerResponse(StatusCodes.Status201Created, "محصول با موفقیت ایجاد شد.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "اطلاعات وارد شده معتبر نیست.")]
        public async Task<ActionResult<BaseApiResponse<Guid>>> PostProduct(AddProductCommand addProductCommand)
        {
            var result = await _mediator.Send(addProductCommand);

            ////return Ok(result);
            return CreatedAtAction(nameof(PostProduct), result);
        }


        [HttpPut("Update")]
        public async Task<ActionResult<BaseApiResponse<object>>> UpdateProduct(UpdateProductCommand updateProductCommand)
        {
            var result = await _mediator.Send(updateProductCommand);
            return Ok(result);
        }


        [HttpDelete("Delete")]
        public async Task<ActionResult<BaseApiResponse<Guid>>> DeleteProduct(DeleteProductCommand deleteCategoryCommand)
        {
            var result = await _mediator.Send(deleteCategoryCommand);

            return Ok(result);
        }


        [HttpGet("{productId:Guid}")]
        //[ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ActionResult<BaseApiResponse<GetProductQueryResponse>>> GetProduct([FromRoute] Guid productId)
        {
            var query = new GetProductQuery();
            query.ProductId = productId;
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [HttpGet]
        public async Task<ActionResult<BaseApiResponse<GetProductListQueryResponse>>> GetAllProdcuts([FromQuery] ProductSpecParams productSpecParams)
        {
            var getProductListQuery = new GetProductListQuery();
            getProductListQuery.specParams = productSpecParams;
            var result = await _mediator.Send(getProductListQuery);

            return Ok(result);
        }



    }
}
