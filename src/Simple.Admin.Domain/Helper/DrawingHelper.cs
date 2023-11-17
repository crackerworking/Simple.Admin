using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Simple.Admin.Domain.Helper
{
    public class DrawingHelper
    {
        /// <summary>
        /// 图片验证码
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns></returns>
        public static byte[] CreateByteByImgVerifyCode(string verifyCode, int width, int height)
        {
            using Image image = new Image<Rgba32>(width, height);
            //漆底色白色
            image.Mutate(x => x.DrawLine(Pens.DashDot(Color.White, width), new PointF[] { new PointF() { X = 0, Y = 0 }, new PointF() { X = width, Y = height } }));

            FontCollection collection = new();
            FontFamily family = collection.Add("font/RedwoodMount-p7Jrd.ttf");
            Font font = family.CreateFont(20, FontStyle.Bold);

            PointF startPointF = new PointF(5, 5);
            Random random = new Random(); //随机数产生器

            Color[] colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Purple, Color.Peru, Color.LightSeaGreen, Color.Lime, Color.Magenta, Color.Maroon, Color.MediumBlue, Color.MidnightBlue, Color.Navy };
            //绘制大小
            for (int i = 0; i < verifyCode.Length; i++)
            {
                image.Mutate(x => x.DrawText(verifyCode[i].ToString(), font, colors[random.Next(colors.Length)], startPointF));
                //Console.WriteLine($"draw code:{verifyCode[i]} point:{startPointF.X}-{startPointF.Y}");
                startPointF.X += (width - 10) / verifyCode.Length;
                startPointF.Y = random.Next(5, 10);
            }

            Pen pen = Pens.DashDot(Color.Silver, 1);

            //绘制干扰线
            for (var k = 0; k < 40; k++)
            {
                PointF[] points = new PointF[2];
                points[0] = new PointF(random.Next(width), random.Next(height));
                points[1] = new PointF(random.Next(width), random.Next(height));
                image.Mutate(x => x.DrawLine(pen, points));
            }

            using MemoryStream stream = new MemoryStream();
            image.Save(stream, JpegFormat.Instance);
            //输出图片流
            return stream.ToArray();
        }
    }
}