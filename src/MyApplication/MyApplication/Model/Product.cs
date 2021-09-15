using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApplication.Model
{
    public class Product
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
    }
}
