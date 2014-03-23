using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PluginsTutorial.Data
{
	public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
	{
		protected DbContext Context { get; set; }
		readonly IDbSet<TEntity> _dbSet;

		protected BaseRepository(IUnitOfWork uow)
		{
			Context = uow.DataContext;
			_dbSet = Context.Set<TEntity>();
		}


		static IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includedEntities)
		{
			includedEntities.ToList().ForEach(entity => query = query.Include(entity));
			return query;
		}


		public IQueryable<TEntity> GetAll(
			params Expression<Func<TEntity, object>>[] includedEntities)
		{
			var query = Include(_dbSet, includedEntities);
			return query;
		}

		public IQueryable<TEntity> GetAll(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			params Expression<Func<TEntity, object>>[] includedEntities
			)
		{
			var query = GetAll(includedEntities);
			return orderBy != null ? orderBy(query) : query;
		}

		public IQueryable<TEntity> GetAll(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			ref int? totalRows, int index, int size,
			params Expression<Func<TEntity, object>>[] includedEntities)
		{
			var query = GetAll(orderBy, includedEntities);
			totalRows = totalRows ?? query.Count();
			var skipCount = index * size;
			query = query.Skip(skipCount).Take(size);
			return query;
		}


		public IQueryable<TEntity> GetMany(
			Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object>>[] includedEntities)
		{
			var query = GetAll(includedEntities).Where(predicate);
			return query;
		}

		public IQueryable<TEntity> GetMany(
			Expression<Func<TEntity, bool>> predicate,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			params Expression<Func<TEntity, object>>[] includedEntities)
		{
			var query = GetAll(orderBy, includedEntities).Where(predicate);
			return query;
		}

		public IQueryable<TEntity> GetMany(
			Expression<Func<TEntity, bool>> predicate,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			ref int? totalRows, int index, int size,
			params Expression<Func<TEntity, object>>[] includedEntities)
		{
			var query = GetAll(orderBy, includedEntities).Where(predicate);
			totalRows = totalRows ?? query.Count();
			var skipCount = index * size;
			query = query.Skip(skipCount).Take(size);
			return query;
		}


		public TEntity Get(
			Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object>>[] includedEntities)
		{
			return GetMany(predicate, null, includedEntities).SingleOrDefault();
		}

		public TEntity Get(params object[] keys)
		{
			return _dbSet.Find(keys);
		}


		public void Insert(TEntity entity, bool commit = false)
		{
			_dbSet.Add(entity);
			if (commit)
				Context.SaveChanges();
		}

		public void Update(TEntity entity, bool commit = false)
		{
			_dbSet.Attach(entity);
			Context.Entry(entity).State = EntityState.Modified;
			if (commit)
				Context.SaveChanges();
		}

		public void Delete(TEntity entity, bool commit = false)
		{
			if (Context.Entry(entity).State == EntityState.Detached)
			{
				_dbSet.Attach(entity);
			}
			_dbSet.Remove(entity);
			if (commit)
				Context.SaveChanges();
		}
	}
}
