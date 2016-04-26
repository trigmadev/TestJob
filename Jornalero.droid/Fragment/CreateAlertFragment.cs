using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;
using Android.Provider;
using Java.IO;
using Android.Content.PM;
using PInvoke;

namespace Jornalero.droid
{
	public class CreateAlertFragment: Fragment
	{
		#region Global Variables

		View view;

		#endregion

		#region Create View, Initialize UI Controls & Events


		public CreateAlertFragment ()
		{
			this.RetainInstance = true;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (view == null) {
				view = inflater.Inflate (Resource.Layout.CreateAlert, container, false);
			}
			InitializeComponents ();

			return view;
		}


		private void InitializeComponents ()
		{
			var llVehicle = view.FindViewById<LinearLayout> (Resource.Id.llVehicle);
			var llOverlay = view.FindViewById<LinearLayout> (Resource.Id.llOverlay);


			llVehicle.Click += (sender, e) => {
				Activity.StartActivity (typeof(AddVehicleFragment));
			};

			var btnUpload = view.FindViewById<Button> (Resource.Id.btnUpload);
			btnUpload.Click += (object sender, EventArgs e) => {
				llOverlay.Visibility = ViewStates.Visible;
			};
		
		}

		Java.IO.File _file;
		Java.IO.File _dir;

		private void TakeAPicture (object sender, EventArgs eventArgs)
		{
			Intent intent = new Intent (MediaStore.ActionImageCapture);

			_file = new Java.IO.File (_dir, String.Format ("myPhoto_{0}.jpg", Guid.NewGuid ()));

			intent.PutExtra (MediaStore.ExtraOutput, Android.Net.Uri.FromFile (_file));

			StartActivityForResult (intent, 100);
		}

		#endregion
	}
}