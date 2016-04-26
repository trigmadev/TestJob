using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Jornalero.Core;
using System;

namespace Jornalero.droid
{
	[Activity (Label = "Landing View", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]
	public class MainActivity : Activity
	{
		#region Create View, Initialize UI Controls & Events

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Main);
			InitializeComponents ();
		}

		private void InitializeComponents ()
		{
			var btnSignIn = FindViewById<Button> (Resource.Id.btn_signIn);
			btnSignIn.Click += (sender, e) => {
				StartActivity (typeof(SignInActivity));
			};

			var btnSignUp = FindViewById<Button> (Resource.Id.btn_signUp);
			btnSignUp.Click += (sender, e) => {
				StartActivity (typeof(SignUpActivity));
			};
		}

		#endregion
	}
}