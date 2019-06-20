﻿using BookStore.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Model
{
    class NameQuantity : INameQuantity
    {
        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}
