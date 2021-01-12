using Mytime.Distribution.Extensions;

namespace Mytime.Distribution.Models
{
    /// <summary>
    /// 结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result(T data) : base()
        {
            this.Data = data;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Result(ResultCodes code, T data) : base(code)
        {
            this.Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        /// <value></value>
        public T Data { get; set; }

    }

    /// <summary>
    /// 结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Result()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public Result(ResultCodes code)
        {
            this.Code = code;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public Result(ResultCodes code, string msg)
        {
            this.Code = code;
            this.Msg = msg;
        }

        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>默认 0</value>
        public ResultCodes Code { get; set; } = ResultCodes.Ok;

        private string _msg = string.Empty;
        /// <summary>
        /// 消息
        /// </summary>
        /// <value></value>
        public string Msg
        {
            get
            {
                if (string.IsNullOrEmpty(_msg))
                {
                    return Code.GetDescription();
                }
                return _msg;
            }
            set
            {
                this._msg = value;
            }
        }

#if DEBUG

        /// <summary>
        /// 程序异常消息
        /// </summary>
        /// <value></value>
        public string ErrorMsg { get; set; }

#endif

        #region static function

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Result Fail(ResultCodes code, string msg = null)
        {
            return new Result(code, msg);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Result<T> Fail<T>(ResultCodes code, T data)
        {
            return new Result<T>(code, data);
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        public static Result Ok()
        {
            return new Result();
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Result<T> Ok<T>(T data)
        {
            return new Result<T>(data);
        }

        #endregion

    }
}