//
//  ControllerHelper.cs
//
//  Author:
//       welcome <>
//
//  Copyright (c) 2015 welcome
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Android.App;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Sql;
using System.Net;
using Jornalero.Core;

namespace Jornalero.droid
{
	public static class ControllerHelper
	{
		static ProgressDialog pd;

		public static Task<int> ShowAlert (Context context, string title, string message, params string[] buttons)
		{
			var taskCompletionSource = new TaskCompletionSource<int> ();
			var alert = new AlertDialog.Builder (context);
			alert.SetTitle (title);
			alert.SetItems (buttons, (sender, args) => taskCompletionSource.TrySetResult (args.Which));
			alert.Create ();
			alert.Show ();
			return taskCompletionSource.Task;
		}

		public static Task<int> ShowCustomAlert (Context context, string title, string message, params string[] buttons)
		{
			var taskCompletionSource = new TaskCompletionSource<int> ();
			var alert = (new AlertDialog.Builder (context)).Create ();
			alert.SetMessage (message);
			alert.SetTitle (title);
			if (buttons.Length >= 1 && buttons [0] != null)
				alert.SetButton (buttons [0], (sender, args) => taskCompletionSource.TrySetResult (0));
			if (buttons.Length >= 2 && buttons [1] != null)
				alert.SetButton2 (buttons [1], (sender, args) => taskCompletionSource.TrySetResult (1));
			if (buttons.Length >= 3 && buttons [2] != null)
				alert.SetButton3 (buttons [2], (sender, args) => taskCompletionSource.TrySetResult (2));
			alert.Show ();
			return taskCompletionSource.Task;
		}

		public static void ShowWebExceptionMessage (Context context)
		{
			ControllerHelper.ShowInformationAlert (context,
				"Connection Error",
				"Please check that you have an active internet connection and try again.",
				"OK");
		}

		public static void ShowWebServerExceptionMessage (Context context)
		{
			ControllerHelper.ShowInformationAlert (context,
				"Server Error",
				"Server error",
				"OK");
		}

		public static void ShowInformationAlert (Context context, string title, string message, string cancelTitle)
		{
			new AlertDialog.Builder (context)
				.SetTitle (title)
				.SetMessage (message)
				.SetCancelable (true)
				.SetNegativeButton (cancelTitle, delegate {
			})
				.Show ();
		}

		public static void ShowInformationAlert (Context context, string title, string message, string cancelTitle, EventHandler<DialogClickEventArgs> eventHandler)
		{
			new AlertDialog.Builder (context)
				.SetTitle (title)
				.SetMessage (message)
				.SetCancelable (true)
				.SetNegativeButton (cancelTitle, eventHandler)
				.Show (); 
		}

		public static void ShowFeatureUnavailableDialog (Context context, string displayMessage = "Check the web app at www.jornalero.co")
		{
			new AlertDialog.Builder (context)
				.SetTitle ("Coming soon")
				.SetMessage (displayMessage)
				.SetCancelable (true)
				.SetNegativeButton ("Cancel", delegate {
			})
				.Show ();
		}

		public static void ShowFeatureComingsoonDialog (Context context, string displayMessage = "Coming soon..")
		{
			new AlertDialog.Builder (context)
				.SetTitle ("Message")
				.SetMessage (displayMessage)
				.SetCancelable (true)
				.SetNegativeButton ("Cancel", delegate {
			})
				.Show ();
		}

		public static void SetPreference (Context context, string pref, bool val)
		{
			var preferences = context.GetSharedPreferences (Constants.JornaleroPreferences, FileCreationMode.Private);
			var editor = preferences.Edit ();
			editor.PutBoolean (pref, val);
			editor.Commit ();
		}

		public static bool GetPreference (Context context, string pref, bool def)
		{
			if (context != null) {
				var preferences = context.GetSharedPreferences (Constants.JornaleroPreferences, FileCreationMode.Private);  
				return preferences.GetBoolean (pref, def);
			}
			return false;
		}

		public static void SetPreference (Context context, string pref, string val)
		{
			var preferences = context.GetSharedPreferences (Constants.JornaleroPreferences, FileCreationMode.Private); 
			var editor = preferences.Edit ();
			editor.PutString (pref, val);
			editor.Commit ();
		}

		public static string GetPreference (Context context, string pref, string def)
		{
			if (context != null) {
				var preferences = context.GetSharedPreferences (Constants.JornaleroPreferences, FileCreationMode.Private);  
				return preferences.GetString (pref, def);
			}
			return string.Empty;
		}

		public static void SetPreference (Context context, string pref, int val)
		{
			var preferences = context.GetSharedPreferences (Constants.JornaleroPreferences, FileCreationMode.Private); 
			var editor = preferences.Edit ();
			editor.PutInt (pref, val);
			editor.Commit ();
		}

		public static int GetPreference (Context context, string pref, int def)
		{
			if (context != null) {
				var preferences = context.GetSharedPreferences (Constants.JornaleroPreferences, FileCreationMode.Private);  
				return preferences.GetInt (pref, def);
			}
			return 0;
		}

		public static void ClearPreference (Context context)
		{
			context.GetSharedPreferences (Constants.JornaleroPreferences, FileCreationMode.Private).Edit ().Clear ().Commit ();
		}

		public static ProgressDialog ShowProgressDialog (Context context, string strMsg, bool cancelable = false)
		{
			if (context != null) {
				pd = new ProgressDialog (context);
				pd.RequestWindowFeature (2);
				pd.SetMessage (strMsg);
				pd.SetCancelable (cancelable);
				pd.Show ();
				return pd;
			}
			return null;
		}

		public static ProgressDialog ShowProgressDialog (Context context, String strMsg, int theme)
		{
			pd = new ProgressDialog (context, theme);
			pd.RequestWindowFeature (2);
			pd.SetMessage (strMsg);
			pd.SetCancelable (true);
			pd.Show ();
			return pd;
		}

		public static void CloseProgressDialog ()
		{
			if (pd != null)
				pd.Dismiss ();
		}

		public static Bitmap LoadAndResizeBitmap (this string fileName, int width, int height)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile (fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width) {
				inSampleSize = outWidth > outHeight
					? outHeight / height
					: outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			Bitmap resizedBitmap = BitmapFactory.DecodeFile (fileName, options);

			return resizedBitmap;
		}

		public static bool IsOnline (Context mContext)
		{
			ConnectivityManager cm = (ConnectivityManager)mContext.GetSystemService (Context.ConnectivityService);
			NetworkInfo netInfo = cm.ActiveNetworkInfo;
			if (netInfo != null && netInfo.IsConnectedOrConnecting) {
				return true;
			}
			new AlertDialog.Builder (mContext)
				.SetTitle ("Connection Error")
				.SetMessage ("Please check that you have an active internet connection and try again.")
				.SetCancelable (true)
				.SetNegativeButton ("OK", (sender, e) => {
			})
				.Show (); 
			return false;
		}

		public static bool DetectNetwork (Context mContext)
		{

			var connectivityManager = (ConnectivityManager)mContext.GetSystemService (Context.ConnectivityService);

			var activeConnection = connectivityManager.ActiveNetworkInfo;

			if ((activeConnection != null) && activeConnection.IsConnected) {
				// we are connected to a network.
				return true;
			}

			var mobile = connectivityManager.GetNetworkInfo (ConnectivityType.Mobile).GetState ();
			if (mobile == NetworkInfo.State.Connected) {
				return true;
			}

			var wifiState = connectivityManager.GetNetworkInfo (ConnectivityType.Wifi).GetState ();
			if (wifiState == NetworkInfo.State.Connected) {
				return true;
			}
			return false;
		}

		//		public static Devices DeviceType (Context context)
		//		{
		//			if ((context.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask) >= ScreenLayout.SizeXlarge) {
		//				return Devices.SizeXlarge;
		//			} else if ((context.Resources.Configuration.ScreenLayout & ScreenLayout.SizeMask) >= ScreenLayout.SizeLarge) {
		//				return Devices.SizeLarge;
		//			} else {
		//				return Devices.Phone;
		//			}
		//		}

		public static Typeface SetJornaleroFont (Context context)
		{
			if (context != null)
				return Typeface.CreateFromAsset (context.Assets, Constants.JornaleroFontStyle);
			else
				return null;
		}

		public static void SetListViewHeightBasedOnChildren (ListView listView)
		{
			IListAdapter listAdapter = listView.Adapter;
			if (listAdapter == null) {
				return;
			}

			int totalHeight = 0;
			for (int i = 0; i < listAdapter.Count; i++) {
				View listItem = listAdapter.GetView (i, null, listView);
				listItem.Measure (View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Unspecified), View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Unspecified));
				totalHeight += listItem.MeasuredHeight;
			}

			ViewGroup.LayoutParams lparams = listView.LayoutParameters;
			lparams.Height = totalHeight + (listView.DividerHeight * (listAdapter.Count - 1));
			listView.LayoutParameters = lparams;
			listView.RequestLayout ();
		}

		public static void SetListViewHeightBasedOnChildren (GridView gridView)
		{
			IListAdapter listAdapter = gridView.Adapter;
			if (listAdapter == null) {
				return;
			}

			int totalHeight = 0;
			for (int i = 0; i < listAdapter.Count; i++) {
				View listItem = listAdapter.GetView (i, null, gridView);
				listItem.Measure (View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Unspecified), View.MeasureSpec.MakeMeasureSpec (0, MeasureSpecMode.Unspecified));
				totalHeight += listItem.MeasuredHeight;
			}

			ViewGroup.LayoutParams lparams = gridView.LayoutParameters;
			lparams.Height = totalHeight + (listAdapter.Count - 1);
			gridView.LayoutParameters = lparams;
			gridView.RequestLayout ();
		}

		public static string ToMMDDyyy (Date mydate)
		{ 
			SimpleDateFormat formatter = new SimpleDateFormat ("MM-dd-yyyy"); 
			string date = formatter.Format (mydate); 
			return date; 
		}

		public static Bitmap GetImageBitmapFromUrl (string url)
		{
			Bitmap imageBitmap = null;
			using (var webClient = new WebClient ()) {
				var imageBytes = webClient.DownloadData (url);
				if (imageBytes != null && imageBytes.Length > 0) {
					imageBitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);
				}
			}
			return imageBitmap;
		}

		public static void WriteLog (string LogMessage)
		{
			var path = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
			var filename = System.IO.Path.Combine (path.ToString (), "jornalero.txt");
			using (var streamWriter = new System.IO.StreamWriter (filename, true)) {
				streamWriter.WriteLine (DateTime.Now.ToString () + "_" + LogMessage);
			}
		}
	}
}