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
using Jornalero.Core;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Android.Content.PM;

namespace Jornalero.droid
{
	[Activity (Label = "@string/app_name", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]
	public class SplashActivity : Activity
	{
		#region Global Variables

		#endregion

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Splash);

			var rlSplash = FindViewById <RelativeLayout> (Resource.Id.rlSplash);
			//Localized.SetCurrentLocale (Resources.Configuration.Locale.Language);
			//rlSplash.SetBackgroundResource (Resource.Drawable.main_bg);
			// Simulate a long loading process on app startup.
			new Handler ().PostDelayed (new Java.Lang.Runnable (() => {
				if (ControllerHelper.GetPreference (this, SystemResources.LaborId, 0) > 0) {
					Constants.LaborId = ControllerHelper.GetPreference (this, SystemResources.LaborId, 0);
					Constants.ProfileCompleteness = ControllerHelper.GetPreference (this, SystemResources.ProfileCompleteness, 0);
					StartActivity (typeof(HomeActivity));
					this.Finish ();
				} else {
					StartActivity (typeof(MainActivity));
					this.Finish ();
				}
			}), 2000);
		}
	}
}