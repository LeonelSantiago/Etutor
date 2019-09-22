using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Etutor.Api.Filters;
using System;
using System.Threading.Tasks;
using Etutor.BL.Dtos;
using Etutor.DataModel.Entities;
using Etutor.BL.UnitOfWork;
using Etutor.Core.Models.Enums;
using Etutor.Core.Extensions;

namespace Etutor.Api.Controllers.Api
{
    [Area("Authorization")]
    public class UserController : ApplicationBaseApiController<User, UserDto>
    {
        protected readonly IConfiguration _configuration;

        public UserController(UnitOfWork unitOfWork,
                                IConfiguration configuration,
                                IMapper mapper)
            : base(unitOfWork, mapper)
        {
            _configuration = configuration;
        }

        // POST api/values/
        [RequiresPermissionFilter(OperationsPermission.Create)]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] UserDto dto)
        {
            if (dto == null) throw new ArgumentNullException(typeof(UserDto).GetCleanNameFromDto());

            var model = _mapper.Map<User>(dto);
            await _unitOfWork.UsuarioRepository.AddAsync(model, dto.Password);
            //await _userEmailNotificacionService.UserModificationNotification(dto, TipoCorreo.CreacionUsuario);

            return Ok(_mapper.Map(model, dto));
        }

        [RequiresPermissionFilter(OperationsPermission.Update)]
        [HttpPut("{key}")]
        public override async Task<IActionResult> Put([FromODataUri] int key, [FromBody] UserDto dto)
        {
            if (dto == null) throw new ArgumentNullException(typeof(UserDto).GetCleanNameFromDto());

            var model = await _unitOfWork.UsuarioRepository.FindAsync(key);
            model = _mapper.Map(dto, model);
            await _unitOfWork.UsuarioRepository.UpdateAsync(model, dto.Password);

            return Updated(_mapper.Map(model, dto));
        }
    }
}
