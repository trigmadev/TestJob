using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using Fragment = Android.Support.V4.App.Fragment;

namespace Jornalero.droid
{
	public class AddVehicleFragment : Fragment
	{
		View view;

		public AddVehicleFragment ()
		{
			this.RetainInstance = true;
		}

		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (view == null) {
				view = inflater.Inflate (Resource.Layout.AddVehicle, container, false);
			}
			return view;
		}
	}
}

