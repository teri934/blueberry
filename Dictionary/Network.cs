using Android.App;
using Android.Content;
using Android.Net;
using Plugin.Connectivity;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dictionary
{
	public class Receiver : BroadcastReceiver
	{
		public override void OnReceive(Context context, Intent intent)
		{
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
			Network[] info = cm.GetAllNetworks();  //active in java
			if (info != null)
			{
				for (int i = 0; i < info.Length; i++)
				{
					NetworkCapabilities nc = cm.GetNetworkCapabilities(info[i]);
					if(nc.HasTransport(TransportType.Wifi))
					{
						status = "Wifi enabled";
						return status;
					}
					else if (nc.HasTransport(TransportType.Wifi))  //different mobile data
					{
						status = "Mobile data enabled";
						return status;
					}
				}
			}
			else
			{
				status = "No internet is available";
				return status;
			}

			return status;
		}

	}
}