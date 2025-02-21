using BaseCore.Application.Features.Product;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseCore.Api.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> PostProduct(AddProductCommand addProductCommand)
        {
            var result = await _mediator.Send(addProductCommand);

            return Ok(result);
        }
    }
}
