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
using Android.Locations;

namespace Jornalero.droid
{
	/// <summary>
	/// Location service handler.
	/// </summary>
	[Service]
	public class LocationService : Service, ILocationListener
	{
		#region Location Manager

		protected LocationManager LocManager = Android.App.Application.Context.GetSystemService (LocationService) as LocationManager;

		#endregion

		#region Event Arguments

		public event EventHandler<LocationChangedEventArgs> LocationChanged = delegate { };
		public event EventHandler<ProviderDisabledEventArgs> ProviderDisabled = delegate { };
		public event EventHandler<ProviderEnabledEventArgs> ProviderEnabled = delegate { };
		public event EventHandler<StatusChangedEventArgs> StatusChanged = delegate { };

		#endregion

		#region OnBind

		public override IBinder OnBind (Intent intent)
		{
			var binder = new LocationServiceBinder (this);
			return binder;
		}

		#endregion

		#region OnStartCommandResult

		public override StartCommandResult OnStartCommand (Intent intent, StartCommandFlags flags, int startId)
		{
			return StartCommandResult.Sticky;
		}

		#endregion

		#region Location Start Update

		public void StartLocationUpdates ()
		{
			var locationCriteria = new Criteria ();
			locationCriteria.Accuracy = Accuracy.NoRequirement;
			locationCriteria.PowerRequirement = Power.NoRequirement;
			var locationProvider = LocManager.GetBestProvider (locationCriteria, true);

			LocManager.RequestLocationUpdates (locationProvider, 2000, 0, this);
		}

		#endregion

		#region OnDestroy

		public override void OnDestroy ()
		{
			base.OnDestroy ();
			LocManager.RemoveUpdates (this);
		}

		#endregion

		#region Implementation : ILocationListener

		public void OnLocationChanged (Location location)
		{
			this.LocationChanged (this, new LocationChangedEventArgs (location));
		}

		public void OnProviderDisabled (string provider)
		{
			this.ProviderDisabled (this, new ProviderDisabledEventArgs (provider));
		}

		public void OnProviderEnabled (string provider)
		{
			this.ProviderEnabled (this, new ProviderEnabledEventArgs (provider));
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
			this.StatusChanged (this, new StatusChangedEventArgs (provider, status, extras));
		}

		#endregion
	}
}