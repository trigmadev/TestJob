using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using System.Threading.Tasks;
using Android.Graphics;
using System.IO;
using System.Net;
using Android.Provider;

namespace Jornalero.droid
{
	public class PopDialogView : PopupWindow
	{
		public static  Activity ctx;
		View PopupView;

		protected PopDialogView (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}

		public PopDialogView (Activity context) : base (context)
		{
			ctx = context;
			PopupView = LayoutInflater.From (ctx).Inflate (Resource.Layout.PopDialogView, null);
			base.ContentView = PopupView;

			DisplayMetrics metrics = new DisplayMetrics ();
			(ctx).WindowManager.DefaultDisplay.GetMetrics (metrics);
			base.Height = (int)(metrics.HeightPixels * 0.97f);
			base.Width = (int)(metrics.WidthPixels * 1.0f);
			InitializeComponents (PopupView);
		}

		public void SetupView (View view)
		{
			ContentView.FindViewById<LinearLayout> (Resource.Id.layoutPopPdfView).Visibility = ViewStates.Invisible;
			var layoutPopView = ContentView.FindViewById<LinearLayout> (Resource.Id.layoutPopView);
			layoutPopView.AddView (view);
		}

		public void SetupWebView (View view)
		{
			ContentView.FindViewById<LinearLayout> (Resource.Id.layoutPopView).Visibility = ViewStates.Invisible;
			var layoutPopPdfView = ContentView.FindViewById<LinearLayout> (Resource.Id.layoutPopPdfView);
			layoutPopPdfView.AddView (view);
		}

		private void InitializeComponents (View view)
		{
			// If the PopupWindow should be focusable
			Focusable = true;
			// If you need the PopupWindow to dismiss when touched outside 
			SetBackgroundDrawable (new ColorDrawable ());
			// Using location, the PopupWindow will be displayed right under anchorView
			ShowAtLocation (PopupView, GravityFlags.Bottom, 0, 0);
			ImageView ivClose = view.FindViewById<ImageView> (Resource.Id.ivClose);
			ivClose.Click += CloseMe;
		}

		private void CloseMe (object sender, EventArgs e)
		{
			this.Dismiss (); 
			this.Dispose ();
		}
	}
}