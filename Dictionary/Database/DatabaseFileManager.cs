using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PP = Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Android.Widget;
using Android.Content;
using SQLitePCL;

namespace Dictionary.Database
{
	static class DatabaseFileManager
	{
        const string message = "In order to save and extract your results the permission for storage access is needed.";
        const string results_list = "results_list";
        public const string xml_file = "blueberry_results.xml";

		/// <summary>
		/// serializes the database and copies it to Downloads folder
		/// </summary>
		public static async void GetDatabaseToDownloads()
		{
            string basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(basePath, xml_file);

            ResultsDatabase database = GetInstanceDatabase();
            var list = await database.GetResultsAsync();
            Serialize(path, list);

            bool value = await CheckPermission();

            if(value)
			{
                #pragma warning disable 618
                //the file should be stored in a public directory for user's convenience
                //other way it would be stored in app's folder which would be removed after uninstalling the app
                var folder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                string destinationPath = folder.Path;
                File.Copy(path, destinationPath, true);

                Toast.MakeText(Android.App.Application.Context, MainActivity.GetLocalString("@string/toast_generate"), ToastLength.Short).Show();
            }
        }

        public static async Task<bool> CheckPermission()
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
                return true;
            }

            return false;
        }

        private static void Serialize(string path, List<Result> list)
		{
            using (var writer = new FileStream(path, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Result>), new XmlRootAttribute(results_list));
                serializer.Serialize(writer, list);
            }
        }

        public static List<Result> Deserialize(string path)
		{
            List<Result> list;
            using (var reader = new StreamReader(path))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(List<Result>), new XmlRootAttribute(results_list));
                list = (List<Result>)deserializer.Deserialize(reader);
            }

            return list;
        }

        public static async void Synchronize(List<Result> list)
        {
            ResultsDatabase database = GetInstanceDatabase();

            await database.DeleteTableAsync();  

			await database.FillTableAsync(list);
		}

        private static ResultsDatabase GetInstanceDatabase()
		{
            MainActivity activity = (MainActivity)CrossCurrentActivity.Current.Activity;
            ResultsDatabase database = (ResultsDatabase)activity.GetType().GetField("database", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(activity);

            return database;
        }
    }
}