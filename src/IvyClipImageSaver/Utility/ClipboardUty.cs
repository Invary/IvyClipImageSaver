using System.Collections.Generic;
using System.Threading;

namespace Invary.Utility
{
	internal class ClipboardUty
	{



		static void GetImageThread(object? param)
		{
			var list = param as List<Image>;
			if (list == null)
				return;

			list.Clear();
			list.Add(Clipboard.GetImage());
		}

		public static Image? GetImage()
		{
			List<Image> listClip = new List<Image>();

			//Clipboard access may fail if not STA thread.
			Thread thread = new Thread(GetImageThread);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start(listClip);
			thread.Join();

			if (listClip.Count == 0)
				return null;

			return listClip[0];
		}



		static void GetTextThread(object? param)
		{
			var list = param as List<string>;
			if (list == null)
				return;

			list.Clear();
			list.Add(Clipboard.GetText());

			//var tt = Data.GetFormats();
			//if (tt != null && tt.Contains("HTML Format"))
			//	list.Add(Data.GetData("HTML Format"));
		}



		public static List<string> GetText()
		{
			List<string> listClipText = new List<string>();

			//Clipboard access may fail if not STA thread.
			Thread thread = new Thread(GetTextThread);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start(listClipText);
			thread.Join();

			return listClipText;
		}




		static void SetTextThread(object? param)
		{
			var text = param as string;
			if (text == null)
				return;
			Clipboard.SetText(text);
		}


		public static void SetText(string text)
		{
			//Clipboard access may fail if not STA thread.
			Thread thread = new Thread(SetTextThread);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start(text);
			thread.Join();
		}


	}
}
