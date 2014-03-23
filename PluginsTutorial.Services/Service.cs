using System.Collections.Generic;
using System.Linq;
using PluginsTutorial.Data;
using PluginsTutorial.Data.Models;

namespace PluginsTutorial.Services
{
	public class Service : IService
	{
		private readonly IGenericRepository _genericRepository;
		private readonly IUnitOfWork _unitOfWork;

		public Service(IGenericRepository genericRepository, IUnitOfWork unitOfWork)
		{
			_genericRepository = genericRepository;
			_unitOfWork = unitOfWork;
		}


		#region All Products
		public IEnumerable<Product> GetAllProducts()
		{
			var products = _genericRepository.GetAll<Product>().AsEnumerable();
			return products;
		}

		public IEnumerable<Product> GetAllProductsWithCategories()
		{
			var products = _genericRepository.GetAll<Product>(x => x.Category).AsEnumerable();
			return products;
		}

		public IEnumerable<Product> GetAllProductsOrdredByProductName()
		{
			var products = _genericRepository.GetAll<Product>(p => p.OrderBy(x => x.ProductName)).AsEnumerable();
			return products;
		}
		
		public IEnumerable<Product> GetAllProductsWithCategoriesOrdredByProductName()
		{
			var products = _genericRepository.GetAll<Product>(p => p.OrderBy(x => x.ProductName), x => x.Category).AsEnumerable();
			return products;
		}
		#endregion

		
		#region Second 10 Products Ordred By Product Name
		public IEnumerable<Product> GetSecond10ProductsOrdredByProductName()
		{
			//int? totalRows = 70;
			int? totalRows = null;
			var products = _genericRepository.GetAll<Product>(p => p.OrderBy(x => x.ProductName), ref totalRows, 1, 10).AsEnumerable();
			return products;
		}

		public IEnumerable<Product> GetSecond10ProductsWithCategoriesOrdredByProductName()
		{
			int? totalRows = null;
			var products = _genericRepository.GetAll<Product>(p => p.OrderBy(x => x.ProductName), ref totalRows, 1, 10, x => x.Category).AsEnumerable();
			return products;
		}
		#endregion


		#region Get Products With Condition
		public IEnumerable<Product> GetProductsWithCondition()
		{
			var products = _genericRepository.GetMany<Product>(x=>x.ProductName.Contains("A"));
			return products.ToList();
		}

		public IEnumerable<Product> GetProductsWithConditionWithCategories()
		{
			var products = _genericRepository.GetMany<Product>(x => x.ProductName.Contains("A"), x => x.Category);
			return products.ToList();
		}

		public IEnumerable<Product> GetProductsWithConditionOrdredByProductName()
		{
			var products = _genericRepository.GetMany<Product>(x => x.ProductName.Contains("A"), p => p.OrderBy(x => x.ProductName));
			return products.ToList();
		}

		public IEnumerable<Product> GetProductsWithConditionWithCategoriesOrdredByProductName()
		{
			var products = _genericRepository.GetMany<Product>(x => x.ProductName.Contains("A"), p => p.OrderBy(x => x.ProductName), x => x.Category);
			return products.ToList();
		}
		#endregion


		#region Get Second 10 Products With Condition Ordred By Product Name
		public IEnumerable<Product> GetSecond10ProductsWithConditionOrdredByProductName()
		{
			int? totalRows = null;
			var products = _genericRepository.GetMany<Product>(x => x.ProductName.Contains("A"), p => p.OrderBy(x => x.ProductName), ref totalRows, 1, 10).AsEnumerable();
			return products;
		}

		public IEnumerable<Product> GetSecond10ProductsWithConditionWithCategoriesOrdredByProductName()
		{
			int? totalRows = null;
			var products = _genericRepository.GetMany<Product>(x => x.ProductName.Contains("A"), p => p.OrderBy(x => x.ProductName), ref totalRows, 1, 10, x => x.Category).AsEnumerable();
			return products;
		}
		#endregion
		

		#region Get By Key
		public Product GetProductUsingPredicate()
		{
			var product = _genericRepository.Get<Product>(x => x.ProductId == 1, x => x.Category);
			return product;
		}

		public Product GetProductUsingFind()
		{
			var product = _genericRepository.Get<Product>(1);
			return product;
		}

		public OrderDetail GetOrderDetailUsingFind()
		{
			var product = _genericRepository.Get<OrderDetail>(10248, 11);
			return product;
		}
		#endregion


		#region Insert
		public int Insert(Product product)
		{
			_genericRepository.Insert(product);
			_unitOfWork.Commit();
			return product.ProductId;
		}
		
		public int InsertAndCommit(Product product)
		{
			_genericRepository.Insert(product, true);
			return product.ProductId;
		}
		#endregion


		#region Update
		public void Update(Product product)
		{
			_genericRepository.Update(product);
			_unitOfWork.Commit();
		}

		public void UpdateAndCommit(Product product)
		{
			_genericRepository.Update(product, true);
		}
		#endregion


		#region Delete
		public void Delete(Product product)
		{
			_genericRepository.Delete(product);
			_unitOfWork.Commit();
		}

		public void DeleteAndCommit(Product product)
		{
			_genericRepository.Delete(product, true);
		}

		public void DeleteManyProducts(List<Product> products)
		{
			foreach (var product in products)
			{
				_genericRepository.Delete(product);
			}
			_unitOfWork.Commit();
		}
		#endregion
	}
}
