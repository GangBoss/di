﻿using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.Results;

namespace TagsCloudVisualization.ImageSaver
{
    internal class JpgSaver : IImageSaver

    {
        public Result<None> Save(Bitmap bitmap, string pathToSave)
        {
            return Result.OfAction(() =>
            {
                var jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                var myEncoder = Encoder.Quality;
                var myEncoderParameter = new EncoderParameter(myEncoder, 0L);
                var myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bitmap.Save(pathToSave + "/TagCloud.jpg", jpgEncoder, myEncoderParameters);
            });
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();

            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }
    }
}