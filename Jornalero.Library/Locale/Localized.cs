using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using Jornalero.Library.Resources;

namespace Jornalero.Library
{
	public static class Localized
	{
		public static void SetCurrentLocale (string locale)
		{
			if (string.IsNullOrWhiteSpace (locale))
				locale = "en-US";

			var language = locale.Substring (0, 2);
			
			switch (language.ToLower ()) {
			case "en":
				CurrentLocale = LocalizationItems.English;
				break;
			case "es":
				CurrentLocale = LocalizationItems.Spanish;
				break;
			default:
				break;
			}
		}

		static Localized ()
		{
			CurrentLocale = LocalizationItems.None;
		}

		private const string STRINGS_ROOT = "Jornalero.Library.Resources";
		//private static string manifest;
		//private static string[] manifests;
		private static ResourceManager manager;

		public static LocalizationItems CurrentLocale { get; set; }

		public static string GetLocaleItem (int localeID)
		{
			if (CurrentLocale == LocalizationItems.None)
				CurrentLocale = LocalizationItems.English;

			if (manager != null)
				return manager.GetString (localeID.ToString ());

			switch (CurrentLocale) {
			case LocalizationItems.English:
				return GetLocaleTypeString (localeID, typeof(English).GetTypeInfo ().Assembly, string.Format ("{0}.English", STRINGS_ROOT));
			case LocalizationItems.Spanish:
				return GetLocaleTypeString (localeID, typeof(Spanish).GetTypeInfo ().Assembly, string.Format ("{0}.Spanish", STRINGS_ROOT));
			default:
				return "";
			}
		}

		static string GetLocaleTypeString (int locID, Assembly assembly, string localeRes)
		{
			manager = new ResourceManager (localeRes, assembly);
			string localeValue = manager.GetString (locID.ToString ());
			if (string.IsNullOrEmpty (localeValue)) {
				//if other locale value is empty then It will return default English locale. I saw, few emty string in russion lang.
				var mManager = new ResourceManager (string.Format ("{0}.English", STRINGS_ROOT), typeof(English).GetTypeInfo ().Assembly);
				return mManager.GetString (locID.ToString ());
			}
			return localeValue;
		}
	}

	public enum LocalizationItems
	{
		English = 0,
		Spanish,
		None
	}
}