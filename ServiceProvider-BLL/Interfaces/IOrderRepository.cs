﻿using SeeviceProvider_BLL.Abstractions;
using ServiceProvider_BLL.Dtos.OrderDto;
using ServiceProvider_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider_BLL.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Result<OrderResponseV2>> GetOrderAsync(int orderId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<OrderResponseV2>>> GetUserOrdersAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<OrdersOfVendorResponse>>> GetVendorsOrders(string vendorId, CancellationToken cancellationToken = default);
        Task<Result<OrderResponseV2>> AddOrderAsync( OrderRequest request, CancellationToken cancellationToken = default);
        Task<Result<OrderResponseV2>> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusRequest request, CancellationToken cancellationToken = default);
        Task<Result<OrderResponse>> CheckoutAsync(CheckoutRequest request , CancellationToken cancellationToken);


        
    }
}
