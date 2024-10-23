using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagement.DAL.Interfaces.Repositories.Base
{
    public interface IRepositoryBase<T>
    {
        //Возвращает все сущности из базы данных
        IQueryable<T> GetAllEntities(bool trackChanges);

        //Позволяет получить сущности, соответствующие условию(для фильтрации)
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

        //Создание 
        Task CreateEntity(T entity);

        //Обновление сущ.
        void UpdateEntity(T entity);

        //Удаление
        void DeleteEntity(T entity);
    }
}
