namespace Mytime.Distribution.Models
{
    /// <summary>
    /// 分页请求
    /// </summary>
    public class PaginationRequest
    {
        private int _page;
        /// <summary>
        /// 页
        /// </summary>
        /// <value></value>
        public int Page
        {
            get
            {
                if (_page <= 0)
                    return 1;
                return _page;
            }
            set { _page = value; }
        }

        private int _limit;
        /// <summary>
        /// 行
        /// </summary>
        /// <value></value>
        public int Limit
        {
            get
            {
                if (_limit <= 0)
                    return 10;
                return _limit;
            }
            set { _limit = value; }
        }

    }
}