using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mytime.Distribution.Domain.Entities
{
    public class GoodsMedia
    {
        public GoodsMedia()
        {
        }

        public GoodsMedia(int mediaId)
        {
            MediaId = mediaId;
        }
        public int GoodsId { get; set; }
        [ForeignKey("GoodsId")]
        public virtual Goods Goods { get; set; }
        public int MediaId { get; set; }
        [ForeignKey("MediaId")]
        public virtual Media Media { get; set; }
    }
}