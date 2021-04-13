using Android.App;
using Android.Content;
using Android.Net;
using Android.Provider;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Util;
using System.Text;

namespace Dictionary
{
	[BroadcastReceiver(Enabled = true)]
	[IntentFilter(new[] { "com.companyname.dictionary.Receiver.android.net.conn.CONNECTIVITY_CHANGE" })]
	public class Receiver : BroadcastReceiver
	{
		public const string action = "com.companyname.dictionary.Receiver.android.net.conn.CONNECTIVITY_CHANGE";
		public override void OnReceive(Context context, Intent intent)
		{
			Log.Debug("ONRECEIVE", "");
			string status = NetworkUtil.GetConnectivityStatusString(context);
			if (status == null || status.Length == 0)  //empty string, probably in Java IsEmpty()
			{
				status = "No Internet Connection";
			}
			Toast.MakeText(context, status, ToastLength.Long).Show();
		}
	}

	class NetworkUtil
	{
		public static string GetConnectivityStatusString(Context context)
		{
			string status = null;
			ConnectivityManager cm = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
			Network network = cm.ActiveNetwork;
			if (network != null && !IsAirplaneModeOn(context))  //on my phone still wifi, airpane mode
			{
				NetworkCapabilities nc = cm.GetNetworkCapabilities(network);
				if(nc.HasTransport(TransportType.Wifi))
				{
					status = "Wifi enabled";
					return status;
				}
				else if (nc.HasTransport(TransportType.Cellular))
				{
					status = "Mobile data enabled";
					return status;
				}

			}
			else
			{
				status = "No internet is available";
				return status;
			}

			return status;
		}

		static bool IsAirplaneModeOn(Context context)
		{
			return Settings.Global.GetInt(context.ContentResolver,Settings.Global.AirplaneModeOn, 0) != 0;
		}

	}
}