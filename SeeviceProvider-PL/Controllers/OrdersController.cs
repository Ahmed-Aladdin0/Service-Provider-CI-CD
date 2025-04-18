﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceProvider_BLL.Abstractions;
using ServiceProvider_BLL.Dtos.OrderDto;
using ServiceProvider_BLL.Interfaces;
using System.Security.Claims;

namespace SeeviceProvider_PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IUnitOfWork orderRepositry) : ControllerBase
    {
        private readonly IUnitOfWork _orderRepositry = orderRepositry;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder([FromRoute] int id , CancellationToken cancellationToken)
        {
            var result = await _orderRepositry.Orders.GetOrderAsync(id , cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId, CancellationToken cancellationToken)
        {
            // Authorization check
            //var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (currentUserId != userId && !User.IsInRole("Admin"))
            //    return Forbid();

            var result = await _orderRepositry.Orders.GetUserOrdersAsync(userId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("vendors/{vendorId}")]
        [Authorize(Policy = "AdminOrApprovedVendor")]
        public async Task<IActionResult> GetVendorOrders(string vendorId, CancellationToken cancellationToken)
        {
            // Authorization check
            //var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (currentUserId != userId && !User.IsInRole("Admin"))
            //    return Forbid();

            var result = await _orderRepositry.Orders.GetVendorsOrders(vendorId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddOrder ( [FromBody] OrderRequest request, CancellationToken cancellationToken)
        {
            var result = await _orderRepositry.Orders.AddOrderAsync(request, cancellationToken);

            return result.IsSuccess
                ?  CreatedAtAction(nameof(GetOrder), new { id = result.Value.Id }, result.Value)
                : result.ToProblem();
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "AdminOrApprovedVendor")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int id, [FromBody] UpdateOrderStatusRequest request , CancellationToken cancellationToken)
        {
            var result = await _orderRepositry.Orders.UpdateOrderStatusAsync(id, request , cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

     

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request , CancellationToken cancellationToken)
        {
            var result = await _orderRepositry.Orders.CheckoutAsync(request , cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : result.ToProblem();
        }

    }
}
