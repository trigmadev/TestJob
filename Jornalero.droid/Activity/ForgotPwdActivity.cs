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
using Android.Content.PM;
using Android.Graphics;
using Jornalero.Core;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Android.Views.Animations;

namespace Jornalero.droid
{
	[Activity (Label = "Forgot Password", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]
	public class ForgotPwdActivity : Activity
	{
		#region Global Variables

		EditText etEmail;
		Button btnSend;
		string VarificationCode, Email;

		#endregion

		#region Create View, Initialize UI Controls & Events

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.ForgotPwd);
			InitializeComponents ();
		}

		private void InitializeComponents ()
		{
			etEmail = FindViewById<EditText> (Resource.Id.etEmail);
			btnSend = FindViewById<Button> (Resource.Id.btnSend);
			btnSend.Click += async delegate {
				await ForgotPassword ();
			};
			var tvSignIn = FindViewById<TextView> (Resource.Id.tvSignIn);
			tvSignIn.Click += (sender, e) => {
				StartActivity (typeof(SignInActivity));
			};
			var tvSignUp = FindViewById<TextView> (Resource.Id.tvSignUp);
			tvSignUp.Click += (sender, e) => {
				StartActivity (typeof(SignUpActivity));
			};
		}

		#endregion

		#region Send Email Click

		private async Task ForgotPassword ()
		{
			if (string.IsNullOrEmpty (etEmail.Text)) {
				etEmail.Hint = Constants.BlankMsg;
				etEmail.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etEmail.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			}
			JornaleroResponse<ForgotPassword> objResp = new JornaleroResponse<ForgotPassword> ();
			User objReq = new User ();
			objReq.Email = SecurityClass.EncryptAes (etEmail.Text.Trim ());
			ServiceHandler.PostData<JornaleroResponse<ForgotPassword>, User> (Constants.ForgotPassword, HttpMethod.Post,
				objReq);
			var TopAlertView = LayoutInflater.From (this).Inflate (Resource.Layout.TopAlertView, null);
			var cardAnimationIn = AnimationUtils.LoadAnimation (this, Resource.Animation.abc_slide_in_top);
			cardAnimationIn.Duration = 500;
			TopAlertView.StartAnimation (cardAnimationIn);
			var layout = new ViewGroup.LayoutParams (
				             ViewGroup.LayoutParams.WrapContent,
				             ViewGroup.LayoutParams.WrapContent);
			AddContentView (TopAlertView, layout);
			var rlRight = TopAlertView.FindViewById<RelativeLayout> (Resource.Id.rlRight);
			rlRight.Click += (isender, ie) => {
				var cardAnimationOut = AnimationUtils.LoadAnimation (this, Resource.Animation.abc_slide_out_top);
				cardAnimationOut.Duration = 500;
				TopAlertView.StartAnimation (cardAnimationOut);
				((ViewGroup)TopAlertView.Parent).RemoveView (TopAlertView);
				StartActivity (typeof(SignInActivity));
			};
		}

		#endregion
	}
}