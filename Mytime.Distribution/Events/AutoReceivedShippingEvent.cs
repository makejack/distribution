using MediatR;

namespace Mytime.Distribution.Events
{
    /// <summary>
    /// 自动收货
    /// </summary>
    public class AutoReceivedShippingEvent : INotification
    {
        /// <summary>
        /// ShipmentId
        /// </summary>
        /// <value></value>
        public int ShipmentId { get; set; }
    }
}