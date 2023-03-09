using Microsoft.EntityFrameworkCore;
using RigorDomain.Common;
using RigORGenericRepository.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RigORGenericRepository.GenericRepository
{
    [ExcludeFromCodeCoverage]
    /// <summary>
    /// This class is used to perform generic curd operation
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        //private readonly DbContext _context;

        protected readonly DbContext _dbContext;
        private readonly DbSet<TEntity> table;
        /// <summary>
        /// Here we are going get dynamic DBContext and Entity from particular microservice
        /// </summary>
        public GenericRepository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));

            table = _dbContext.Set<TEntity>();
        }

        /// <summary>
        /// Used to data from table based on where clause
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await table.Where(expression).AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        /// <summary>
        /// Used to data from table based on where clause
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<TEntity> GetByConditionWithTracking(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await table.Where(expression).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        /// <summary>
        /// Used to read the data based on parimary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<TEntity> GetById(object id)
        {
            try
            {
                return await table.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        /// <summary>
        /// Used to retrieve all table records.
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            try
            {
                return await table.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        /// <summary>
        /// Used to get multiple records based on where clause
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllByCondition(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                return await table.Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        /// <summary>
        ///  Used to insert the data into a table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> Insert(TEntity entity, long? UserId = 0)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Insert)} entity must not be null");
            }
            try
            {
                await table.AddAsync(entity);
                return await SaveChangesAndTrackAsync(UserId);
               
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }

        }

        /// <summary>
        /// Update an existing data resource by its entity.
        /// </summary>
        /// <param name="entity"></param>
        public async Task<int> Update(TEntity entity, long? UserId = 0)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Update)} entity must not be null");
            }
            try
            {
                table.Attach(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
                /*TO get only the those column got updated just comment above 2 lines*/
                return await SaveChangesAndTrackAsync(UserId);

            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

        public async Task<int> Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Delete)} entity must not be null");
            }
            try
            {
                table.Remove(entity);
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be delete: {ex.Message}");
            }
        }

        /// <summary>
        /// Method is used to return scope_Identity from table
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAndRId(TEntity entity, long? UserId = 0)
        {
            try
            {
                await table.AddAsync(entity);
                var returnId = await SaveChangesAndTrackAsync(UserId);
                var Id = (entity as IIdentifier).Identifier;
                return Id;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }
        /// <summary>
        /// Update specific fields 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<int> UpdateByProperty(TEntity entity, params Expression<Func<TEntity, object>>[] expression)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateByProperty)} entity must not be null");
            }
            try
            {
                table.Attach(entity);
                foreach (var property in expression)
                {
                    _dbContext.Entry(entity).Property(property).IsModified = true;
                }
                return await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be update: {ex.Message}");
            }
        }


        public async Task<int> DeleteByRange(IEnumerable<TEntity> entity)
        {
            table.RemoveRange(entity);
            return await _dbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Method is used to select specific column based on where caluse 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TType>> GetAllByConditionandColumn<TType>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TType>> column)
        {
            try
            {
                return (IEnumerable<TType>)await table.Where(expression).Select(column).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        /// <summary>
        /// Method is used for dynamic sorting
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortOrderBy"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TEntity>> GetAllByConditionAndSorting(Expression<Func<TEntity, bool>> expression, string sortByColumn, string sortOrderBy)
        {
            try
            {
               
                IQueryable<TEntity> query = table;
                query = query.Where(expression);

                if (!string.IsNullOrEmpty(sortOrderBy) && !string.IsNullOrEmpty(sortByColumn))
                {
                    if (sortOrderBy.ToLower() == "asc")
                    {
                        query = query.OrderBy(p => EF.Property<object>(p, sortByColumn));
                    }
                    else
                    {
                        query = query.OrderByDescending(p => EF.Property<object>(p, sortByColumn));
                    }
                }
                return await query.ToListAsync(); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }
        /// <summary>
        /// Used for bulk update
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="setProperty"></param>
        /// <returns></returns>
        public async Task<int> UpdateByProperty_BasedOnMultipleIds(Expression<Func<TEntity, bool>> expression, Action<TEntity> setProperty, long? UserId = 0)
        {
            if (setProperty == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateByProperty_BasedOnMultipleIds)} Property must not be null");
            }
            try
            {

                var recordsToBeUpdated = await table.Where(expression).ToListAsync();
                recordsToBeUpdated.ForEach(setProperty);

                return await SaveChangesAndTrackAsync(UserId);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(table)} could not be update: {ex.Message}");
            }
        }
        /// <summary>
        /// Used to insert the multiple record 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public async Task AddRange(IEnumerable<TEntity> entities)
        {

            if (entities == null)
            {
                throw new ArgumentNullException($"{nameof(AddRange)} entity must not be null");
            }
            try
            {
                await table.AddRangeAsync(entities);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(AddRange)} could not be saved: {ex.Message}");
            }

        }

        public async Task<int> SaveChangesAndTrackAsync(long? UserId = 0)
        {

            if (UserId > 0)
            {
                AuditLogEntire auditLogEntire = new AuditLogEntire(_dbContext);
                auditLogEntire.OnBeforeSaveChanges(UserId);
            }
            return await _dbContext.SaveChangesAsync();

        }
    }


}
