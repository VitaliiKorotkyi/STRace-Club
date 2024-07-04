using STRaceLifePG.Models;

namespace STRaceLifePG.Interface
{
    public interface IDASU<T>where T : class
    {
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();
    }
}
