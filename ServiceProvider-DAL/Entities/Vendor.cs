﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceProvider_DAL.Entities
{
    public class Vendor : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? BusinessName { get; set; }
        public string BusinessType { get; set; }  =string.Empty;
        public string? TaxNumber { get; set; } 
        public float? Rating { get; set; }

        public bool IsApproved { get; set; }

        public virtual ICollection<Message>? Messages {  get; set; } = new List<Message>();
        public virtual ICollection<Product>? Products { get; set; } = new List<Product>();
        public virtual ICollection<VendorSubCategory>? VendorSubCategories { get; set; } = new List<VendorSubCategory>();
    }
}
