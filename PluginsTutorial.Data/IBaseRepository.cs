using System;
using System.Linq;
using System.Linq.Expressions;

namespace PluginsTutorial.Data
{
	public interface IBaseRepository<TEntity> where TEntity : class
	{
		IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includedEntities);

		IQueryable<TEntity> GetAll(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, 
			params Expression<Func<TEntity, object>>[] includedEntities);

		IQueryable<TEntity> GetAll(
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			ref int? totalRows, int index, int size,
			params Expression<Func<TEntity, object>>[] includedEntities);


		IQueryable<TEntity> GetMany(
			Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object>>[] includedEntities);

		IQueryable<TEntity> GetMany(
			Expression<Func<TEntity, bool>> predicate,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			params Expression<Func<TEntity, object>>[] includedEntities);

		IQueryable<TEntity> GetMany(
			Expression<Func<TEntity, bool>> predicate,
			Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
			ref int? totalRows, int index, int size,
			params Expression<Func<TEntity, object>>[] includedEntities);


		TEntity Get(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includedEntities);

		TEntity Get(params object[] keys);

		void Insert(TEntity entity, bool commit = false);

		void Update(TEntity entity, bool commit = false);

		void Delete(TEntity entity, bool commit = false);
	}
}
