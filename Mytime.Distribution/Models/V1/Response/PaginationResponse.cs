namespace Mytime.Distribution.Models.V1.Response
{
    /// <summary>
    /// 分页响应
    /// </summary>
    public class PaginationResponse
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PaginationResponse()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="totalRows"></param>
        /// <param name="result"></param>
        public PaginationResponse(int currentPage, int totalRows, object result)
        {
            CurrentPage = currentPage;
            TotalRows = totalRows;
            Result = result;
        }

        /// <summary>
        /// 当前页
        /// </summary>
        /// <value></value>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 总行
        /// </summary>
        /// <value></value>
        public int TotalRows { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        /// <value></value>
        public object Result { get; set; }
    }
}