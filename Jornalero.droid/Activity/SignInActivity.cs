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
using Android.Views.InputMethods;
using Android.Graphics;
using Jornalero.Core;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Jornalero.droid
{
	[Activity (Label = "Sign In", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]
	public class SignInActivity : BaseActivity
	{
		#region Global variables

		bool IsViewPwd;
		EditText etEmail, etPassword;
		Button btnSignIn;

		#endregion

		#region implemented abstract members of BaseActivity

		protected override int LayoutResource {
			get {
				return Resource.Layout.SignIn;
			}
		}

		#endregion

		#region Create View, Initialize UI Controls & Events

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			//SetContentView (Resource.Layout.SignIn);
			InitializeComponents ();
		}

		private void InitializeComponents ()
		{
			etPassword = FindViewById<EditText> (Resource.Id.etPassword);
			etEmail = FindViewById<EditText> (Resource.Id.etEmail);
			var img_view_icon = FindViewById<ImageView> (Resource.Id.img_view_icon);
			img_view_icon.Click += ImgViewPasswordClick;
			var tvSignUp = FindViewById<TextView> (Resource.Id.tvSignUp);
			tvSignUp.Click += (sender, e) => {
				StartActivity (typeof(SignUpActivity));
			};
			var tvForgotPwd = FindViewById<TextView> (Resource.Id.tvForgotPwd);
			tvForgotPwd.Click += (sender, e) => {
				StartActivity (typeof(ForgotPwdActivity));
			};
			btnSignIn = FindViewById<Button> (Resource.Id.btnSignIn);
			btnSignIn.Click += async delegate {
				await SignIn ();
			};
		}

		#endregion

		#region View Password Click

		void ImgViewPasswordClick (object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty (etPassword.Text)) {
				if (IsViewPwd) {
					IsViewPwd = false;
					etPassword.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
					etPassword.SetSelection (etPassword.Length ());
				} else {
					IsViewPwd = true;
					etPassword.InputType = Android.Text.InputTypes.TextVariationVisiblePassword;
					etPassword.SetSelection (etPassword.Length ());
				}
			}
		}

		#endregion

		#region Sign In Click

		private async Task SignIn ()
		{
			if (string.IsNullOrEmpty (etEmail.Text)) {
				etEmail.Hint = Constants.BlankMsg;
				etEmail.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etEmail.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			} else if (string.IsNullOrEmpty (etPassword.Text)) {
				etPassword.Hint = Constants.BlankMsg;
				etPassword.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etPassword.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			}

			ControllerHelper.ShowProgressDialog (this, Constants.Processing);
			LaborUserLogin objUser = new LaborUserLogin ();
			objUser.LoginId = SecurityClass.EncryptAes (etEmail.Text.Trim ());
			objUser.Password = SecurityClass.EncryptAes (etPassword.Text.Trim ());
			JornaleroResponse<LoginResponse> objResp = new JornaleroResponse<LoginResponse> ();
			ServiceHandler.PostData<JornaleroResponse<LoginResponse>, LaborUserLogin> (Constants.LaborUserLogin, HttpMethod.Post,
				objUser).ContinueWith ((completed) => {
				try {
					if (!completed.IsFaulted) {
						objResp = completed.Result;
						if (objResp != null) {
							if (objResp.status_code == (int)HttpStatusCode.OK && objResp.data != null) {
								ControllerHelper.SetPreference (this, SystemResources.LaborId, objResp.data.UserId);
								ControllerHelper.SetPreference (this, SystemResources.Password, etPassword.Text);
								ControllerHelper.SetPreference (this, SystemResources.FirstName, SecurityClass.DecryptAes (objResp.data.FirstName));
								ControllerHelper.SetPreference (this, SystemResources.LastName, SecurityClass.DecryptAes (objResp.data.LastName));
								ControllerHelper.SetPreference (this, SystemResources.Email, etEmail.Text.Trim ());								
								ControllerHelper.SetPreference (this, SystemResources.Gender, objResp.data.Gender);
								ControllerHelper.SetPreference (this, SystemResources.Age, objResp.data.Age);
								ControllerHelper.SetPreference (this, SystemResources.OriginId, objResp.data.OriginId);
								Constants.OriginId = objResp.data.OriginId;
								ControllerHelper.SetPreference (this, SystemResources.OriginName, objResp.data.OriginName);
								ControllerHelper.SetPreference (this, SystemResources.OccupationId, objResp.data.OccupationId);
								Constants.OccupationId = objResp.data.OccupationId;
								ControllerHelper.SetPreference (this, SystemResources.OccupationName, objResp.data.OccupationName);
								ControllerHelper.SetPreference (this, SystemResources.ProfileCompleteness, objResp.data.ProfileCompleteness);
								Constants.LaborId = objResp.data.UserId;
								Constants.ProfileCompleteness = objResp.data.ProfileCompleteness;
								StartActivity (typeof(HomeActivity));
							} else
								Toast.MakeText (this, objResp.message, ToastLength.Long).Show ();
						} else
							Toast.MakeText (this, Constants.ErrorMessageEnglish, ToastLength.Long).Show ();
					} else
						Toast.MakeText (this, Constants.ErrorMessageEnglish, ToastLength.Long).Show ();
				} catch (Exception ex) {
					Toast.MakeText (this, Constants.ErrorMessageEnglish, ToastLength.Long).Show ();
					Console.WriteLine (ex);
				} finally {
					ControllerHelper.CloseProgressDialog ();
				}
			}, TaskScheduler.FromCurrentSynchronizationContext ());
		}

		#endregion
	}
}