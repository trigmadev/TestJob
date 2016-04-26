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
	public class LocationServiceBinder : Binder
	{
		#region Properties

		public bool IsBound { get; set; }

		public LocationService Service {
			get { return this.service; }
		}

		protected LocationService service;

		#endregion

		#region Constructor

		public LocationServiceBinder (LocationService service)
		{
			this.service = service;
		}

		#endregion
	}
}