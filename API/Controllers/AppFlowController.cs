using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application._AppFlow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AppFlowController : AdminController
    {

        [HttpGet]
		public async Task<ActionResult<List<AppFlowDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

        
        [HttpGet("{id}", Name = "TableFlows")]
        [Route("TableFlows/{id}")]
		public async Task<ActionResult<List<AppFlowDto>>> TableFlows(int id)
        {
            return await Mediator.Send(new TableFlows.Query{ TableId = id } );
        }


		[HttpGet("{id}")]
        [Route("[action]/{id}")]
		public async Task<ActionResult<AppFlowDto>> Details(int id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }


        [HttpPost]
		public async Task<ActionResult<AppFlowDto>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
		public async Task<ActionResult<AppFlowDto>> Edit(int id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
		public async Task<ActionResult<Unit>> Delete(int id)
		{
			return await Mediator.Send(new Delete.Command { Id = id });
		}
    }
}
