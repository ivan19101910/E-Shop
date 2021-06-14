﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApp.Models
{
    public class OrderProduct
    {
        public int ProductId { get; set; }

        public int OrderId { get; set; }

        public int Amount { get; set; }

        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }
    }
}
