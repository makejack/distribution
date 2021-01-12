using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class ShipmentOrderItem
    {
        public ShipmentOrderItem()
        {
        }

        public ShipmentOrderItem(int orderItemId)
        {
            OrderItemId = orderItemId;
        }

        public int ShipmentId { get; set; }
        [ForeignKey("ShipmentId")]
        public virtual Shipment Shipment { get; set; }
        public int OrderItemId { get; set; }
        [ForeignKey("OrderItemId")]
        public virtual OrderItem OrderItem { get; set; }
    }
}