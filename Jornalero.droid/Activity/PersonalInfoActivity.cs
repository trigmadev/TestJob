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
using Android.Support.V4.Content;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace Jornalero.droid
{
	[Activity (Label = "Personal Info", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]
	public class PersonalInfoActivity : BaseActivity
	{
		#region Global Variable

		EditText etFirstName, etLastName;
		int GenderTypeId;
		string AgeValue;
		TextView tvMale, tvFemale, tvOther, tvLessThan18, tv18To28, tv29To39, tv40To50, tv51To61, tvGreaterTo61, tvOccupation, tvOrigin;

		#endregion

		#region Set Layout Resource

		protected override int LayoutResource {
			get {
				return Resource.Layout.PersonalInfo;
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
			FindViewById<TextView> (Resource.Id.tvTitle).SetText (Resource.String.personal_info_caps);
			var tvProfilePercentage = FindViewById<TextView> (Resource.Id.tvProfilePercentage);
			tvProfilePercentage.Text = (Constants.ProfileCompleteness.ToString () + "%");
			tvProfilePercentage.Visibility = ViewStates.Visible;
			etFirstName = FindViewById<EditText> (Resource.Id.etFirstName);
			etFirstName.Text = ControllerHelper.GetPreference (this, SystemResources.FirstName, string.Empty);
			etFirstName.SetSelection (etFirstName.Length ());
			etLastName = FindViewById<EditText> (Resource.Id.etLastName);
			etLastName.Text = ControllerHelper.GetPreference (this, SystemResources.LastName, string.Empty);

			tvOrigin = FindViewById<TextView> (Resource.Id.tvOrigin);
			tvOrigin.Text = ControllerHelper.GetPreference (this, SystemResources.OriginName, string.Empty);
			Constants.OriginId = ControllerHelper.GetPreference (this, SystemResources.OriginId, 0);
			tvOccupation = FindViewById<TextView> (Resource.Id.tvOccupation);
			Constants.OccupationId = ControllerHelper.GetPreference (this, SystemResources.OccupationId, 0);
			tvOccupation.Text = ControllerHelper.GetPreference (this, SystemResources.OccupationName, string.Empty);

			tvMale = FindViewById<TextView> (Resource.Id.tvMale);
			tvMale.Click += (sender, e) => {
				GenderTypeId = (int)Gender.Male;
				ClearGenderTabs ((int)Gender.Male);
			};
			tvFemale = FindViewById<TextView> (Resource.Id.tvFemale);
			tvFemale.Click += (sender, e) => {
				ClearGenderTabs ((int)Gender.Female);
				GenderTypeId = (int)Gender.Female;
			};
			tvOther = FindViewById<TextView> (Resource.Id.tvOther);
			tvOther.Click += (sender, e) => {
				ClearGenderTabs ((int)Gender.Other);
				GenderTypeId = (int)Gender.Other;
			};

			GenderTypeId = ControllerHelper.GetPreference (this, SystemResources.Gender, 0);
			if (GenderTypeId > 0)
				ClearGenderTabs (GenderTypeId);
			else
				ClearGenderTabs ((int)Gender.Male);

			tvLessThan18 = FindViewById<TextView> (Resource.Id.tvLessThan18);
			tvLessThan18.Click += (sender, e) => {
				AgeValue = tvLessThan18.Text;
				ClearAgeTabs (AgeValue);
			};
			tv18To28 = FindViewById<TextView> (Resource.Id.tv18To28);
			tv18To28.Click += (sender, e) => {
				AgeValue = tv18To28.Text;
				ClearAgeTabs (AgeValue);
			};
			tv29To39 = FindViewById<TextView> (Resource.Id.tv29To39);
			tv29To39.Click += (sender, e) => {
				ClearAgeTabs (AgeValue);
				AgeValue = tv29To39.Text;
			};
			tv40To50 = FindViewById<TextView> (Resource.Id.tv40To50);
			tv40To50.Click += (sender, e) => {
				AgeValue = tv40To50.Text;
				ClearAgeTabs (AgeValue);
			};
			tv51To61 = FindViewById<TextView> (Resource.Id.tv51To61);
			tv51To61.Click += (sender, e) => {
				AgeValue = tv51To61.Text;
				ClearAgeTabs (AgeValue);
			};
			tvGreaterTo61 = FindViewById<TextView> (Resource.Id.tvGreaterTo61);
			tvGreaterTo61.Click += (sender, e) => {
				AgeValue = tvGreaterTo61.Text;
				ClearAgeTabs (AgeValue);
			};
			AgeValue = ControllerHelper.GetPreference (this, SystemResources.Age, string.Empty);
			if (!string.IsNullOrEmpty (AgeValue))
				ClearAgeTabs (AgeValue);
			else {
				AgeValue = Constants.DefaultAge;
				ClearAgeTabs (AgeValue);
			}

			string[] lengths = new string[]{ "Test", "Test 1", "Test 2", "Test 3" };
			string[] occupations = new string[]{ "Test", "Test 1", "Test 2", "Test 3" };
			var rlOrigin = FindViewById<RelativeLayout> (Resource.Id.rlOrigin);
			rlOrigin.Click += (sender, e) => {
				Intent intent = new Intent (this, typeof(TestActivity));
				if (lengths != null)
					intent.PutExtra ("origin", JsonConvert.SerializeObject (lengths));
				else
					intent.PutExtra ("origin", string.Empty);
				StartActivityForResult (intent, 1000);
			};
			var rlOccupation = FindViewById<RelativeLayout> (Resource.Id.rlOccupation);
			rlOccupation.Click += (sender, e) => {
				Intent intent = new Intent (this, typeof(TestActivity));
				if (occupations != null)
					intent.PutExtra ("occupation", JsonConvert.SerializeObject (occupations));
				else
					intent.PutExtra ("occupation", string.Empty);
				StartActivityForResult (intent, 1000);
			};

			var btnCancel = FindViewById<Button> (Resource.Id.btnCancel);
			btnCancel.Click += delegate {
				this.Finish ();
			};
			var btnSave = FindViewById<Button> (Resource.Id.btnSave);
			btnSave.Click += async delegate {
				await Save ();
			};
			var imgLeft = FindViewById<ImageView> (Resource.Id.imgLeft);
			imgLeft.Click += delegate {
				this.Finish ();
			};
		}

		#endregion

		#region On Activity result callback

		protected override void OnActivityResult (int requestCode, Android.App.Result resultCode, Intent data)
		{
			base.OnActivityResult (requestCode, resultCode, data);
			if (resultCode == Android.App.Result.Ok) {
				if (!string.IsNullOrEmpty (data.GetStringExtra ("origin"))) {
					tvOrigin.Text = data.GetStringExtra ("origin");
				} else if (!string.IsNullOrEmpty (data.GetStringExtra ("occupation"))) {
					tvOccupation.Text = data.GetStringExtra ("occupation");
				}
			}
		}

		#endregion

		#region Save Profile Info Click

		async Task Save ()
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
			} else if (Constants.OriginId == 0)
				Toast.MakeText (this, "You must select country of origin", ToastLength.Long).Show ();
			else if (Constants.OccupationId == 0)
				Toast.MakeText (this, "You must select occupation", ToastLength.Long).Show ();

			ControllerHelper.ShowProgressDialog (this, Constants.Processing);
			PersonalInfo objUser = new PersonalInfo ();

			objUser.FirstName = SecurityClass.EncryptAes (etFirstName.Text.Trim ());
			objUser.LastName = SecurityClass.EncryptAes (etLastName.Text.Trim ());
			objUser.Gender = GenderTypeId;
			objUser.Age = AgeValue;
			objUser.BirthPlace = Constants.OriginId;
			objUser.Profession = Constants.OccupationId;
			objUser.UserId = Constants.LaborId;
			JornaleroResponse<int> objResp = new JornaleroResponse<int> ();
			ServiceHandler.PostData<JornaleroResponse<int>, PersonalInfo> (Constants.UpdateUserInfo, HttpMethod.Post,
				objUser).ContinueWith ((completed) => {
				try {
					if (!completed.IsFaulted) {
						objResp = completed.Result;
						if (objResp != null) {
							if (objResp.status_code == (int)HttpStatusCode.OK) {
								Toast.MakeText (this, "Profile updated successfully", ToastLength.Long).Show ();
								ControllerHelper.SetPreference (this, SystemResources.FirstName, etFirstName.Text);
								ControllerHelper.SetPreference (this, SystemResources.LastName, etLastName.Text);
								ControllerHelper.SetPreference (this, SystemResources.Gender, GenderTypeId);
								ControllerHelper.SetPreference (this, SystemResources.Age, AgeValue);
								ControllerHelper.SetPreference (this, SystemResources.ProfileCompleteness, 100);
								ControllerHelper.SetPreference (this, SystemResources.OriginId, Constants.OriginId);
								ControllerHelper.SetPreference (this, SystemResources.OriginName, tvOrigin.Text);
								ControllerHelper.SetPreference (this, SystemResources.OccupationId, Constants.OccupationId);
								ControllerHelper.SetPreference (this, SystemResources.OccupationName, tvOccupation.Text);
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

		#region Change color of tabs when switch

		private void ClearGenderTabs (int position)
		{
			switch (position) {
			case 1:
				tvMale.SetTextColor (Color.White);
				tvMale.SetBackgroundColor (Color.ParseColor (Constants.select_color));
				tvFemale.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvFemale.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tvOther.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvOther.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				break;
			case 2:
				tvFemale.SetTextColor (Color.White);
				tvFemale.SetBackgroundColor (Color.ParseColor (Constants.select_color));
				tvMale.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvMale.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tvOther.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvOther.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				break;
			case 3:
				tvOther.SetTextColor (Color.White);
				tvOther.SetBackgroundColor (Color.ParseColor (Constants.select_color));
				tvMale.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvMale.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tvFemale.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvFemale.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				break;
			}
		}

		private void ClearAgeTabs (string Age)
		{
			switch (Age) {
			case "<18":
				tvLessThan18.SetTextColor (Color.White);
				tvLessThan18.SetBackgroundColor (Color.ParseColor (Constants.select_color));
				tv18To28.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv18To28.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv29To39.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv29To39.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv40To50.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv40To50.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv51To61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv51To61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tvGreaterTo61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvGreaterTo61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				break;
			case "18-28":
				tv18To28.SetTextColor (Color.White);
				tv18To28.SetBackgroundColor (Color.ParseColor (Constants.select_color));
				tvLessThan18.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvLessThan18.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv29To39.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv29To39.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv40To50.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv40To50.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv51To61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv51To61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tvGreaterTo61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvGreaterTo61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				break;
			case "29-39":
				tv29To39.SetTextColor (Color.White);
				tv29To39.SetBackgroundColor (Color.ParseColor (Constants.select_color));
				tvLessThan18.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvLessThan18.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv18To28.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv18To28.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv40To50.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv40To50.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv51To61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv51To61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tvGreaterTo61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvGreaterTo61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				break;
			case "40-50":
				tv40To50.SetTextColor (Color.White);
				tv40To50.SetBackgroundColor (Color.ParseColor (Constants.select_color));
				tvLessThan18.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvLessThan18.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv18To28.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv18To28.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv29To39.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv29To39.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv51To61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv51To61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tvGreaterTo61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvGreaterTo61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				break;
			case "51-61":
				tv51To61.SetTextColor (Color.White);
				tv51To61.SetBackgroundColor (Color.ParseColor (Constants.select_color));
				tvLessThan18.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvLessThan18.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv18To28.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv18To28.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv40To50.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv40To50.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv29To39.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv29To39.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tvGreaterTo61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvGreaterTo61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				break;
			case "61>":
				tvGreaterTo61.SetTextColor (Color.White);
				tvGreaterTo61.SetBackgroundColor (Color.ParseColor (Constants.select_color));
				tvLessThan18.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tvLessThan18.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv18To28.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv18To28.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv40To50.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv40To50.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv51To61.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv51To61.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				tv29To39.SetTextColor (Color.ParseColor (Constants.dark_grey));
				tv29To39.SetBackgroundColor (Color.ParseColor (Constants.sky_blue_input));
				break;
			}
		}

		#endregion
	}
}