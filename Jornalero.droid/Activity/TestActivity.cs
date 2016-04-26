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
using Newtonsoft.Json;
using Jornalero.Core;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Jornalero.droid
{
	[Activity (Label = "TestActivity")]			
	public class TestActivity : Activity
	{
		protected LayoutInflater Inflater;
		string[] origins = null;
		string[] occupations = null;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Test);
			Inflater = (LayoutInflater)this.GetSystemService (Context.LayoutInflaterService);
			if (!string.IsNullOrEmpty (Intent.GetStringExtra ("origin"))) {
				FindViewById<TextView> (Resource.Id.tvTitle).SetText (Resource.String.country_of_origin);
				GetBirthPlaceList ();
			} else if (!string.IsNullOrEmpty (Intent.GetStringExtra ("occupation"))) {
				FindViewById<TextView> (Resource.Id.tvTitle).SetText (Resource.String.occupation);
				GetOccupationList ();
			}
			var imgLeft = FindViewById<ImageView> (Resource.Id.imgLeft);
			imgLeft.SetImageResource (Resource.Drawable.cross_white);
			imgLeft.Click += delegate {
				this.Finish ();
			};
		}

		#region Get Birth Place & Occupation List

		private void GetBirthPlaceList ()
		{
			ControllerHelper.ShowProgressDialog (this, Constants.Processing);
			JornaleroResponse<List<BirthPlace>> objResp = new JornaleroResponse<List<BirthPlace>> ();
			ServiceHandler.PostData<JornaleroResponse<List<BirthPlace>>, string> (Constants.GetBirthPlaceList, HttpMethod.Get,
				string.Empty).ContinueWith ((completed) => {
				try {
					if (!completed.IsFaulted) {
						objResp = completed.Result;
						if (objResp != null && objResp.status_code == (int)HttpStatusCode.OK && objResp.data != null) {
							origins = new string[objResp.data.Count];
							for (int i = 0; i < objResp.data.Count; i++) {
								origins [i] = objResp.data [i].Name;
							}
							var listView = FindViewById<ListView> (Resource.Id.listView);
							View view = Inflater.Inflate (Resource.Layout.TestItem, null, false);
							var tvTitle = view.FindViewById<TextView> (Resource.Id.tvTitle);
							tvTitle.Typeface = ControllerHelper.SetJornaleroFont (this);
							if (origins != null && origins.Count () > 0)
								listView.Adapter = new ArrayAdapter<string> (this, Resource.Layout.TestItem, origins);
							listView.ItemClick += (lsender, le) => {
								var origin = origins != null ? origins [le.Position] : origins [le.Position];
								Constants.OriginId = objResp.data.Where (x => x.Name == origin).FirstOrDefault ().BirthPlaceId;
								Console.WriteLine (origin);
								Intent intent = new Intent (this, typeof(PersonalInfoActivity));
								intent.PutExtra ("origin", origin);
								SetResult (Android.App.Result.Ok, intent);
								Finish ();
							};
						} else
							Toast.MakeText (this, objResp.message, ToastLength.Long).Show ();
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

		private void GetOccupationList ()
		{
			ControllerHelper.ShowProgressDialog (this, Constants.Processing);
			JornaleroResponse<List<Profession>> objResp = new JornaleroResponse<List<Profession>> ();
			ServiceHandler.PostData<JornaleroResponse<List<Profession>>, string> (Constants.GetProfessionList, HttpMethod.Get,
				string.Empty).ContinueWith ((completed) => {
				try {
					if (!completed.IsFaulted) {
						objResp = completed.Result;
						if (objResp != null && objResp.status_code == (int)HttpStatusCode.OK && objResp.data != null) {
							occupations = new string[objResp.data.Count];
							for (int i = 0; i < objResp.data.Count; i++) {
								occupations [i] = objResp.data [i].Name;
							}
							var listView = FindViewById<ListView> (Resource.Id.listView);
							View view = Inflater.Inflate (Resource.Layout.TestItem, null, false);
							var tvTitle = view.FindViewById<TextView> (Resource.Id.tvTitle);
							tvTitle.Typeface = ControllerHelper.SetJornaleroFont (this);
							if (occupations != null && occupations.Count () > 0)
								listView.Adapter = new ArrayAdapter<string> (this, Resource.Layout.TestItem, occupations);
							listView.ItemClick += (lsender, le) => {
								var occupation = occupations != null ? occupations [le.Position] : occupations [le.Position];
								Constants.OccupationId = objResp.data.Where (x => x.Name == occupation).FirstOrDefault ().ProfessionId;
								Console.WriteLine (occupation);
								Intent intent = new Intent (this, typeof(PersonalInfoActivity));
								intent.PutExtra ("occupation", occupation);
								SetResult (Android.App.Result.Ok, intent);
								Finish ();
							};
						} else
							Toast.MakeText (this, objResp.message, ToastLength.Long).Show ();
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