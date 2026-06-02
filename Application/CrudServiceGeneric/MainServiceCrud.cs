using AutoMapper;
using InfraStructure.Repositories.Generic;

namespace Application.CrudServiceGeneric
{
    public class MainServiceCrud<CreateDTO,UpdateDTO,MainEntity>:ImainServiceCRUD<CreateDTO, UpdateDTO>
        where CreateDTO : class where UpdateDTO : class where MainEntity : class
    {
        private readonly IMainInterFace<MainEntity> _repo;
        private readonly IMapper _mapper;
        public MainServiceCrud(IMainInterFace<MainEntity> repo,IMapper mapper)
        {
            _repo= repo;
            _mapper= mapper;
        }
        public virtual async Task Create(CreateDTO create)
        {
            var result = _mapper.Map<MainEntity>(create);
            await _repo.Create(result);
        }
        public virtual async Task Update(UpdateDTO update)
        {
            var result = _mapper.Map<MainEntity>(update);
            await _repo.Update(result);
        }
        public virtual async Task Delete(int id)
        {
            var result = await _repo.GetByID(id);
            await _repo.Delete(result);
        }
    }

}
