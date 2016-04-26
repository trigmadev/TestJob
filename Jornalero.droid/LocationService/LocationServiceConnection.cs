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

namespace Jornalero.droid
{
	public class LocationServiceConnection : Java.Lang.Object, IServiceConnection
	{
		#region Event Argument

		public event EventHandler<ServiceConnectedEventArgs> ServiceConnected = delegate { };

		#endregion

		#region Constructor

		public LocationServiceConnection (LocationServiceBinder binder)
		{
			if (binder != null) {
				this.binder = binder;
			}
		}

		#endregion

		#region Binder

		protected LocationServiceBinder binder;

		public LocationServiceBinder Binder {
			get { return this.binder; }
			set { this.binder = value; }
		}

		#endregion

		#region Service Connnected

		public void OnServiceConnected (ComponentName name, IBinder service)
		{
			LocationServiceBinder serviceBinder = service as LocationServiceBinder;
			if (serviceBinder != null) {
				this.binder = serviceBinder;
				this.binder.IsBound = true;
				this.ServiceConnected (this, new ServiceConnectedEventArgs () { Binder = service });
				serviceBinder.Service.StartLocationUpdates ();
			}
		}

		#endregion

		#region Service Disconnected

		public void OnServiceDisconnected (ComponentName name)
		{
			this.binder.IsBound = false;
		}

		#endregion
	}

	#region Service Connected Event

	public class ServiceConnectedEventArgs : EventArgs
	{
		public IBinder Binder { get; set; }
	}

	#endregion
}