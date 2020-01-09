﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public virtual void AddItem(Products product, int quantity)
        {
            CartLine line = lineCollection.Where(p => p.Product.Id == product.Id).FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Products product) => lineCollection.RemoveAll(l => l.Product.Id == product.Id);
        public virtual decimal ComputeTotalValue() => lineCollection.Sum(e => e.Product.Price * e.Quantity);
        public virtual void Clear() => lineCollection.Clear();
        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Products Product { get; set; }
        public int Quantity { get; set; }
    }
}

