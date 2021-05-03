using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using PP = Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Xamarin.Forms;

namespace Dictionary
{
	static class DatabaseFileManager
	{
        const string message = "In order to save and extract your results the permission for storage access is needed.";

        public static void CopyFileDownloads()
		{
            //alright
            var basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var files = Directory.GetFiles(basePath);
        }

        public async static void Generate()
        {
            await CheckPermission();
        }

        private static async Task<bool> CheckPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            if (status != PP.PermissionStatus.Granted)
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(PP.Permission.Storage))
                {
                    await new Page().DisplayAlert("Need permission", message, "OK");
                }

                status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
            }

            if (status == PP.PermissionStatus.Granted)
            {
                Context context = CrossCurrentActivity.Current.AppContext;
                var folder2 = context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads);
                var ffff = await folder2.ListFilesAsync();

                var folder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                var files = await folder.ListFilesAsync();
                var f = Directory.GetFiles(folder.AbsolutePath);

                return true;
            }

            return false;
        }

        private static void Serialize()
		{

		}

        private static void Deserialize()
		{

		}
    }
}