using System.Text;
using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;

//VERY VERY VERY IMPORTANT NOTE!!!!
// Your package name MUST NOT start with an uppercase letter.
// Android does not allow permissions to start with an upper case letter
// If it does you will get a very cryptic error in logcat and it will not be obvious why you are crying!
// So please, for the love of all that is kind on this earth, use a LOWERCASE first letter in your Package Name!!!!
using Android.Media;
using Android.Support.V4.App;
using Jornalero.Core;

namespace Jornalero.droid
{
	//You must subclass this!
	[BroadcastReceiver (Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
	[IntentFilter (new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter (new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter (new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
	public class GcmBroadcastReceiver : GcmBroadcastReceiverBase<PushHandlerService>
	{
		//IMPORTANT: Change this to your own Sender ID!
		//The SENDER_ID is your Google API Console App Project ID.
		//  Be sure to get the right Project ID from your Google APIs Console.  It's not the named project ID that appears in the Overview,
		//  but instead the numeric project id in the url: eg: https://code.google.com/apis/console/?pli=1#project:785671162406:overview
		//  where 785671162406 is the project id, which is the SENDER_ID to use!
		public static string[] SENDER_IDS = new string[] { "568697004811" };

		public const string TAG = "PushSharp-GCM";
	}

	[Service] //Must use the service tag
	public class PushHandlerService : GcmServiceBase
	{
		public PushHandlerService () : base (GcmBroadcastReceiver.SENDER_IDS)
		{
		}

		const string TAG = "GCM-SAMPLE";

		protected override void OnRegistered (Context context, string registrationId)
		{
			Log.Verbose (TAG, "GCM Registered: " + registrationId);
			ControllerHelper.SetPreference (this, string.Empty, registrationId);
			//ServiceHelper.ServiceApi<string, string> (Constants.pushNotification + ControllerHelper.GetPreference (this, SystemResources.UserId, string.Empty) + "/" + registrationId + "/" + DeviceType.Android.ToString (), HttpMethod.Post, string.Empty);
		}

		protected override void OnUnRegistered (Context context, string registrationId)
		{
			Log.Verbose (TAG, "GCM Unregistered: " + registrationId);
		}

		protected override void OnMessage (Context context, Intent intent)
		{
			Log.Info (TAG, "GCM Message Received!");

			string msg = string.Empty;

			if (intent != null && intent.Extras != null) {
				msg = intent.Extras.Get ("notification_data").ToString ();
			}

			Intent resultIntent = new Intent (this, typeof(MainActivity));
			Android.App.TaskStackBuilder stackBuilder = Android.App.TaskStackBuilder.Create (this);
			stackBuilder.AddParentStack (Java.Lang.Class.FromType (typeof(SplashActivity)));
			stackBuilder.AddNextIntent (resultIntent);
			PendingIntent resultPendingIntent = stackBuilder.GetPendingIntent (0, PendingIntentFlags.UpdateCurrent);

			Android.Net.Uri soundUri = RingtoneManager.GetDefaultUri (RingtoneType.Notification);
			NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder (this)
				.SetAutoCancel (true)
				.SetContentIntent (resultPendingIntent)
				.SetSound (soundUri)
				.SetContentTitle (Jornalero.Core.Constants.Jornalero)
				.SetContentText (msg);

			var notificationManager = (NotificationManager)GetSystemService (Context.NotificationService);
			notificationManager.Notify (0, notificationBuilder.Build ());
		}

		protected override bool OnRecoverableError (Context context, string errorId)
		{
			Log.Warn (TAG, "Recoverable Error: " + errorId);

			return base.OnRecoverableError (context, errorId);
		}

		protected override void OnError (Context context, string errorId)
		{
			Log.Error (TAG, "GCM Error: " + errorId);
		}
	}
}