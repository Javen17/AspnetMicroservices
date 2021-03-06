using Discount.api.Entities;
using Discount.api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Discount.api.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class DiscountController : ControllerBase
	{
		private readonly IDiscountRepository _repository;

		public DiscountController(IDiscountRepository repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		[HttpGet("{productName}", Name = "GetDiscount")]
		[ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<bool>> GetDiscount(string productName)
		{
			var discount = await _repository.GetDiscount(productName);
			return Ok(discount);
		}

		[HttpPost]
		[ProducesResponseType(typeof(ActionResult<Coupon>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> CreateDiscount(Coupon coupon)
		{
			await _repository.CreateDiscount(coupon);
			return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
		}

		[HttpPut]
		[ProducesResponseType(typeof(ActionResult<Coupon>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> UpdateDiscount(Coupon coupon)
		{
			return Ok(await _repository.UpdateDiscount(coupon));
		}

		[HttpDelete("{productName}", Name = "GetDiscount")]
		[ProducesResponseType(typeof(ActionResult<Coupon>), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<Coupon>> DeleteDiscount(string productName)
		{
			return Ok(await _repository.DeleteDiscount(productName));
		}

	}
}
