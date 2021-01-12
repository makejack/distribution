namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 后台团队销量
    /// </summary>
    public class AdminHomeTeamSalesResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }
        /// <summary>
        /// 团队数量
        /// </summary>
        /// <value></value>
        public int TeamQuantity { get; set; }
        /// <summary>
        /// 团队金额
        /// </summary>
        /// <value></value>
        public int TeamAmount { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        /// <value></value>
        public string NickName { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        /// <value></value>
        public string AvatarUrl { get; set; }
    }
}