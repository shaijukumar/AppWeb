using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Application.Todo;
//using Application.ToDos;
using Application.User;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    [AllowAnonymous]
    public class UserManagerController : AdminController
    {
        
        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            return await Mediator.Send(new List.Query());
        }

        // GET api/values/5
        [HttpGet("{id}")]     
        public async Task<ActionResult<UserDTO>> Get(string Id)
        {
            return await Mediator.Send(new Details.Query{Username = Id});
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Create(Create.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> Edit(string id, Edit.Command command)
        {
            command.Username = id;
            return await Mediator.Send(command);
        }
/*
        [HttpPut("{id}")]
        public async Task<ActionResult<ToDoDto>> Edit(Guid id, Edit.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await Mediator.Send(new Delete.Command{Id = id});
        }
        */
    }
}
 