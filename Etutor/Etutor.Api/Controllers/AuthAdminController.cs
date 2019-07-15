using System;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Etutor.Core.Models;
using Etutor.Core.Extensions;
using Etutor.Core.Exceptions;
using Etutor.BL.Dtos.Identity;
using Etutor.BL.Dtos;
using Etutor.BL.UnitOfWork;
using Etutor.DataModel.Entities;
using Etutor.Services.Interfaces;
using System.Security.Claims;
using Etutor.BL.Resources;

namespace Etutor.Api.Controllers
{
    [Route("AutenticacionAdmin")]
    public class AuthAdminController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly LockoutOptions _lockoutOptions;
        private readonly FluentValidation.IValidator<InicioSesionDto> _validatorIS;
        private readonly FluentValidation.IValidator<RestablecerContrasenaDto> _validatorRC;
        private readonly IStringLocalizer<ShareResource> _localizer;
        private readonly ITokenGeneratorService _tokenGeneratorService;
        private readonly IMapper _mapper;

        public AuthAdminController(UnitOfWork unitOfWork,
                                SignInManager<Usuario> signInManager,
                                FluentValidation.IValidator<InicioSesionDto> validatorIS,
                                FluentValidation.IValidator<RestablecerContrasenaDto> validatorRC,
                                IStringLocalizer<ShareResource> localizer,
                                ITokenGeneratorService tokenGeneratorService,
                                IOptions<LockoutOptions> options,
                                IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _validatorIS = validatorIS;
            _validatorRC = validatorRC;
            _localizer = localizer;
            _tokenGeneratorService = tokenGeneratorService;
            _lockoutOptions = options.Value;
            _mapper = mapper;
        }

        [HttpPost("InicioSesion")]
        public async Task<IActionResult> Login([FromBody] InicioSesionDto model)
        {
            if (model == null) throw new ArgumentNullException(typeof(InicioSesionDto).GetCleanNameFromDto());
            else if (!_validatorIS.Validate(model).IsValid) throw new ValidationException(_validatorIS.Validate(model).Errors.ToMessage());

            var appUser = await _unitOfWork.UsuarioRepository.FindByNameAsync(model.Usuario.Split('@')[0]);
            if (appUser.Estado == EntityStatus.Inactivo) throw new ValidationException("Disabled user, contact administrator to obtain more information.", true);

            var signInResult = await _signInManager.PasswordSignInAsync(appUser.UserName, model.Contrasena, isPersistent: false, lockoutOnFailure: true);
            if (signInResult.IsLockedOut) throw new ValidationException(string.Format(_localizer["User blocked by failed attempts, try again after {0} minutes."], _lockoutOptions.DefaultLockoutTimeSpan.Minutes));
            bool isValid = signInResult.Succeeded;

            if (!isValid) throw new ValidationException("User or password incorrect.", true);

            var claims = await _unitOfWork.UsuarioRepository.GetClaimsAsync(appUser);
            var payload = new
            {
                token = _tokenGeneratorService.GenerateJwtToken($"{appUser.Nombre} {appUser.Apellido}", appUser, claims),
                changePassword = !appUser.UltimoAcceso.HasValue,
                user = _mapper.Map<UsuarioDto>(appUser),

            };

            if (appUser.UltimoAcceso.HasValue)
            {
                appUser.UltimoAcceso = DateTime.Now;
                await _unitOfWork.UsuarioRepository.UpdateAsync(appUser);
            }

            return Ok(payload);
        }

        [Authorize]
        [HttpPost("RefrescarToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var userName = User.Claims.Where(c => c.Type.Contains(ClaimTypes.Name)).Select(c => c.Value).FirstOrDefault();
            if (userName == null) throw new ValidationException("Invalid security token refresh attempt.", true);

            var appUser = await _unitOfWork.UsuarioRepository.FindByNameAsync(userName);

            var claims = await _unitOfWork.UsuarioRepository.GetClaimsAsync(appUser);
            return Ok(_tokenGeneratorService.GenerateJwtToken($"{appUser.Nombre} {appUser.Apellido}", appUser, claims));
        }

        [Authorize]
        [HttpPost("RestablecerContrasena")]
        public async Task<IActionResult> ResetPassword([FromBody] RestablecerContrasenaDto model)
        {
            if (model == null) throw new ArgumentNullException(typeof(RestablecerContrasenaDto).GetCleanNameFromDto());
            else if (!_validatorRC.Validate(model).IsValid) throw new ValidationException(_validatorRC.Validate(model).Errors.ToMessage());

            var userName = User.Claims.Where(c => c.Type.Contains(ClaimTypes.Name)).Select(c => c.Value).FirstOrDefault();
            if (userName == null) throw new ValidationException("Invalid change password attempt.", true);

            await _unitOfWork.UsuarioRepository.ChangePasswordAsync(userName, model.Contrasena, model.NuevaContrasena);

            return Ok();
        }




    }
}