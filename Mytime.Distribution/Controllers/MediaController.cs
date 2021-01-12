using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mytime.Distribution.Domain.Entities;
using Mytime.Distribution.Domain.IRepositories;
using Mytime.Distribution.Models;
using Mytime.Distribution.Utils.Helpers;
using Mytime.Distribution.Models.V1.Response;
using Mytime.Distribution.Extensions;

namespace Mytime.Distribution.Controllers
{
    /// <summary>
    /// 媒体
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/media")]
    public class MediaController : ControllerBase
    {
        private const string PRODUCT_IMAGE_PATH = "upload/images";

        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _accessor;
        private readonly IRepositoryByInt<Media> _mediaRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="accessor"></param>
        /// <param name="mediaRepository"></param>
        /// <param name="mapper"></param>
        public MediaController(IWebHostEnvironment environment,
                               IHttpContextAccessor accessor,
                               IRepositoryByInt<Media> mediaRepository,
                               IMapper mapper)
        {
            _environment = environment;
            _accessor = accessor;
            _mediaRepository = mediaRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Upload(IFormFile file)
        {
            // string imgUrl = string.Empty;
            if (file == null) return Result.Fail(ResultCodes.RequestParamError);

            DateTime now = DateTime.Now;
            string extension = ".jpg";//Path.GetExtension(file.FileName);
            string fileName = Guid.NewGuid().ToString("N") + extension;
            string imageDir = Path.Combine(PRODUCT_IMAGE_PATH, now.ToString("yyyyMMdd"), fileName);
            string imageFileName = Path.Combine(_environment.WebRootPath, imageDir);
            FileHelper.CreateImgFolder(imageFileName);

            using (Stream mStream = new FileStream(imageFileName, FileMode.Create))
            {
                await file.CopyToAsync(mStream);

                //Bitmap srcBitmap = new Bitmap(stream);
                //ImageHelper.Compress(srcBitmap, Path.Combine(imageDir, fileName), 50);
            }
            SixLaborsImageSharpHelper.Compress(imageFileName, imageFileName, 50);

            var media = new Media(imageDir, file.ContentType, file.Length);
            await _mediaRepository.InsertAsync(media);

            // 返回完整地址
            media.Url = _accessor.HttpContext.Request.GetHostUrl() + "/" + media.Url;

            return Result.Ok(_mapper.Map<MediaResponse>(media));
        }
    }
}