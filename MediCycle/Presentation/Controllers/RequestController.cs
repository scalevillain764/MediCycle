using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Requests;
namespace Presentation.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : BaseController
    {
        private readonly IMediator _mediator;
        public RequestController(IMediator mediator)
        {
            _mediator = mediator;
        }
        


    }
}
