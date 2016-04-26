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
using Android.Support.V4.Widget;
using Android.Content.Res;
using Android.Content.PM;
using Android.Support.V7.App;
using Android.Support.V4.App;
using Android.Views.Animations;
using Jornalero.Core;

namespace Jornalero.droid
{
	[Activity (Label = "Home", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]

	public class HomeActivity : FragmentActivity
	{
		#region Global Variables

		private DrawerLayout drawerLayout;
		private LinearLayout left_drawer;
		bool IsEnglish;
		Android.Support.V4.App.Fragment fragment = null;

		#endregion

		#region Create View, Initialize UI Controls & Events

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Home);
			if (Constants.IsSignUp)
				ShowTopAlert ();
			InitializeComponents ();
			Constants.IsSignUp = false;
		}

		protected override void OnResume ()
		{
			base.OnResume ();
			FindViewById<TextView> (Resource.Id.tvProfilePercentage).Text = (Constants.ProfileCompleteness.ToString () + "%");
		}

		private void InitializeComponents ()
		{
			IsEnglish = true;
			drawerLayout = FindViewById<DrawerLayout> (Resource.Id.drawerLayout);
			left_drawer = FindViewById<LinearLayout> (Resource.Id.left_drawer);

			// Left toolbar Slide menu click
			var imgMenu = FindViewById<ImageView> (Resource.Id.imgMenu);
			var rlMenu = FindViewById<RelativeLayout> (Resource.Id.rlMenu);
			rlMenu.Click += (sender, e) => {
				if (drawerLayout.IsDrawerOpen (left_drawer)) {
					// Do Nothing
				} else {
					drawerLayout.OpenDrawer (left_drawer);
				}
			};
				
			// Set first fragment to show
			fragment = new MyProfilesFragment ();
			SupportFragmentManager.BeginTransaction ()
				.Replace (Resource.Id.content_frame, fragment)
				.Commit ();
			FindViewById<ImageView> (Resource.Id.imgRight).Visibility = ViewStates.Gone;
			FindViewById<TextView> (Resource.Id.tvTitle).SetText (Resource.String.txt_myProfile_caps);

			#region Left Slide Menu Items Click

			var img_toggle_btn = left_drawer.FindViewById<ImageView> (Resource.Id.img_toggle_btn);
			img_toggle_btn.Click += Img_toggle_btn_Click;
			var rl_logout = left_drawer.FindViewById<RelativeLayout> (Resource.Id.rl_logout);
			rl_logout.Click += async (sender, e) => { 
				if (await ControllerHelper.ShowCustomAlert (this, "Logout", Constants.LogoutAlert, "Logout", "Cancel") == 0) {
					ControllerHelper.ClearPreference (this);
					this.StartActivity (typeof(MainActivity));
					this.Finish ();
				}
			};

			var rlMyProfile = left_drawer.FindViewById<RelativeLayout> (Resource.Id.rlMyProfile);
			rlMyProfile.Click += (sender, e) => {
				fragment = new MyProfilesFragment ();
				SupportFragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, fragment)
					.Commit ();
				drawerLayout.CloseDrawers ();
			};

			#endregion

			#region Bottom tabs Click Events

			var llAlertTab = FindViewById<LinearLayout> (Resource.Id.llAlertTab);
			llAlertTab.Click += (sender, e) => {
				fragment = new CreateAlertFragment ();
				SupportFragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, fragment)
					.Commit ();
				drawerLayout.CloseDrawers ();
			};

			var llCheckinTab = FindViewById<LinearLayout> (Resource.Id.llCheckinTab);
			llCheckinTab.Click += (sender, e) => {
				fragment = new MyProfilesFragment ();
				SupportFragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, fragment)
					.Commit ();
				drawerLayout.CloseDrawers ();
			};

			var llReportTab = FindViewById<LinearLayout> (Resource.Id.llReportTab);
			llReportTab.Click += (sender, e) => {
				fragment = new MyProfilesFragment ();
				SupportFragmentManager.BeginTransaction ()
					.Replace (Resource.Id.content_frame, fragment)
					.Commit ();
				drawerLayout.CloseDrawers ();
			};

			#endregion
		}

		#endregion

		#region Back pressed handle to clear history

		public override void OnBackPressed ()
		{
			MoveTaskToBack (true);
		}

		#endregion

		#region Show/Hide Top Alert View

		private void ShowTopAlert ()
		{
			var TopAlertView = LayoutInflater.From (this).Inflate (Resource.Layout.TopAlertView, null);
			var tvAlert = TopAlertView.FindViewById<TextView> (Resource.Id.tvAlert);
			tvAlert.SetText (Resource.String.signup_msg);
			var cardAnimationIn = AnimationUtils.LoadAnimation (this, Resource.Animation.abc_slide_in_top);
			cardAnimationIn.Duration = 500;
			TopAlertView.StartAnimation (cardAnimationIn);
			var layout = new ViewGroup.LayoutParams (
				             ViewGroup.LayoutParams.MatchParent,
				             ViewGroup.LayoutParams.MatchParent);
			AddContentView (TopAlertView, layout);
			TopAlertView.FindViewById<ImageView> (Resource.Id.imgRight).SetImageResource (Resource.Drawable.forward_errow);
			TopAlertView.FindViewById<ImageView> (Resource.Id.imgLeft).SetImageResource (Resource.Drawable.star);
			var rlRight = TopAlertView.FindViewById<RelativeLayout> (Resource.Id.rlRight);
			rlRight.Click += (isender, ie) => {
				var cardAnimationOut = AnimationUtils.LoadAnimation (this, Resource.Animation.abc_slide_out_top);
				cardAnimationOut.Duration = 500;
				TopAlertView.StartAnimation (cardAnimationOut);
				((ViewGroup)TopAlertView.Parent).RemoveView (TopAlertView);
				StartActivity (typeof(PersonalInfoActivity));
			};
		}

		#endregion

		#region Language Toggle Button Click Handle

		void Img_toggle_btn_Click (object sender, EventArgs e)
		{
			var img_button = sender as ImageView;
			if (IsEnglish) {
				IsEnglish = false;
				img_button.SetBackgroundResource (Resource.Drawable.in_active);
			} else {
				IsEnglish = true;
				img_button.SetBackgroundResource (Resource.Drawable.active);
			}
		}

		#endregion
	}
}