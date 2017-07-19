
namespace Domain.Datas
{
    using Infrastructure.Log;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Repository base class
    /// </summary>
    /// <typeparam name="TEntity">The type of underlying entity in this repository</typeparam>
    public class Repository<TEntity> :IRepository<TEntity>
        where TEntity:Entity
    {
        #region Members

        IQueryableUnitOfWork _UnitOfWork;
       
        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of repository
        /// </summary>
        /// <param name="unitOfWork">Associated Unit Of Work</param>
        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == (IUnitOfWork)null)
                throw new ArgumentNullException("unitOfWork");

            _UnitOfWork = unitOfWork;
        }

        #endregion

        #region IRepository Members

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        public IUnitOfWork UnitOfWork
        {
            get 
            {
                return _UnitOfWork;
            }
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        /// <param name="item"><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></param>
        public virtual void Add(TEntity item)
        {

            if (item != (TEntity)null)
                GetSet().Add(item); // add new item in this set
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo("can not add null object",typeof(TEntity).ToString());
                
            }
            
        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        /// <param name="item"><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></param>
        public virtual void Remove(TEntity item)
        {
            if (item != (TEntity)null)
            {
                //attach item if not exist
                _UnitOfWork.SetUnchanged(item);

                //set as "removed"
                GetSet().Remove(item);
            }
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo("entity is null", typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        /// <param name="item"><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></param>
        public virtual void TrackItem(TEntity item)
        {
            if (item != (TEntity)null)
                _UnitOfWork.SetUnchanged<TEntity>(item);
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo("entity is null", typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        /// <param name="item"><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></param>
        public virtual void Modify(TEntity item)
        {
            if (item != (TEntity)null)
                _UnitOfWork.SetModified(item);
            else
            {
                LoggerFactory.CreateLog()
                          .LogInfo("entity is null", typeof(TEntity).ToString());
            }
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        /// <param name="id"><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></returns>
        public virtual TEntity Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
                return GetSet().Find(id);
            else
                return null;
        }
        public virtual TEntity Get(string id,params string[] include)
        {
            var set=GetSet();
            if (!string.IsNullOrEmpty(id))
            {
                IQueryable<TEntity> qry=set.AsQueryable();
                foreach(var i in include)
                {
                    qry=qry.Include(i);
                }
                return qry.Where(a => a.Id == id).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetSet();
        }
        public virtual IEnumerable<TEntity> GetAll(params string[] include)
        {
            IQueryable<TEntity> qry = GetSet().AsQueryable();
            foreach (var i in include)
            {
                qry = qry.Include(i);
            }
            return qry.AsEnumerable();
        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        /// <param name="specification"><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></returns>
        public virtual IEnumerable<TEntity> GetPaged<KProperty>(int pageIndex, int pageCount, Expression<Func<TEntity, bool>> filter,  Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending,string[] includeExpression)
        {
            IDbSet<TEntity> set = GetSet();
            IQueryable<TEntity> result = set.AsQueryable();
            if (includeExpression!=null)
            {
                foreach(var i in includeExpression)
                {
                    result=result.Include(i);
                }
            }
            result = result.Where(filter);
            if (ascending)
            {
                return result.OrderBy(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
            else
            {
                return result.OrderByDescending(orderByExpression)
                          .Skip(pageCount * pageIndex)
                          .Take(pageCount);
            }
        }
        public int GetCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return GetSet().Count(filter);
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        /// <param name="filter"><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></returns>
        public IEnumerable<TEntity> GetFiltered<KProperty>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, KProperty>> orderByExpression, bool ascending, params string[] includeExpression)
        {
            IDbSet<TEntity> set = GetSet();
            IQueryable<TEntity> result = set.AsQueryable();
            if (includeExpression != null)
            {
                foreach (var i in includeExpression)
                {
                    result = result.Include(i);
                }
            }
            result = result.Where(filter);
            if (ascending)
            {
                return result.OrderBy(orderByExpression);
            }
            else
            {
                return result.OrderByDescending(orderByExpression);
            }
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/>
        /// </summary>
        /// <param name="persisted"><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></param>
        /// <param name="current"><see cref="Microsoft.Samples.NLayerApp.Domain.Seedwork.IRepository{TValueObject}"/></param>
        public virtual void Merge(TEntity persisted, TEntity current)
        {
            _UnitOfWork.ApplyCurrentValues(persisted, current);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// <see cref="M:System.IDisposable.Dispose"/>
        /// </summary>
        public void Dispose()
        {
            if (_UnitOfWork != null)
            {
                try
                {
                    _UnitOfWork.Dispose();
                }
                catch (Exception e)
                {
                    LoggerFactory.CreateLog()
                          .LogInfo("dispose failed",e.Message);
                }
            }
        }

        #endregion

        #region Private Methods

        IDbSet<TEntity> GetSet()
        {
            return  _UnitOfWork.CreateSet<TEntity>();
        }
        #endregion
    }
}
