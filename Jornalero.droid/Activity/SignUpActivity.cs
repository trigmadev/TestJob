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
	[Activity (Label = "Sign Up", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]
	public class SignUpActivity : BaseActivity
	{
		#region Global variables

		bool IsViewPwd;
		EditText etFirstName, etLastName, etEmail, etCenterCode, etPassword;
		Button btnSignUp;

		#endregion

		#region implemented abstract members of BaseActivity

		protected override int LayoutResource {
			get {
				return Resource.Layout.SignUp;
			}
		}

		#endregion

		#region Create View, Initialize UI Controls & Events

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			//SetContentView (Resource.Layout.SignUp);
			InitializeComponents ();
			//InputMethodManager imm = (InputMethodManager)GetSystemService (Context.InputMethodService);
			//imm.HideSoftInputFromInputMethod (Window.CurrentFocus.WindowToken, 0);
		}

		private void InitializeComponents ()
		{
			etPassword = FindViewById<EditText> (Resource.Id.etPassword);
			etFirstName = FindViewById<EditText> (Resource.Id.etFirstName);
			etLastName = FindViewById<EditText> (Resource.Id.etLastName);
			etEmail = FindViewById<EditText> (Resource.Id.etEmail);
			etCenterCode = FindViewById<EditText> (Resource.Id.etCenterCode);
			var img_view_icon = FindViewById<ImageView> (Resource.Id.img_view_icon);
			img_view_icon.Click += ImgViewPasswordClick;
			var tvSignIn = FindViewById<TextView> (Resource.Id.tvSignIn);
			tvSignIn.Click += (sender, e) => {
				StartActivity (typeof(SignInActivity));
			};
			btnSignUp = FindViewById<Button> (Resource.Id.btnSignUp);
			btnSignUp.Click += BtnSignUp_Click;
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

		#region Sign Up Click

		void BtnSignUp_Click (object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (etFirstName.Text)) {
				etFirstName.Hint = Constants.BlankMsg;
				etFirstName.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etFirstName.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			} else if (string.IsNullOrEmpty (etLastName.Text)) {
				etLastName.Hint = Constants.BlankMsg;
				etLastName.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etLastName.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			} else if (string.IsNullOrEmpty (etEmail.Text)) {
				etEmail.Hint = Constants.BlankMsg;
				etEmail.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etEmail.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			} else if (string.IsNullOrEmpty (etCenterCode.Text)) {
				etCenterCode.Hint = Constants.BlankMsg;
				etCenterCode.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etCenterCode.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			} else if (string.IsNullOrEmpty (etPassword.Text)) {
				etPassword.Hint = Constants.BlankMsg;
				etPassword.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etPassword.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			}
			ControllerHelper.ShowProgressDialog (this, Constants.Processing);
			RegisterLabor objUser = new RegisterLabor ();
			objUser.Email = SecurityClass.EncryptAes (etEmail.Text.Trim ());
			objUser.Password = SecurityClass.EncryptAes (etPassword.Text.Trim ());
			objUser.FirstName = SecurityClass.EncryptAes (etFirstName.Text.Trim ());
			objUser.LastName = SecurityClass.EncryptAes (etLastName.Text.Trim ());
			objUser.WorkCenterCode = etCenterCode.Text.Trim ();
			objUser.IsAgreed = true;
			objUser.DeviceType = Convert.ToString (DeviceType.Droid);
			objUser.DeviceToken = Constants.DeviceToken;
			objUser.Latitude = 30.7654;
			objUser.Longitude = 71.7654;
			if (string.IsNullOrEmpty (objUser.DeviceToken))
				objUser.DeviceToken = Constants.DeviceToken;
			JornaleroResponse<int> objResp = new JornaleroResponse<int> ();
			ServiceHandler.PostData<JornaleroResponse<int>, RegisterLabor> (Constants.RegisterLaborUser, HttpMethod.Post,
				objUser).ContinueWith ((completed) => {
				try {
					if (!completed.IsFaulted) {
						objResp = completed.Result;
						if (objResp != null) {
							if (objResp.status_code == (int)HttpStatusCode.OK) {
								ControllerHelper.SetPreference (this, SystemResources.LaborId, objResp.data);
								ControllerHelper.SetPreference (this, SystemResources.FirstName, etFirstName.Text);
								ControllerHelper.SetPreference (this, SystemResources.LastName, etLastName.Text);
								ControllerHelper.SetPreference (this, SystemResources.Email, etEmail.Text);
								ControllerHelper.SetPreference (this, SystemResources.Password, etPassword.Text);
								ControllerHelper.SetPreference (this, SystemResources.CenterCode, etCenterCode.Text);
								Constants.LaborId = objResp.data;
								Constants.IsSignUp = true;
								Constants.ProfileCompleteness = 40;
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