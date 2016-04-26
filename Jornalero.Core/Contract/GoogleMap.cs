using System;
using System.Collections.Generic;

namespace Jornalero.Core
{
	public class GPSLocation
	{
		public double lat { get; set; }

		public double lng { get; set; }
	}

	public class Photo
	{
		public int height { get; set; }

		public List<object> html_attributions { get; set; }

		public string photo_reference { get; set; }

		public int width { get; set; }
	}

	public class OpeningHours
	{
		public bool open_now { get; set; }

		public List<object> weekday_text { get; set; }
	}

	public class Geometry
	{
		public GPSLocation location { get; set; }
	}

	public class Result
	{
		public string adr_address { get; set; }

		public string formatted_address { get; set; }

		public Geometry geometry { get; set; }

		public string icon { get; set; }

		public string id { get; set; }

		public string name { get; set; }

		public string place_id { get; set; }

		public string reference { get; set; }

		public string scope { get; set; }

		public List<string> types { get; set; }

		public string url { get; set; }

		public string vicinity { get; set; }

		public List<AddressComponent> address_components { get; set; }

		public OpeningHours opening_hours { get; set; }
	}

	public class AddressComponent
	{
		public string long_name { get; set; }

		public string short_name { get; set; }

		public List<string> types { get; set; }
	}

	public class MapItems
	{
		public double Lat { get; set; }

		public double Long { get; set; }

		public string Name { get; set; }

		public string Address { get; set; }
	}

	public class Prediction
	{
		public string description { get; set; }

		public string id { get; set; }

		public string place_id { get; set; }

		public string reference { get; set; }

		public List<string> types { get; set; }
	}

	public class RootObject
	{
		public List<Prediction> predictions { get; set; }

		public string status { get; set; }

		public List<string> html_attributions { get; set; }

		public string next_page_token { get; set; }

		public List<Result> results { get; set; }
	}
}

namespace Jornalero.Place
{
	#region Location from PlaceID

	public class AddressComponent
	{
		public string long_name { get; set; }

		public string short_name { get; set; }

		public List<string> types { get; set; }
	}

	public class Location
	{
		public double lat { get; set; }

		public double lng { get; set; }
	}

	public class Geometry
	{
		public Location location { get; set; }
	}

	public class Result
	{
		public List<AddressComponent> address_components { get; set; }

		public string adr_address { get; set; }

		public string formatted_address { get; set; }

		public Geometry geometry { get; set; }

		public string icon { get; set; }

		public string id { get; set; }

		public string name { get; set; }

		public string place_id { get; set; }

		public string reference { get; set; }

		public string scope { get; set; }

		public List<string> types { get; set; }

		public string url { get; set; }

		public int utc_offset { get; set; }

		public string vicinity { get; set; }

		public string website { get; set; }
	}

	public class RootObject
	{
		public List<object> html_attributions { get; set; }

		public Result result { get; set; }

		public string status { get; set; }
	}

	#endregion
}

namespace Jornalero.MapDelegate
{
	public class Northeast
	{
		public double lat { get; set; }

		public double lng { get; set; }
	}

	public class Southwest
	{
		public double lat { get; set; }

		public double lng { get; set; }
	}

	public class Bounds
	{
		public Northeast northeast { get; set; }

		public Southwest southwest { get; set; }
	}

	public class Distance
	{
		public string text { get; set; }

		public int value { get; set; }
	}

	public class Duration
	{
		public string text { get; set; }

		public int value { get; set; }
	}

	public class EndLocation
	{
		public double lat { get; set; }

		public double lng { get; set; }
	}

	public class StartLocation
	{
		public double lat { get; set; }

		public double lng { get; set; }
	}

	public class Distance2
	{
		public string text { get; set; }

		public int value { get; set; }
	}

	public class Duration2
	{
		public string text { get; set; }

		public int value { get; set; }
	}

	public class EndLocation2
	{
		public double lat { get; set; }

		public double lng { get; set; }
	}

	public class Polyline
	{
		public string points { get; set; }
	}

	public class StartLocation2
	{
		public double lat { get; set; }

		public double lng { get; set; }
	}

	public class Step
	{
		public Distance2 distance { get; set; }

		public Duration2 duration { get; set; }

		public EndLocation2 end_location { get; set; }

		public string html_instructions { get; set; }

		public Polyline polyline { get; set; }

		public StartLocation2 start_location { get; set; }

		public string travel_mode { get; set; }

		public string maneuver { get; set; }
	}

	public class Leg
	{
		public Distance distance { get; set; }

		public Duration duration { get; set; }

		public string end_address { get; set; }

		public EndLocation end_location { get; set; }

		public string start_address { get; set; }

		public StartLocation start_location { get; set; }

		public List<Step> steps { get; set; }

		public List<object> via_waypoint { get; set; }
	}

	public class OverviewPolyline
	{
		public string points { get; set; }
	}

	public class Route
	{
		public Bounds bounds { get; set; }

		public string copyrights { get; set; }

		public List<Leg> legs { get; set; }

		public OverviewPolyline overview_polyline { get; set; }

		public string summary { get; set; }

		public List<object> warnings { get; set; }

		public List<object> waypoint_order { get; set; }
	}

	public class RootObject
	{
		public List<Route> routes { get; set; }

		public string status { get; set; }
	}
}