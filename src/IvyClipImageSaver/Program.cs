using Invary.Utility;
using System.Drawing.Imaging;

namespace Invary.IvyClipImageSaver
{
	internal static class Program
	{
		/// <summary>
		///  The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			bool bSucceeded = false;

			Console.WriteLine($"IvyClipImageSaver {Setting.strVersion}");
			Console.WriteLine(Setting.strProjectUrl);
			Console.WriteLine("\n");

			try
			{
				//parse command line options
				{
					var ret = CheckAndApplyCommandLineArgs();
					if (ret == false)
					{
						Console.WriteLine("error: Invalid command line args.");
						return;
					}
				}

				var image = ClipboardUty.GetImage();
				if (image == null)
				{
					Console.WriteLine("error: Image is not exists in clipboard.");
					return;
				}

				Console.WriteLine("Image loaded.");
				Console.WriteLine($"width: {image.Width}px");
				Console.WriteLine($"height: {image.Height}px");

				string file = GetFilePath();


				if (Setting.IsCut)
				{
					int cutH = Setting.CutLeft + Setting.CutRight;
					int cutV = Setting.CutTop + Setting.CutBottom;

					int width = image.Width - cutH;
					int height = image.Height - cutV;

					if (width <= 0 || height <= 0)
					{
						Console.WriteLine("error: Image size is too small after cuting");
						Console.WriteLine($"original image width={image.Width}, height={image.Height}");
						Console.WriteLine($"after cutting image width={width}, height={height}");
						return;
					}

					Image bmp = new Bitmap(image, new Size(width, height));
					using (var g = Graphics.FromImage(bmp))
					{
						var dest = new Rectangle(0, 0, width, height);
						var src = new Rectangle(Setting.CutLeft, Setting.CutTop, width, height);
						g.DrawImage(image, dest, src, GraphicsUnit.Pixel);
					}
					image.Dispose();
					image = bmp;

					Console.WriteLine("Cutting to ...");
					Console.WriteLine($"width: {image.Width}px");
					Console.WriteLine($"height: {image.Height}px");
				}

				if (Setting.ResizeTo != null)
				{
					Image bmp = new Bitmap(image, Setting.ResizeTo.Value);
					using (var g = Graphics.FromImage(bmp))
					{
						var dest = new Rectangle(0, 0, Setting.ResizeTo.Value.Width, Setting.ResizeTo.Value.Height);
						var src = new Rectangle(0, 0, image.Width, image.Height);
						g.DrawImage(image, dest, src, GraphicsUnit.Pixel);
					}
					image.Dispose();
					image = bmp;

					Console.WriteLine("Resize to ...");
					Console.WriteLine($"width: {image.Width}px");
					Console.WriteLine($"height: {image.Height}px");
				}

				image.Save(file, Setting.FileFormat);

				Console.WriteLine("Save to...");
				Console.WriteLine($"path: {file}");

				image.Dispose();
				bSucceeded = true;
				Console.WriteLine("Succeeded.");
			}
#if DEBUG
#else
			catch (Exception)
			{
			}
#endif
			finally
			{
				if (bSucceeded && Setting.BeepSucceeded)
				{
					Console.Beep(300, 400);
				}
				if (bSucceeded == false && Setting.BeepFailed)
				{
					Console.Beep(1000, 400);
				}

				if (bSucceeded == false)
				{
					var message = "\n\n";
					message += "Command line arguments.\n";

					var options = GetCommandLineOptions();
					foreach (var option in options)
					{
						message += option.Help += "\n";
					}
					Console.WriteLine(message);
				}
			}
		}




		static string GetFilePath()
		{
			string file;

			if (string.IsNullOrEmpty(Setting.SaveFolder))
			{
				//default save folder is desktop
				file = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			}
			else
				file = Setting.SaveFolder;

			file = Path.Combine(file, GetFileNameWithoutExtension());

			if (Setting.OverWrite)
			{
				file += Setting.FileExtension;
			}
			else
			{
				var tmp = file + Setting.FileExtension;
				if (File.Exists(tmp) == false)
				{
					file = tmp;
				}
				else
				{
					//if file exists, add "(n)"

					int n = 0;
					while (true)
					{
						n++;
						tmp = file + $" ({n})" + Setting.FileExtension;
						if (File.Exists(tmp))
							continue;

						file = tmp;
						break;
					}
				}
			}

			return file;
		}



		static string GetFileNameWithoutExtension()
		{
			var name = Setting.FileNameTemplate;

			if (string.IsNullOrEmpty(name))
			{
				return DateTime.Now.ToString("yyyyMMdd_HHmmss");
			}

			var dtNow = DateTime.Now;

			name = name.Replace(":year:", dtNow.ToString("yyyy"));
			name = name.Replace(":month:", dtNow.ToString("MM"));
			name = name.Replace(":day:", dtNow.ToString("dd"));
			name = name.Replace(":hour24:", dtNow.ToString("HH"));
			name = name.Replace(":hour12:", dtNow.ToString("hh"));
			name = name.Replace(":min:", dtNow.ToString("mm"));
			name = name.Replace(":sec:", dtNow.ToString("ss"));

			return name;
		}




		static bool CheckAndApplyCommandLineArgs()
		{
			var options = GetCommandLineOptions();

			var ret = CommandLineOption.AnalyzeCommandLine(options);
			if (ret == false)
				return false;

			foreach (var option in options)
			{
				option.Apply();

				if (option.Failed)
					return false;
			}

			return true;
		}





		static List<CommandLineOption> GetCommandLineOptions()
		{
			List<CommandLineOption> options = new List<CommandLineOption>();

			options.Add(new CommandLineOption("--folder", 1, true)
			{
				Help = "--folder {folder path}",
				OnApply = (option) =>
				{
					Setting.SaveFolder = option.Values[0];

					//TODO: is folder exists / create folder
					//option.Failed = true;
				}
			});


			options.Add(new CommandLineOption("--filename", 1, true)
			{
				Help = "--filename {name}\n\tname can include the following, which will be replaced at runtime.\n\t\t:year:  :month:  :day:  :hour24:  :hour12:  :min:  :sec:",
				OnApply = (option) =>
				{
					Setting.FileNameTemplate = option.Values[0];

					//TODO: validation check
					//option.Failed = true;
				}
			});


			options.Add(new CommandLineOption("--format", 1, true)
			{
				Help = "--format [png|jpg|bmp|tiff|gif]",
				OnApply = (option) =>
				{
					var tmp = option.Values[0].ToLower();

					if (tmp == "png")
						Setting.FileFormat = ImageFormat.Png;
					else if (tmp == "jpg")
						Setting.FileFormat = ImageFormat.Jpeg;
					else if (tmp == "bmp")
						Setting.FileFormat = ImageFormat.Bmp;
					else if (tmp == "tiff")
						Setting.FileFormat = ImageFormat.Tiff;
					else if (tmp == "gif")
						Setting.FileFormat = ImageFormat.Gif;
					else
						option.Failed = true;
				}
			});

			options.Add(new CommandLineOption("--overwrite", 1, true)
			{
				Help = "--overwrite [true|false]",
				OnApply = (option) =>
				{
					var tmp = option.Values[0].ToLower();

					if (tmp == "true")
						Setting.OverWrite = true;
					else
						Setting.OverWrite = false;
				}
			});

			options.Add(new CommandLineOption("--cuttop", 1, true)
			{
				Help = "--cuttop {number of pixel to cut}",
				OnApply = (option) =>
				{
					try
					{
						Setting.CutTop = Convert.ToInt32(option.Values[0]);
						if (Setting.CutTop < 0)
							option.Failed = true;
					}
					catch (Exception)
					{
						option.Failed = true;
					}
				}
			});

			options.Add(new CommandLineOption("--cutbottom", 1, true)
			{
				Help = "--cutbottom {number of pixel to cut}",
				OnApply = (option) =>
				{
					try
					{
						Setting.CutBottom = Convert.ToInt32(option.Values[0]);
						if (Setting.CutBottom < 0)
							option.Failed = true;
					}
					catch (Exception)
					{
						option.Failed = true;
					}
				}
			});

			options.Add(new CommandLineOption("--cutleft", 1, true)
			{
				Help = "--cutleft {number of pixel to cut}",
				OnApply = (option) =>
				{
					try
					{
						Setting.CutLeft = Convert.ToInt32(option.Values[0]);
						if (Setting.CutLeft < 0)
							option.Failed = true;
					}
					catch (Exception)
					{
						option.Failed = true;
					}
				}
			});

			options.Add(new CommandLineOption("--resize", 1, true)
			{
				Help = "--resize {width},{hight}",
				OnApply = (option) =>
				{
					try
					{
						var data = option.Values[0].Split(',');
						if (data.Length != 2)
						{
							option.Failed = true;
							return;
						}

						var width = Convert.ToInt32(data[0]);
						var height = Convert.ToInt32(data[1]);

						if (width <= 0 || height <= 0)
						{
							option.Failed = true;
							return;
						}

						Setting.ResizeTo = new Size(width, height);
					}
					catch (Exception)
					{
						option.Failed = true;
					}
				}
			});


			options.Add(new CommandLineOption("--beep-succeeded", 1, true)
			{
				Help = "--beep-succeeded [on|off]",
				OnApply = (option) =>
				{
					var tmp = option.Values[0].ToLower();

					if (tmp == "on")
						Setting.BeepSucceeded = true;
					else
						Setting.BeepSucceeded = false;
				}
			});


			options.Add(new CommandLineOption("--beep-failed", 1, true)
			{
				Help = "--beep-failed [on|off]",
				OnApply = (option) =>
				{
					var tmp = option.Values[0].ToLower();

					if (tmp == "on")
						Setting.BeepFailed = true;
					else
						Setting.BeepFailed = false;
				}
			});


			//TODO: add option, update check
			//TODO: add option, open image? edit image?



			return options;
		}




	}
}