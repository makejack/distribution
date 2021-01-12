using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class Goods : AggregateRoot
    {
        public Goods()
        {
        }

        [Required]
        [MaxLength(512)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string NormalizedName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int? ThumbnailImageId { get; set; }
        [ForeignKey("ThumbnailImageId")]
        public virtual Media ThumbnailImage { get; set; }
        public bool IsPublished { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Goods Parent { get; set; }
        public virtual List<Goods> Childrens { get; set; }
        /// <summary>
        /// 子商品没有选项
        /// </summary>
        /// <value></value>
        public bool HasOptions { get; set; }
        /// <summary>
        /// 子商品不显示列表
        /// </summary>
        /// <value></value>
        public bool IsVisible { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        /// <value></value>
        public int StockQuantity { get; set; }
        public DateTime? PublishedOn { get; set; }
        /// <summary>
        /// 城市合伙人折扣
        /// </summary>
        /// <value></value>
        public int CityDiscount { get; set; }
        /// <summary>
        /// 网点合伙人折扣
        /// </summary>
        public int BranchDiscount { get; set; }
        /// <summary>
        /// 显示排序
        /// </summary>
        /// <value></value>
        public int DisplayOrder { get; set; }
        public DateTime Createat { get; set; }

        public virtual List<GoodsMedia> GoodsMedias { get; set; }
        public virtual List<GoodsOptionValue> OptionValues { get; set; }
        public virtual List<GoodsOptionCombination> OptionCombinations { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}