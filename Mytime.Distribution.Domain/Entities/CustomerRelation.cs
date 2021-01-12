using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class CustomerRelation : AggregateRoot
    {
        public CustomerRelation()
        {
        }

        public CustomerRelation(int parentId, int childrenId, int level)
        {
            ParentId = parentId;
            ChildrenId = childrenId;
            Level = level;
            Createat = DateTime.Now;
        }

        public int ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Customer Parent { get; set; }
        public int ChildrenId { get; set; }
        [ForeignKey("ChildrenId")]
        public virtual Customer Children { get; set; }
        public int Level { get; set; }
        public DateTime Createat { get; set; }
    }
}