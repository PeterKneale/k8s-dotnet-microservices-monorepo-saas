using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using BuildingBlocks.Domain.DDD.Rules;
using BuildingBlocks.Domain.DDD.ValueTypes;

namespace Carts.Domain
{
    public class Cart
    {
        private readonly IList<Product> _products = new List<Product>();

        private Cart(string cartId, Currency currency)
        {
            Guard.Against.NullOrWhiteSpace(cartId, nameof(cartId));
            CartId = cartId;
            Currency = currency;
        }

        private Cart()
        {
            _products = new List<Product>();
        }

        public static Cart CreateInstance(string cartId, Currency currency)
        {
            return new Cart(cartId, currency);
        }

        public string CartId { get; private set; }

        public Money TotalPrice => Money.CreateInstance(Products.Sum(x => x.UnitPrice.Amount * x.Quantity), Currency);

        public Currency Currency { get; private set; }

        public IEnumerable<Product> Products => _products;

        public void AddProduct(string productId, string description, int quantity, Money unitPrice)
        {
            if (!Equals(unitPrice.Currency, Currency))
            {
                throw new BusinessRuleBrokenException("The product's unit price is a different currency to the carts");
            }
            var item = _products.SingleOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                item.AddQuantity(quantity);
                if (item.UnitPrice != unitPrice)
                {
                    item.UpdateUnitPrice(unitPrice);
                }
                if (item.Description != description)
                {
                    item.UpdateDescription(description);
                }
            }
            else
            {
                _products.Add(Product.CreateInstance(productId, description, quantity, unitPrice));
            }
        }

        public bool ContainsProduct(string productId) => _products.Any(x => x.ProductId == productId);

        public void RemoveProduct(string productId)
        {
            var item = _products.SingleOrDefault(x => x.ProductId == productId);
            if (item == null)
            {
                throw new Exception("Item does not exist");
            }
            _products.Remove(item);
        }

        public void Empty()
        {
            _products.Clear();
        }
    }
}