using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RigORGenericRepository.GenericRepository
{
    /// <summary>
    /// This interface used to add generic curd opertion
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class
    {


        /// <summary>
        /// Get single entity by dynamic query 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Get single entity by primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetById(object id);

        /// <summary>
        /// Used to read all the record from tables
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAll();


        /// <summary>
        /// Used to read all the record from table based on the condition
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllByCondition(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// insert entity to db
        /// </summary>
        /// <param name="entity"></param>
        Task<int> Insert(TEntity entity, long? UserId = 0);

        /// <summary>
        /// Update entity in db
        /// </summary>
        /// <param name="entity"></param>
        Task<int> Update(TEntity entity, long? UserId = 0);

        /// <summary>
        /// Delete entity from db by primary key
        /// </summary>
        /// <param name="id"></param>
        Task<int> Delete(TEntity entity);
        /// <summary>
        /// Method is used to return scope_Identity from table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
         Task<int> InsertAndRId(TEntity entity, long? UserId = 0);

        /// <summary>
        /// Update specific fields 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<int> UpdateByProperty(TEntity entity, params Expression<Func<TEntity, object>>[] expression);

        Task<int> DeleteByRange(IEnumerable<TEntity> entity);

        Task<IEnumerable<TType>> GetAllByConditionandColumn<TType>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TType>> column);

        /// <summary>
        /// Method is used for dynamic sorting
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>

        Task<IEnumerable<TEntity>> GetAllByConditionAndSorting(Expression<Func<TEntity, bool>> expression, string sortByColumn, string sortOrderBy);

        /// <summary>
        /// Used for bulk update
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="setProperty"></param>
        /// <returns></returns>

        Task<int> UpdateByProperty_BasedOnMultipleIds(Expression<Func<TEntity, bool>> expression, Action<TEntity> setProperty, long? UserId = 0);

        /// <summary>
        /// Used to data from table based on where clause
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task<TEntity> GetByConditionWithTracking(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Used for bulk insert
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRange(IEnumerable<TEntity> entities);
    }
}
