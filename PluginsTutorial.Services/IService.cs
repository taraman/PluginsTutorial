using System.Collections.Generic;
using PluginsTutorial.Data.Models;

namespace PluginsTutorial.Services
{
	public interface IService
	{
		IEnumerable<Product> GetAllProducts();
		IEnumerable<Product> GetAllProductsWithCategories();
		IEnumerable<Product> GetAllProductsOrdredByProductName();
		IEnumerable<Product> GetAllProductsWithCategoriesOrdredByProductName();

		IEnumerable<Product> GetSecond10ProductsOrdredByProductName();
		IEnumerable<Product> GetSecond10ProductsWithCategoriesOrdredByProductName();

		IEnumerable<Product> GetProductsWithCondition();
		IEnumerable<Product> GetProductsWithConditionWithCategories();
		IEnumerable<Product> GetProductsWithConditionOrdredByProductName();
		IEnumerable<Product> GetProductsWithConditionWithCategoriesOrdredByProductName();

		IEnumerable<Product> GetSecond10ProductsWithConditionOrdredByProductName();
		IEnumerable<Product> GetSecond10ProductsWithConditionWithCategoriesOrdredByProductName();

		Product GetProductUsingPredicate();
		Product GetProductUsingFind();
		OrderDetail GetOrderDetailUsingFind();

		int Insert(Product product);
		int InsertAndCommit(Product product);

		void Update(Product product);
		void UpdateAndCommit(Product product);

		void Delete(Product product);
		void DeleteAndCommit(Product product);
		void DeleteManyProducts(List<Product> products);
	}
}
