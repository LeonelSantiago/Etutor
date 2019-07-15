using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Etutor.BL.Abstract;
using Etutor.BL.UnitOfWork;
using Etutor.Core;
using Etutor.Core.Extensions;

namespace Etutor.Api.Controllers
{
    [Route("Api/[controller]")]
    public abstract class ApplicationBaseApiController<T, TD> : ODataController
                where T : class, IEntityBase, new()
                where TD : class, IEntityBaseDto, new()
    {
        protected readonly UnitOfWork _unitOfWork;
        protected readonly IEntityBaseRepository<T> _repository;
        protected readonly IMapper _mapper;

        public ApplicationBaseApiController(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.Get<T>();
            _mapper = mapper;
        }

        //�GET:�api/values
        [EnableQuery(MaxExpansionDepth = 5, MaxAnyAllExpressionDepth = 5)]
        //[RequiresPermissionFilter(OperationsPermission.Read)]
        [HttpGet]
        public virtual IQueryable<T> Get()
        {
            return _repository.GetAll();
        }

        // GET api/values/5 
        [EnableQuery(MaxExpansionDepth = 5, MaxAnyAllExpressionDepth = 5)]
        //[RequiresPermissionFilter(OperationsPermission.Read)]
        [HttpGet("{key}")]
        public virtual SingleResult<T> Get(int key)
        {
            var queryable = _repository.GetAll().Where(x => x.Id == key);
            return SingleResult.Create(queryable);
        }

        // POST api/values/
        //[RequiresPermissionFilter(OperationsPermission.Create)]
        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] TD dto)
        {
            if (dto == null) throw new ArgumentNullException(typeof(TD).GetCleanNameFromDto());

            var model = _mapper.Map<T>(dto);

            await _repository.AddAsync(model);
            await _unitOfWork.SaveAsync();

            return Ok(_mapper.Map(model, dto));
        }

        //[RequiresPermissionFilter(OperationsPermission.Update)]
        [HttpPatch("{key}")]
        public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<TD> pacthDto)
        {
            if (pacthDto == null) throw new ArgumentNullException(typeof(TD).GetCleanNameFromDto());

            var model = await _repository.FindAsync(key);

            var dto = _mapper.Map<TD>(model);
            pacthDto.Patch(dto);
            _mapper.Map(dto, model);

            _repository.Update(model);
            await _unitOfWork.SaveAsync();

            return Updated(dto);
        }

        //[RequiresPermissionFilter(OperationsPermission.Update)]
        [HttpPut("{key}")]
        public virtual async Task<IActionResult> Put([FromODataUri] int key, [FromBody] TD dto)
        {
            if (dto == null) throw new ArgumentNullException(typeof(TD).GetCleanNameFromDto());

            var model = await _repository.FindAsync(key);

            model = _mapper.Map(dto, model);

            _repository.Update(model);
            await _unitOfWork.SaveAsync();

            return Updated(_mapper.Map(model, dto));
        }

        //[RequiresPermissionFilter(OperationsPermission.Delete)]
        [HttpDelete("{key}")]
        public virtual async Task<IActionResult> Delete([FromODataUri] int key)
        {
            await _repository.RemoveAsync(key);
            await _unitOfWork.SaveAsync();

            return Ok();
        }
    }
}