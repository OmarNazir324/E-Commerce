namespace Application.CrudServiceGeneric
{
    public interface ImainServiceCRUD<CreateEntity, UpdateEntity, BaseEntity> where CreateEntity : class
        where UpdateEntity : class
    {
        Task<(bool Status, String MSG, BaseEntity? entity)> Create(CreateEntity t, params object?[] parameters);
        Task Update(UpdateEntity t);
        Task<(Boolean Status, String? msg)> Delete(int id, params object?[] parameters);
        Task<(Boolean Status, String? msg)> Delete(BaseEntity t, params object?[] parameters);
    }
}
