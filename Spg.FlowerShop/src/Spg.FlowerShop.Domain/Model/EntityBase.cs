using Spg.FlowerShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Model
{
    public class EntityBase
    {
        public int Id { get; private set; } // pk
        public DateTime? LastChangeDate { get; set; } 
    }
}
