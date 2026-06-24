using AutoMapper;
using InfraStructure.Persistence.UnitOfWork;
using InfraStructure.Repositories.Generic;

namespace Application.CrudServiceGeneric
{
    public class MainServiceCrud<CreateDTO,UpdateDTO,MainEntity>:ImainServiceCRUD<CreateDTO, UpdateDTO>
        where CreateDTO : class where UpdateDTO : class where MainEntity : class
    {
        private readonly IMainInterFace<MainEntity> _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public MainServiceCrud(IMainInterFace<MainEntity> repo,IMapper mapper,IUnitOfWork uow)
        {
            _repo= repo;
            _mapper= mapper;
            _uow = uow;
        }
        public virtual async Task Create(CreateDTO create)
        {   
            var result = _mapper.Map<MainEntity>(create);
            await _repo.Create(result);
            await _uow.SaveChangesAsync();
        }
        public virtual async Task Update(UpdateDTO update)
        {
            var result = _mapper.Map<MainEntity>(update);
            await _repo.Update(result);
            await _uow.SaveChangesAsync();
        }
        public virtual async Task Delete(int id)
        {
            var result = await _repo.GetByID(id);
            try
            {
                await _repo.Delete(result);
                await _uow.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync();
                throw ex;
            }
        }
    }

}
