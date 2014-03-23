using System;
using System.Linq;
using System.Linq.Expressions;

namespace PluginsTutorial.Data
{
	public interface IGenericRepository
	{
		IQueryable<TEntity> GetAll<TEntity>(params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class;

		IQueryable<TEntity> GetAll<TEntity>(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class;

		IQueryable<TEntity> GetAll<TEntity>(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			ref int? totalRows, int index, int size,
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class;


		IQueryable<TEntity> GetMany<TEntity>(
			Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class;

		IQueryable<TEntity> GetMany<TEntity>(
			Expression<Func<TEntity, bool>> predicate,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class;

		IQueryable<TEntity> GetMany<TEntity>(
			Expression<Func<TEntity, bool>> predicate,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			ref int? totalRows, int index, int size,
			params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class;

		
		TEntity Get<TEntity>(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includedEntities) where TEntity : class;

		TEntity Get<TEntity>(params object[] keys) where TEntity : class;
		
		void Insert<TEntity>(TEntity entity, bool commit = false) where TEntity : class;

		void Update<TEntity>(TEntity entity, bool commit = false) where TEntity : class;
		
		void Delete<TEntity>(TEntity entity, bool commit = false) where TEntity : class;
	}
}
