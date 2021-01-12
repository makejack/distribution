using MediatR;

namespace Mytime.Distribution.Events
{
    /// <summary>
    /// 合伙人关系
    /// </summary>
    public class CustomerRelationEvent : INotification
    {
        /// <summary>
        /// 上级
        /// </summary>
        /// <value></value>
        public int ParentId { get; set; }
        /// <summary>
        /// 子级
        /// </summary>
        /// <value></value>
        public int ChildrenId { get; set; }
    }
}