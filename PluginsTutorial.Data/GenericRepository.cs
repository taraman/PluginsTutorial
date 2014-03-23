using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PluginsTutorial.Data
{
	public class GenericRepository : IGenericRepository
	{
		DbContext Context { get; set; }
	
		public GenericRepository(IUnitOfWork uow)
		{
			Context = uow.DataContext;
		}

		static IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class
		{
			includedEntities.ToList().ForEach(entity => query = query.Include(entity));
			return query;
		}
		

		public IQueryable<TEntity> GetAll<TEntity>(
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class
		{
			var dbSet = Context.Set<TEntity>();
			var query = Include(dbSet, includedEntities);
			return query;
		}
		
		public IQueryable<TEntity> GetAll<TEntity>(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			params Expression<Func<TEntity, object>>[] includedEntities
			) where TEntity : class
		{
			var query = GetAll(includedEntities);
			return orderBy != null ? orderBy(query) : query;
		}

		public IQueryable<TEntity> GetAll<TEntity>(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			ref int? totalRows, int index, int size,
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class
		{
			var query = GetAll(orderBy, includedEntities);
			totalRows = totalRows ?? query.Count();
			var skipCount = index * size;
			query = query.Skip(skipCount).Take(size);
			return query;
		}


		public IQueryable<TEntity> GetMany<TEntity>(
			Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class
		{
			var query = GetAll(includedEntities).Where(predicate);
			return query;
		}
		
		public IQueryable<TEntity> GetMany<TEntity>(
			Expression<Func<TEntity, bool>> predicate,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class
		{
			var query = GetAll(orderBy,includedEntities).Where(predicate);
			return query;
		}
		
		public IQueryable<TEntity> GetMany<TEntity>(
			Expression<Func<TEntity, bool>> predicate,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			ref int? totalRows, int index, int size,
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class
		{
			var query = GetAll(orderBy, includedEntities).Where(predicate);
			totalRows = totalRows ?? query.Count();
			var skipCount = index * size;
			query = query.Skip(skipCount).Take(size);
			return query;
		}


		public TEntity Get<TEntity>(
			Expression<Func<TEntity, bool>> predicate, 
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class
		{
			return GetMany(predicate, null,includedEntities).SingleOrDefault();
		}

		public TEntity Get<TEntity>(params object[] keys) where TEntity : class
		{
			var dbSet = Context.Set<TEntity>();
			return dbSet.Find(keys);
		}

		public void Insert<TEntity>(TEntity entity, bool commit = false) where TEntity : class
		{
			var dbSet = Context.Set<TEntity>();
			dbSet.Add(entity);
			if (commit)
				Context.SaveChanges();
		}

		public void Update<TEntity>(TEntity entity, bool commit = false) where TEntity : class
		{
			var dbSet = Context.Set<TEntity>();
			dbSet.Attach(entity);
			Context.Entry(entity).State = EntityState.Modified;
			if (commit)
				Context.SaveChanges();
		}

		public void Delete<TEntity>(TEntity entity, bool commit = false) where TEntity : class
		{
			var dbSet = Context.Set<TEntity>();
			if (Context.Entry(entity).State == EntityState.Detached)
			{
				dbSet.Attach(entity);
			}
			dbSet.Remove(entity);
			if (commit)
				Context.SaveChanges();
		}
	}
}
