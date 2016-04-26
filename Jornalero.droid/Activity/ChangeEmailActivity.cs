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
using Android.Graphics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace Jornalero.droid
{
	[Activity (Label = "Change Email", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]		
	public class ChangeEmailActivity : BaseActivity
	{
		#region Global Variables

		EditText etNewEmail;
		TextView tvCurrentEmail;

		#endregion

		#region Set Layout Resource

		protected override int LayoutResource {
			get {
				return Resource.Layout.ChangeEmail;
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
			FindViewById<TextView> (Resource.Id.tvTitle).SetText (Resource.String.change_email_address);
			FindViewById<TextView> (Resource.Id.tvProfilePercentage).Visibility = ViewStates.Gone;
			etNewEmail = FindViewById<EditText> (Resource.Id.etNewEmail);
			tvCurrentEmail = FindViewById<TextView> (Resource.Id.tvCurrentEmail);
			tvCurrentEmail.Text = ControllerHelper.GetPreference (this, SystemResources.Email, string.Empty);
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
			if (string.IsNullOrEmpty (etNewEmail.Text)) {
				etNewEmail.Hint = Constants.BlankMsg;
				etNewEmail.SetHintTextColor (Color.ParseColor (Constants.error_color));
				etNewEmail.SetBackgroundResource (Resource.Drawable.error_bg);
				return;
			}
			if (etNewEmail.Text == tvCurrentEmail.Text) {
				Toast.MakeText (this, "Please input different email", ToastLength.Long).Show ();
				return;
			}

			ControllerHelper.ShowProgressDialog (this, Constants.Processing);
			LaborProfile objUser = new LaborProfile ();
			objUser.Email = SecurityClass.EncryptAes (etNewEmail.Text.Trim ());
			objUser.UserId = Constants.LaborId;
			JornaleroResponse<int> objResp = new JornaleroResponse<int> ();
			ServiceHandler.PostData<JornaleroResponse<int>, LaborProfile> (Constants.ChangeEmail, HttpMethod.Post,
				objUser).ContinueWith ((completed) => {
				try {
					if (!completed.IsFaulted) {
						objResp = completed.Result;
						if (objResp != null) {
							if (objResp.status_code == (int)HttpStatusCode.OK) {
								Toast.MakeText (this, "Email has been updated successfully", ToastLength.Long).Show ();
								ControllerHelper.SetPreference (this, SystemResources.Email, etNewEmail.Text);
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