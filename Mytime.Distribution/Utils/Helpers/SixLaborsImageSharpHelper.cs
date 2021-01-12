using System.IO;
using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace Mytime.Distribution.Utils.Helpers
{
    /// <summary>
    /// Sixlabor 图片帮助
    /// </summary>
    public static class SixLaborsImageSharpHelper
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="newfile"></param>
        /// <param name="maxHeight"></param>
        /// <param name="maxWidth"></param>
        /// <param name="level"></param>
        public static void GenerateSmallImage(string filename, string newfile, int maxHeight, int maxWidth, int level = 50)
        {
            //使用SixLabors.ImageSharp
            using (Image img = Image.Load(filename))
            {
                Size newSize = NewSize(maxWidth, maxHeight, img.Width, img.Height);
                img.Mutate(x => x.Resize(newSize));
                IImageEncoder imageEncoder = GetImageFomat(System.IO.Path.GetExtension(newfile), level);
                img.Save(newfile, imageEncoder);
            }
        }


        //调整图片大小
        private static Size NewSize(int maxWidth, int maxHeight, int Width, int Height)
        {
            double w = 0.0;
            double h = 0.0;
            double sw = Convert.ToDouble(Width);
            double sh = Convert.ToDouble(Height);
            double mw = Convert.ToDouble(maxWidth);
            double mh = Convert.ToDouble(maxHeight);

            if (sw < mw && sh < mh)//如果maxWidth和maxHeight大于源图像，则缩略图的长和高不变
            {
                w = sw;
                h = sh;
            }
            else if ((sw / sh) > (mw / mh))
            {
                w = maxWidth;
                h = (w * sh) / sw;
            }
            else
            {
                h = maxHeight;
                w = (h * sw) / sh;
            }
            return new Size(Convert.ToInt32(w), Convert.ToInt32(h));
        }

        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="targetFile">传入的Bitmap对象</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(string targetFile, string destFile, int level)
        {
            using (Image img = Image.Load(targetFile))
            {
                IImageEncoder imageEncoder = GetImageFomat(System.IO.Path.GetExtension(destFile), level);
                img.Save(destFile, imageEncoder);
            }
        }
        private static IImageEncoder GetImageFomat(string extention, int level)
        {
            if (extention == ".jpg" || extention == ".jpeg")
            {
                return new JpegEncoder()
                {
                    Quality = level
                };
            }
            else if (extention == ".png")
            {
                return new PngEncoder()
                {
                    CompressionLevel = 6
                };
            }
            return null;
        }

        /// <summary>
        /// Implements a full image mutating pipeline operating on IImageProcessingContext
        /// </summary>
        /// <param name="processingContext"></param>
        /// <param name="size"></param>
        /// <param name="cornerRadius"></param>
        /// <returns></returns>
        public static IImageProcessingContext ConvertToAvatar(this IImageProcessingContext processingContext, Size size, float cornerRadius)
        {
            return processingContext.Resize(new ResizeOptions
            {
                Size = size,
                Mode = ResizeMode.Crop
            }).ApplyRoundedCorners(cornerRadius);
        }

        // This method can be seen as an inline implementation of an `IImageProcessor`:
        // (The combination of `IImageOperations.Apply()` + this could be replaced with an `IImageProcessor`)
        private static IImageProcessingContext ApplyRoundedCorners(this IImageProcessingContext ctx, float cornerRadius)
        {
            Size size = ctx.GetCurrentSize();
            IPathCollection corners = BuildCorners(size.Width, size.Height, cornerRadius);

            var graphicOptions = new GraphicsOptions(true)
            {
                AlphaCompositionMode = PixelAlphaCompositionMode.DestOut // enforces that any part of this shape that has color is punched out of the background
            };
            // mutating in here as we already have a cloned original
            // use any color (not Transparent), so the corners will be clipped
            return ctx.Fill(graphicOptions, Rgba32.LightGreen, corners);
        }

        private static IPathCollection BuildCorners(int imageWidth, int imageHeight, float cornerRadius)
        {
            // first create a square
            var rect = new RectangularPolygon(-0.5f, -0.5f, cornerRadius, cornerRadius);

            // then cut out of the square a circle so we are left with a corner
            IPath cornerTopLeft = rect.Clip(new EllipsePolygon(cornerRadius - 0.5f, cornerRadius - 0.5f, cornerRadius));

            // corner is now a corner shape positions top left
            //lets make 3 more positioned correctly, we can do that by translating the original around the center of the image

            float rightPos = imageWidth - cornerTopLeft.Bounds.Width + 1;
            float bottomPos = imageHeight - cornerTopLeft.Bounds.Height + 1;

            // move it across the width of the image - the width of the shape
            IPath cornerTopRight = cornerTopLeft.RotateDegree(90).Translate(rightPos, 0);
            IPath cornerBottomLeft = cornerTopLeft.RotateDegree(-90).Translate(0, bottomPos);
            IPath cornerBottomRight = cornerTopLeft.RotateDegree(180).Translate(rightPos, bottomPos);

            return new PathCollection(cornerTopLeft, cornerBottomLeft, cornerTopRight, cornerBottomRight);
        }
    }
}
