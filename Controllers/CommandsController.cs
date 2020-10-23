using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.Mvc;

namespace Commander.Controllers
{
    //api/commands
    [Route("api/commands")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        // GET api/commands
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetCommands()
        {
            var commandItems = _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        // GET api/commands/{id}
        // Naming has a few gotchas in async programming
        [HttpGet("{id}", Name="GetCommandsById")]
        public ActionResult<CommandReadDto> GetCommandsById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if(commandItem != null) return Ok(_mapper.Map<CommandReadDto>(commandItem));
            return NotFound();
        }

        // POST api/commands/
        [HttpPost()]
        public ActionResult<CommandCreateDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();
            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandsById), new{Id = commandReadDto.Id}, commandReadDto);
        }

        // PUT api/commands/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if(commandModelFromRepo == null) return NotFound();

            var commandModel = _mapper.Map(commandUpdateDto, commandModelFromRepo);
            _repository.UpdateCommand(commandModel);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}