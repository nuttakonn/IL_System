using System;
using System.Collections.Specialized;
using System.Web;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;
using System.IO;
using System.Globalization;
using EB_Service.Commons;

namespace Elsinore.Website
{
	public class GradientHandler : IHttpHandler
	{
		private const string ORIENTATION_PARAM = "Orientation";
		private const string LENGTH_PARAM = "Length";
		private const string START_COLOR_PARAM = "StartColor";
		private const string FINISH_COLOR_PARAM = "FinishColor";

		#region Static Implementation

		private static string handlerPath;
		private static NameValueCollection values;

		static GradientHandler()
		{
			handlerPath = Utility.GetHandlerPath(typeof(GradientHandler));
			values = new NameValueCollection();
		}

		public static string GetUrl(Orientation orientation, int length, Color startColor, Color finishColor)
		{
			lock (values)
			{
				values[ORIENTATION_PARAM] = orientation.ToString();
				values[LENGTH_PARAM] = length.ToString();
				values[START_COLOR_PARAM] = startColor.ToArgb().ToString("X8").Substring(2);
				values[FINISH_COLOR_PARAM] = finishColor.ToArgb().ToString("X8").Substring(2);

				return Utility.GetUrl(handlerPath, values);
			}
		}

		#endregion

		public bool IsReusable
		{
			get { return true; }
		}

		public void ProcessRequest(HttpContext context)
		{
			// cache forever
			context.Response.Cache.SetCacheability(HttpCacheability.Private);
			context.Response.Cache.SetExpires(DateTime.Now.AddYears(100));

			string orientationString = context.Request.QueryString[ORIENTATION_PARAM];
			string lengthString = context.Request.QueryString[LENGTH_PARAM];
			string startColorString = context.Request.QueryString[START_COLOR_PARAM];
			string finishColorString = context.Request.QueryString[FINISH_COLOR_PARAM];

			// parse query string parameters
			Orientation orientation = (string.IsNullOrEmpty(orientationString) ? Orientation.Horizontal : (Orientation)Enum.Parse(typeof(Orientation), orientationString));
			int length = (string.IsNullOrEmpty(lengthString) ? 100 : int.Parse(lengthString));
			Color startColor = (string.IsNullOrEmpty(startColorString) ? Color.Black : this.GetColorFromHexString(startColorString));
			Color finishColor = (string.IsNullOrEmpty(finishColorString) ? Color.White : this.GetColorFromHexString(finishColorString));

			// calculate geometry based on orientation and length
			int width = (orientation == Orientation.Horizontal ? length : 1);
			int height = (orientation == Orientation.Horizontal ? 1 : length);
			Point endPoint = new Point(orientation == Orientation.Horizontal ? length : 0, orientation == Orientation.Horizontal ? 0 : length);
			Rectangle rectangle = new Rectangle(Point.Empty, new Size(width, height));

			using (Bitmap bitmap = new Bitmap(width, height))
			using (Graphics graphics = Graphics.FromImage(bitmap))
			using (Brush brush = new LinearGradientBrush(Point.Empty, endPoint, startColor, finishColor))
			using (MemoryStream memoryStream = new MemoryStream())
			{
				graphics.FillRectangle(brush, rectangle);

				// memory stream required because of bug in GDI+ when saving PNG directly to Response.OutputStream
				bitmap.Save(memoryStream, ImageFormat.Png);

				context.Response.ContentType = "image/png";
				memoryStream.WriteTo(context.Response.OutputStream);
			}
		}

		private Color GetColorFromHexString(string colorString)
		{
			int colorInt = int.Parse(colorString, NumberStyles.HexNumber);
			Color baseColor = Color.FromArgb(colorInt);
			return Color.FromArgb(byte.MaxValue, baseColor);
		}
	}
}