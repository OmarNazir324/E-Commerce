namespace Application.CrudServiceGeneric
{
    public interface ImainServiceCRUD<CreateEntity,UpdateEntity> where CreateEntity : class 
        where UpdateEntity : class
    {
        Task Create(CreateEntity t);
        Task Update(UpdateEntity t);
        Task Delete(int id);
    }
}
