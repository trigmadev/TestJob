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
using Jornalero.Core;

namespace Jornalero.droid
{
	public class MyProfilesFragment : Android.Support.V4.App.Fragment
	{
		#region Global Variables

		View view;

		#endregion

		#region Create View, Initialize UI Controls & Events

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (view == null) {
				view = inflater.Inflate (Resource.Layout.MyProfiles, container, false);
				InitializeComponents ();
			}
			return view;
		}

		public override void OnStart ()
		{
			base.OnStart ();
			if (view != null)
				view.FindViewById<TextView> (Resource.Id.tvProfilePercentage).Text = (Constants.ProfileCompleteness.ToString () + "%");
		}

		private void InitializeComponents ()
		{
			var rlPersonal = view.FindViewById<RelativeLayout> (Resource.Id.rlPersonal);
			rlPersonal.Click += (sender, e) => {
				Activity.StartActivity (typeof(PersonalInfoActivity));
			};
			var rlEmail = view.FindViewById<RelativeLayout> (Resource.Id.rlEmail);
			rlEmail.Click += (sender, e) => {
				Activity.StartActivity (typeof(ChangeEmailActivity));
			};
			var rlPassword = view.FindViewById<RelativeLayout> (Resource.Id.rlPassword);
			rlPassword.Click += (sender, e) => {
				Activity.StartActivity (typeof(ChangePasswordActivity));
			};
			var rlWorkCenter = view.FindViewById<RelativeLayout> (Resource.Id.rlWorkCenter);
			rlWorkCenter.Click += (sender, e) => {
				Activity.StartActivity (typeof(ChangeWorkCenterActivity));
			};
		}

		#endregion
	}
}