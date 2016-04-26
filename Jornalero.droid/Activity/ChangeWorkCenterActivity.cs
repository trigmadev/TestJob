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
using Android.Locations;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Newtonsoft.Json;

namespace Jornalero.droid
{
	[Activity (Label = "Change Work Center", ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = (ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize))]		
	public class ChangeWorkCenterActivity : BaseActivity, IOnMapReadyCallback
	{
		#region Global variables

		MapFragment mapFrag;
		GoogleMap map;
		Location currentLocation;
		bool IsMapBind;
		Geocoder gc;
		List<Marker> markerList;

		#endregion

		#region Set Layout Resource

		protected override int LayoutResource {
			get {
				return Resource.Layout.ChangeWorkCenter;
			}
		}

		#endregion

		#region Create View, Initialize UI Controls & Events

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			//Check if location enabled
			if (!LocManager.IsProviderEnabled (LocationManager.NetworkProvider))
				ShowLocationDisableAlert ();

			//register location update service
			JornaleroApp.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
				JornaleroApp.Current.LocationService.LocationChanged += HandleLocationChanged;
				JornaleroApp.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
				JornaleroApp.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
				JornaleroApp.Current.LocationService.StatusChanged += HandleStatusChanged;
			};
			//Start service
			JornaleroApp.StartLocationService ();

			InitializeComponents ();

			mapFrag = (MapFragment)FragmentManager.FindFragmentById (Resource.Id.map);
			mapFrag.GetMapAsync (this);

			if (gc == null)
				gc = new Geocoder (this);
		}

		private void InitializeComponents ()
		{
			FindViewById<TextView> (Resource.Id.tvTitle).SetText (Resource.String.select_center_on_map);
			var imgLeft = FindViewById<ImageView> (Resource.Id.imgLeft);
			imgLeft.SetImageResource (Resource.Drawable.cross_white);
			imgLeft.Click += delegate {
				this.Finish ();
			};
		}

		#endregion

		#region Map Ready CallBack & Map Marker Bind

		public void OnMapReady (GoogleMap googleMap)
		{
			map = googleMap;
			map.MyLocationEnabled = true;
			map.UiSettings.ZoomControlsEnabled = true;
			map.MarkerClick += MapOnMarkerClick;
			currentLocation = new Location ("gps");
			BindMapMarker ();
		}

		#endregion

		#region Bind Map Marker and create Region bound

		void BindMapMarker ()
		{
			if (markerList == null)
				markerList = new List<Marker> ();
			JornaleroResponse<List<WorkCenter>> objResp = new JornaleroResponse<List<WorkCenter>> ();
			ServiceHandler.PostData<JornaleroResponse<List<WorkCenter>>, string> (Constants.GetWorkCenterList, HttpMethod.Get,
				string.Empty).ContinueWith ((completed) => {
				try {
					if (!completed.IsFaulted) {
						objResp = completed.Result;
						if (objResp != null && objResp.status_code == (int)HttpStatusCode.OK) {
							if (objResp.data != null && objResp.data.Count == 0)
								return;
							//Remove existing markers
							foreach (var item in markerList) {
								item.Remove ();
							}
							foreach (var item in objResp.data) {
								if (item.Latitude != null) {
									Location loc = new Location ("gps");
									loc.Latitude = Convert.ToDouble (item.Latitude);
									loc.Longitude = Convert.ToDouble (item.Longitude);
									//Get address of location
									//var addresses = gc.GetFromLocation (loc.Latitude, loc.Longitude, 1);
									//string address = "N/A";
									//if (addresses != null && addresses.Count > 0)
									//	Console.WriteLine ("Marker: " + address);
									//address = addresses [0].Locality + ", " + addresses [0].CountryName;
									//Set marker and its title
									MarkerOptions marker = new MarkerOptions ();
									marker.SetPosition (new LatLng (loc.Latitude, loc.Longitude));
									//var distance = currentLocation.DistanceTo (loc) / 1000;
									//var distanceFormat = distance <= 1 ? distance.ToString ("0.0") + " km away" : distance.ToString ("0.0") + " kms away";
									marker.SetTitle (item.Address);
									//marker.SetSnippet (distanceFormat);
									var markerNew = map.AddMarker (marker);
									markerList.Add (markerNew);
								}
							}
							//Set Info marker click to show Marker Information
							map.SetInfoWindowAdapter (new MarkerInfo (LayoutInflater));
						}
					}
				} catch (Exception ex) {
					Console.WriteLine (ex);
				}
			}, TaskScheduler.FromCurrentSynchronizationContext ());
		}

		#endregion

		#region Location Service Method Handling

		private void HandleStatusChanged (object sender, Android.Locations.StatusChangedEventArgs e)
		{
			//throw new NotImplementedException();
		}

		private void HandleProviderEnabled (object sender, Android.Locations.ProviderEnabledEventArgs e)
		{
			//throw new NotImplementedException();
		}

		private void HandleProviderDisabled (object sender, Android.Locations.ProviderDisabledEventArgs e)
		{
			//throw new NotImplementedException();
		}

		private void HandleLocationChanged (object sender, Android.Locations.LocationChangedEventArgs e)
		{
			//Console.WriteLine ("Laongitude {0} Latitude {1}", e.Location.Latitude, e.Location.Longitude);
			if (currentLocation.Latitude != e.Location.Latitude) {
				currentLocation = e.Location;
				if (map != null && !IsMapBind) {
					CameraPosition.Builder builder = CameraPosition.InvokeBuilder ();
					builder.Target (new LatLng (e.Location.Latitude, e.Location.Longitude));
					builder.Zoom (Constants.ZoomLevel);
					CameraPosition cameraPosition = builder.Build ();
					CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition (cameraPosition);
					IsMapBind = true;
					map.MapType = GoogleMap.MapTypeNormal;
					map.AnimateCamera (cameraUpdate);
				}
			}
		}

		#endregion

		#region marker click event and draw route between Current User and marker User

		private void MapOnMarkerClick (object sender, GoogleMap.MarkerClickEventArgs markerClickEventArgs)
		{
			markerClickEventArgs.Handled = true;
			Marker marker = markerClickEventArgs.Marker;
			if (marker != null) {
				Console.WriteLine (marker.Position);
				LatLng item = new LatLng (marker.Position.Latitude, marker.Position.Longitude);
				Location markerLocation = new Location ("gps");
				markerLocation.Latitude = Convert.ToDouble (item.Latitude);
				markerLocation.Longitude = Convert.ToDouble (item.Longitude);
				Toast.MakeText (this, markerLocation.Latitude.ToString () + " clicked", ToastLength.Long).Show ();
			}
		}

		#endregion
	}
}