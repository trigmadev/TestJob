using System;

namespace Jornalero.Core
{
	public class Constants
	{
		public const string KeyPass = "jornalero";
		public const string Keysalt = "test123#";
		public const string GoogleMapServerKey = "AIzaSyDWq9fFIh3IxuqGhBLLHNoG6btEYuKO9gI";
		public const string GoogleMapiOSKey = "AIzaSyAMxWiOn8b3WbJTaGIIt6gkn6eeDx-JW3Y";
		public const string BroadcastMyLocationKey = "broadcastLocation";
		public const string EnryptionKey = "5k3f4467Gd117r$$#194hdlj23987hxJKL969#ghf0%96kcj$n44nRkjg";
		public const string WebApiUATUrl = "http://trigmasolutions.com/Jornalero/api/Jornalero/";
		public const string ErrorMessageEnglish = "Something bad happen! Please try again.";
		public const string LogoutAlert = "Are you sure you want to logout?";
		public const string DeviceToken = "jornalero";
		public const string JornaleroPreferences = "JornaleroPreferences";
		public const string Jornalero = "Jornalero";
		public const string DefaultAge = "18-28";
		public const int ZoomLevel = 13;

		public static byte[] MasterKey { get; set; }

		public static int LaborId { get; set; }

		public static int ProfileCompleteness { get; set; }

		public static int OriginId { get; set; }

		public static int OccupationId { get; set; }

		public static bool IsSignUp { get; set; }

		public static string JornaleroFontStyle {
			get {
				return "fonts/SF-UI-Text-Regular.ttf";
			}
		}

		public static string Processing {
			get {
				return "Processing...";
			}
		}

		public static string Posting {
			get {
				return "Posting...";
			}
		}

		public static string SigningUp {
			get {
				return "Signing up...";
			}
		}

		public static string Authenticating {
			get { 
				return "Authenticating...";
			}
		}

		public static string Saving {
			get {
				return "Saving...";
			}
		}

		public static string Uploading {
			get {
				return "Uploading...";
			}
		}

		public static string BlankMsg {
			get { 
				return "Can't be blank";
			}
		}

		#region Color Code

		public static string sky_blue_input {
			get { 
				return "#e7f0fa";
			}
		}

		public static string dark_grey {
			get { 
				return "#7d8793";
			}
		}

		public static string select_color {
			get { 
				return "#314661";
			}
		}

		public static string error_color {
			get { 
				return "#F7656D";
			}
		}

		#endregion

		#region API URLS

		public const string RegisterLaborUser = "RegisterLaborUser";
		public const string LaborUserLogin = "LaborUserLogin";
		public const string GetWorkCenterList = "GetWorkCenterList";
		public const string GetBirthPlaceList = "GetBirthPlaceList";
		public const string GetProfessionList = "GetProfessionList";
		public const string GetAreaList = "GetAreaList";
		public const string ChangePassword = "ChangePassword";
		public const string ChangeEmail = "ChangeEmail";
		public const string ChangePhoneNo = "ChangePhoneNo";
		public const string ForgotPassword = "ForgotPassword";
		public const string UpdateUserInfo = "UpdateUserInfo";

		#endregion
	}
}