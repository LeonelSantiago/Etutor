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
    [Area("Authorizacion")]
    public class UsuarioController : ApplicationBaseApiController<Usuario, UsuarioDto>
    {
        protected readonly IConfiguration _configuration;

        public UsuarioController(UnitOfWork unitOfWork,
                                IConfiguration configuration,
                                IMapper mapper)
            : base(unitOfWork, mapper)
        {
            _configuration = configuration;
        }

        // POST api/values/
        [RequiresPermissionFilter(OperationsPermission.Create)]
        [HttpPost]
        public override async Task<IActionResult> Post([FromBody] UsuarioDto dto)
        {
            if (dto == null) throw new ArgumentNullException(typeof(UsuarioDto).GetCleanNameFromDto());

            var model = _mapper.Map<Usuario>(dto);
            await _unitOfWork.UsuarioRepository.AddAsync(model, dto.Contrasena);
            //await _userEmailNotificacionService.UserModificationNotification(dto, TipoCorreo.CreacionUsuario);

            return Ok(_mapper.Map(model, dto));
        }

        [RequiresPermissionFilter(OperationsPermission.Update)]
        [HttpPut("{key}")]
        public override async Task<IActionResult> Put([FromODataUri] int key, [FromBody] UsuarioDto dto)
        {
            if (dto == null) throw new ArgumentNullException(typeof(UsuarioDto).GetCleanNameFromDto());

            var model = await _unitOfWork.UsuarioRepository.FindAsync(key);
            model = _mapper.Map(dto, model);
            await _unitOfWork.UsuarioRepository.UpdateAsync(model, dto.Contrasena);

            return Updated(_mapper.Map(model, dto));
        }
    }
}
