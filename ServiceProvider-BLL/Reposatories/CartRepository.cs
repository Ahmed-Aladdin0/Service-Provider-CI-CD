﻿using Mapster;
using Microsoft.EntityFrameworkCore;
using SeeviceProvider_BLL.Abstractions;
using ServiceProvider_BLL.Dtos.CartProductDto;
using ServiceProvider_BLL.Dtos.ProductDto;
using ServiceProvider_BLL.Errors;
using ServiceProvider_BLL.Interfaces;
using ServiceProvider_DAL.Data;
using ServiceProvider_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider_BLL.Reposatories
{
    public class CartRepository : BaseRepository<Cart> , ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Result<CartResponse>> GetCart(int cartId, CancellationToken cancellationToken = default)
        {
            var cart = await _context.Carts!
              .Include(c => c.CartProducts)
              .ThenInclude(cp => cp.Product)
              .FirstOrDefaultAsync(c => c.Id == cartId, cancellationToken);

            if (cart == null)
                return Result.Failure<CartResponse>(CartErrors.CartNotFound);

            var response = new CartResponse(
                Id: cart.Id,
                Items: cart.CartProducts.Select(cp => new CartItemResponse(
                         ProductId: cp.ProductId,
                         NameEn: cp.Product.NameEn,
                         NameAr: cp.Product.NameAr,
                         Price: cp.Product.Price,
                         Quantity: cp.Quantity
                )).ToList()
                );

            return Result.Success(response);  
        
        }
        public async Task<Result<CartProductResponse>> AddToCartAsync(CartProductRequest request , CancellationToken cancellationToken )
        {
            var cart = await _context.Carts!
                .FirstOrDefaultAsync(c => c.ApplicationUserId == request.UserId, cancellationToken: cancellationToken);

            if (cart == null)
            {
                cart = new Cart { ApplicationUserId = request.UserId };
                _context.Carts!.Add(cart);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var productExists = await _context.Products!.AnyAsync(p => p.Id == request.ProductId, cancellationToken: cancellationToken);
            if (!productExists)
                return Result.Failure<CartProductResponse>(ProductErrors.ProductNotFound);

            var cartProduct = new CartProduct
            {
                CartId = cart.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

           await _context.CartProducts!.AddAsync(cartProduct , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(cartProduct.Adapt<CartProductResponse>());
        }


        public async Task<Result<CartProductResponse>> UpdateCartItemAsync(UpdateCartItemRequest request , CancellationToken cancellationToken)
        {
            var cartProduct = await _context.CartProducts!
                            .FirstOrDefaultAsync(cp =>
                                cp.CartId == request.CartId &&
                                cp.ProductId == request.ProductId,
                                cancellationToken: cancellationToken
                            );

            if (cartProduct == null)
                return Result.Failure<CartProductResponse>(CartProductErrors.NotFound);

            cartProduct.Quantity = request.Quantity;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(cartProduct.Adapt<CartProductResponse>());
        }
    }
}
