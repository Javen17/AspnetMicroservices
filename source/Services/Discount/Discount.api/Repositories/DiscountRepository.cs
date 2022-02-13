using Discount.api.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;
using System;
using Dapper;

namespace Discount.api.Repositories
{
	public class DiscountRepository : IDiscountRepository
	{
		private IConfiguration _configuration;
		public DiscountRepository(IConfiguration configuration)
		{
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task<bool> CreateDiscount(Coupon coupon)
		{
			using (var connection = BuildDbConnection())
			{
				var affected = await connection.ExecuteAsync
					("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
							new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
				
				if (affected == 0)
				{
					return false;
				}

				return true;
			}
		}

		public async Task<bool> DeleteDiscount(string productName)
		{
			using (var connection = BuildDbConnection())
			{
				var affected = await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

				if (affected == 0)
				{
					return false;
				}

				return true;
			}
		}

		public async Task<Coupon> GetDiscount(string productName)
		{
			using (var connection = BuildDbConnection())
			{
				var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM COUPON WHERE ProductName = @ProductName", new {ProductName =  productName});

				if(coupon == null)
				{
					return new Coupon { ProductName = "No Discount", Description = "No Discount Description" };
				}

				return coupon;
			}
		}

		public async Task<bool> UpdateDiscount(Coupon coupon)
		{
			using (var connection = BuildDbConnection())
			{
				var affected = await connection.ExecuteAsync
				   ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
						   new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

				if (affected == 0)
				{
					return false;
				}

				return true;
			}
		}

		private NpgsqlConnection BuildDbConnection()
		{
			return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
		}
	}
}
