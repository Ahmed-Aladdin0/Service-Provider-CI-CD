﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceProvider_BLL.Abstractions;
using ServiceProvider_BLL.Dtos.CartProductDto;
using ServiceProvider_BLL.Interfaces;
using ServiceProvider_DAL.Entities;

namespace SeeviceProvider_PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController(IUnitOfWork CartRepositry) : ControllerBase
    {
        private readonly IUnitOfWork _cartRepositry = CartRepositry;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartDetalis([FromRoute] int id , CancellationToken cancellationToken)
        {
            var result = await _cartRepositry.Carts.GetCart(id , cancellationToken);

            return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddToCart([FromBody] CartProductRequest request , CancellationToken cancellationToken)
        {
            var result = await _cartRepositry.Carts.AddToCartAsync(request ,cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemRequest request , CancellationToken cancellationToken)
        {
            var result = await _cartRepositry.Carts.UpdateCartItemAsync(request , cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }
    }
}
