using System;

namespace Jornalero.Core
{
	public enum NetworkStatus
	{
		NotReachable,
		ReachableViaCarrierDataNetwork,
		ReachableViaWiFiNetwork
	}

	public enum MenuOption
	{
		Alert,
		MyReport,
		JobLog,
		WorkerRights,
		MyAlert,
		AccountSettings,
		Logout
	}

	public enum DeviceType
	{
		iOS,
		Droid
	}

	public enum Gender : int
	{
		Male = 1,
		Female,
		Other
	}
}