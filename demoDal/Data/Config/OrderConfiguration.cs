using TalabatBLL.Entities;
using TalabatBLL.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabatDAL.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShipedToAddress, a => { a.WithOwner(); });
            // type, value
            builder.Property(o => o.Status).HasConversion(
                s => s.ToString(),
                s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s)
                ) ;
            //بيمسح اوردر يمسح معاه order items , اروح storecontext  علشان اضيف dataset

            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade) ;
        }
    }
}
