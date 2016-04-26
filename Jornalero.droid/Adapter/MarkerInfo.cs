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
using Android.Gms.Maps;
using Android.Gms.Maps.Model;

namespace Jornalero.droid
{
	public class MarkerInfo : Java.Lang.Object, GoogleMap.IInfoWindowAdapter
	{
		private LayoutInflater _layoutInflater;

		public MarkerInfo (LayoutInflater inflater)
		{
			_layoutInflater = inflater;
		}

		public View GetInfoWindow (Marker marker)
		{
			return null;
		}

		public View GetInfoContents (Marker marker)
		{
			var customPopup = _layoutInflater.Inflate (Resource.Layout.MarkerWindow, null);
			var titleTextView = customPopup.FindViewById<TextView> (Resource.Id.txtLat);
			if (titleTextView != null) {
				titleTextView.Text = marker.Title;
			}
			var snippetTextView = customPopup.FindViewById<TextView> (Resource.Id.txtLong);
			if (snippetTextView != null) {
				snippetTextView.Text = marker.Snippet;
			}
			return customPopup;
		}
	}
}