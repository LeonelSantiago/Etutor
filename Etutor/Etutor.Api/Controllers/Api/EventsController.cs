using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Etutor.Api.Filters;
using Etutor.BL.Dtos;
using Etutor.BL.UnitOfWork;
using Etutor.Core.Extensions;
using Etutor.Core.Models.Enums;
using Etutor.DataModel.Entities;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Etutor.Api.Controllers.Api
{
    //[Area("Authorization")]
    public class EventsController : ApplicationBaseApiController<Events, EventsDto>
    {
        protected readonly IConfiguration _configuration;

        public EventsController(UnitOfWork unitOfWork,
                                IConfiguration configuration,
                                IMapper mapper)
            : base(unitOfWork, mapper)
        {
            _configuration = configuration;
        }

        // POST api/values/
        //[RequiresPermissionFilter(OperationsPermission.Create)]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] EventsDto dto)
        {
            if (dto == null) throw new ArgumentNullException(typeof(UserDto).GetCleanNameFromDto());

            var model = _mapper.Map<Events>(dto);
            await _unitOfWork.EventsRepository.AddAsync(model);
            //await _userEmailNotificacionService.UserModificationNotification(dto, TipoCorreo.CreacionUsuario);

            return Ok(_mapper.Map(model, dto));
        }

        //[RequiresPermissionFilter(OperationsPermission.Update)]
        [HttpPut("{key}")]
        public override async Task<IActionResult> Put([FromODataUri] int key, [FromBody] EventsDto dto)
        {
            if (dto == null) throw new ArgumentNullException(typeof(EventsDto).GetCleanNameFromDto());

            var model = await _unitOfWork.EventsRepository.FindAsync(key);
            model = _mapper.Map(dto, model);
            await _unitOfWork.EventsRepository.UpdateAsync(model);

            return Updated(_mapper.Map(model, dto));
        }
    }
}