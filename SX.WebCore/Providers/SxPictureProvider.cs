using System;
using System.Drawing;
using System.IO;

namespace SX.WebCore.Providers
{
    public static class SxPictureProvider
    {
        public static byte[] ScaleImage(byte[] inputByteArray, ImageScaleMode scaleMode = ImageScaleMode.Width, int? destWidth = null, int? destHeight = null)
        {
            var originalImage = imageFromByteArray(ref inputByteArray);

            switch (scaleMode)
            {
                case ImageScaleMode.Width:
                    if (!destWidth.HasValue)
                        throw new ArgumentNullException("Не задано значение ширины изображения");
                    destHeight = (int)(originalImage.Height * (decimal)destWidth / originalImage.Width);
                    break;
                case ImageScaleMode.Height:
                    if (!destHeight.HasValue)
                        throw new ArgumentNullException("Не задано значение высоты изображения");
                    destWidth = (int)(originalImage.Width * (decimal)destHeight / originalImage.Height);
                    break;
                default:
                    throw new NotImplementedException("Такое приведение недопустимо");
            }

            var bitmap = new Bitmap(originalImage, (int)destWidth, (int)destHeight);
            byte[] resultByteArray = null;
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, originalImage.RawFormat);
                resultByteArray = stream.ToArray();
            }
            return resultByteArray;
        }

        private static Image imageFromByteArray(ref byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        public enum ImageScaleMode : byte
        {
            Unknown = 0,
            Width = 1,
            Height = 2
        }
    }
}
