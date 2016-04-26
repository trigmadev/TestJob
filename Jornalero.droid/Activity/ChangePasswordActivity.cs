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
using Jornalero.Core;
using System.Threading.Tasks;
using Android.Graphics;
using System.Net.Http;
using System.Net;

namespace Jornalero.droid
{
	[Activity (Label = "Change Password", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]		
	public class ChangePasswordActivity : BaseActivity
	{
		#region Global Variables

		EditText etCurrentPassword, etNewPassword;

		#endregion

		#region Set Layout Resource

		protected override int LayoutResource {
			get {
				return Resource.Layout.ChangePassword;
			}
		}

		#endregion

		#region Create View, Initialize UI Controls & Events

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			InitializeComponents ();
		}

		private void InitializeComponents ()
		{
			FindViewById<TextView> (Resource.Id.tvTitle).SetText (Resource.String.change_password);
			FindViewById<TextView> (Resource.Id.tvProfilePercentage).Visibility = ViewStates.Gone;
			etCurrentPassword = FindViewById<EditText> (Resource.Id.etCurrentPassword);
			etNewPassword = FindViewById<EditText> (Resource.Id.etNewPassword);
			var btnCancel = FindViewById<Button> (Resource.Id.btnCancel);
			btnCancel.Click += delegate {
				this.Finish ();
			};
			var imgLeft = FindViewById<ImageView> (Resource.Id.imgLeft);
			imgLeft.Click += delegate {
				this.Finish ();
			};
			var btnSave = FindViewById<Button> (Resource.Id.btnSave);
			btnSave.Click += async delegate {
				await Save ();
			};
		}

		#endregion

		#region Save Profile Info Click

		async Task Save ()
		{
			if (string.IsNullOrEmpty (etCurrentPassword.Text)) {
				etCurrentPassword.Hint = Constants.BlankMsg;
				etCurrentPassword.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etCurrentPassword.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			} else if (string.IsNullOrEmpty (etNewPassword.Text)) {
				etNewPassword.Hint = Constants.BlankMsg;
				etNewPassword.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etNewPassword.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			}

			if (etCurrentPassword.Text != ControllerHelper.GetPreference (this, SystemResources.Password, string.Empty)) {
				Toast.MakeText (this, "Current password does not match", ToastLength.Long).Show ();
				return;
			}
			if (etCurrentPassword.Text == etNewPassword.Text) {
				Toast.MakeText (this, "Please input different password", ToastLength.Long).Show ();
				return;
			}
			ControllerHelper.ShowProgressDialog (this, Constants.Processing);
			LaborProfile objUser = new LaborProfile ();
			objUser.Password = SecurityClass.EncryptAes (etNewPassword.Text.Trim ());
			objUser.UserId = Constants.LaborId;
			JornaleroResponse<int> objResp = new JornaleroResponse<int> ();
			ServiceHandler.PostData<JornaleroResponse<int>, LaborProfile> (Constants.ChangePassword, HttpMethod.Post,
				objUser).ContinueWith ((completed) => {
				try {
					if (!completed.IsFaulted) {
						objResp = completed.Result;
						if (objResp != null) {
							if (objResp.status_code == (int)HttpStatusCode.OK) {
								Toast.MakeText (this, "Password has been updated successfully", ToastLength.Long).Show ();
								ControllerHelper.SetPreference (this, SystemResources.Password, etNewPassword.Text);
								Constants.ProfileCompleteness = 100;
								this.Finish ();
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