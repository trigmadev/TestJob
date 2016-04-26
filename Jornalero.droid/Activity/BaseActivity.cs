using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;

namespace Jornalero.droid
{
	public abstract class BaseActivity : Activity
	{
		#region Global Variables

		protected LocationManager LocManager = Android.App.Application.Context.GetSystemService (LocationService) as LocationManager;

		#endregion

		#region Create View

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (LayoutResource);
		}

		#endregion

		#region Abstract Layout Variable

		protected abstract int LayoutResource {
			get;
		}

		#endregion

		#region Alert Location disabled

		public void ShowLocationDisableAlert ()
		{
			RunOnUiThread (() => {
				AlertDialog.Builder builder;
				builder = new AlertDialog.Builder (this);
				builder.SetTitle ("Location Service Disabled");
				builder.SetMessage ("Please enable location services.");
				builder.SetCancelable (false);
				builder.SetPositiveButton ("Enable", delegate {
					var intent = new Intent (Android.Provider.Settings.ActionLocationSourceSettings);
					StartActivity (intent);
				});
				builder.Show ();
			});
		}

		#endregion
	}
}