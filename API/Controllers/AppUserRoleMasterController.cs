using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application._AppUserRoleMaster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AppUserRoleMasterController : BaseController
    {

        [HttpGet]
		public async Task<ActionResult<List<AppUserRoleMasterDto>>> List()
        {
            return await Mediator.Send(new List.Query());
        }

		[HttpGet("{id}")]
		public async Task<ActionResult<AppUserRoleMasterDto>> Details(int id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }


        [HttpPost]
		public async Task<ActionResult<AppUserRoleMasterDto>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
		public async Task<ActionResult<AppUserRoleMasterDto>> Edit(int id, Edit.Command command)
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
