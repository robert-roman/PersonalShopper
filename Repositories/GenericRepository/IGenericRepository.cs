namespace PersonalShopper.DAL.Repositories.GenericRepository
{
    public interface IGenericRepository<TEntity>
    {
        //Create
        Task Create(TEntity entity);

        //Read
        Task<List<TEntity>> GetAll();
        Task<TEntity> GetById(int id);
        //Task<TEntity> GetByNameAsync(string name);

        Task<TEntity> GetByComposedId(int id1, int id2);

        //Update
        Task Update(TEntity entity);

        //Delete
        Task Delete(TEntity entity);

        //Save
        Task<bool> SaveAsync();
    }
}
