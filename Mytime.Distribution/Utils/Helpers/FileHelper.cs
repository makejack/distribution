using System.IO;
using System.Runtime.ConstrainedExecution;

namespace Mytime.Distribution.Utils.Helpers
{
    /// <summary>
    /// 文件帮助
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filefullpath"></param>
        /// <returns></returns>
        public static DirectoryInfo CreateImgFolder(string filefullpath)
        {
            DirectoryInfo dir = null;
            //string dirpath = filefullpath.Substring(0, filefullpath.LastIndexOf('\\'));
            if (!File.Exists(filefullpath))
            {
#if DEBUG
                string[] pathes = filefullpath.Split('\\');
                if (pathes.Length > 1)
                {
                    string path = pathes[0];
                    for (int i = 1; i < pathes.Length - 1; i++)
                    {
                        path += "\\" + pathes[i];
                        if (!Directory.Exists(path))
                        {
                            dir = Directory.CreateDirectory(path);
                        }
                    }
                }
#else
                string[] pathes = filefullpath.Split('/');
                if (pathes.Length > 1)
                {
                    string path = pathes[0];
                    for (int i = 1; i < pathes.Length - 1; i++)
                    {
                        path += "/" + pathes[i];
                        if (!Directory.Exists(path))
                        {
                            dir = Directory.CreateDirectory(path);
                        }
                    }
                }
#endif
            }
            return dir;
        }
    }
}