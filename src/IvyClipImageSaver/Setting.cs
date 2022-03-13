using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invary.IvyClipImageSaver
{
	internal class Setting
	{
		[XmlIgnore]
		public static int nVersion { get; } = 100;

		[XmlIgnore]
		public static string strVersion { get; } = $"Ver{nVersion}";


		[XmlIgnore]
		public static string strProductGuid { get; } = "{C1614061-D3AE-4483-9244-5162960B69A2}";

		[XmlIgnore]
		public static string strUpdateCheckUrl { get; } = @"https://raw.githubusercontent.com/Invary/Status/main/status.json";


		[XmlIgnore]
		public static string strDownloadUrl { get; } = @"https://github.com/Invary/IvyClipImageSaver/Releases";

		[XmlIgnore]
		public static string strProjectUrl { get; } = @"https://github.com/Invary/IvyClipImageSaver";



		public static string SaveFolder { get; set; } = "";

		public static string FileNameTemplate { get; set; } = "";


		public static ImageFormat FileFormat { get; set; } = ImageFormat.Png;

		public static string FileExtension
		{
			get
			{
				if (FileFormat == ImageFormat.Png)
					return ".png";
				if (FileFormat == ImageFormat.Bmp)
					return ".bmp";
				if (FileFormat == ImageFormat.Jpeg)
					return ".jpg";
				if (FileFormat == ImageFormat.Gif)
					return ".gif";
				if (FileFormat == ImageFormat.Tiff)
					return ".tiff";

				Debug.Assert(false);

				return ".png";
			}
		}


		public static bool BeepSucceeded { get; set; } = false;
		public static bool BeepFailed { get; set; } = false;



		public static bool OverWrite { get; set; } = false;


		public static Size? ResizeTo { get; set; } = null;




		public static bool IsCut { get { return (CutLeft > 0 || CutRight > 0 || CutTop > 0 || CutBottom > 0) ? true : false; } }

		//trimming
		public static int CutTop { get; set; } = 0;
		public static int CutLeft { get; set; } = 0;
		public static int CutRight { get; set; } = 0;
		public static int CutBottom { get; set; } = 0;
	}
}
