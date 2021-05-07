using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PP = Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using SQLitePCL;

namespace Dictionary.Database
{
	static class DatabaseFileManager
	{
        const string message = "In order to save and extract your results the permission for storage access is needed.";
        const string results_list = "results_list";

        /// <summary>
        /// serializes the database and copies it to Downloads folder
        /// </summary>
        public static void GetDatabaseToDownloads()
		{
            string basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(basePath, "results.xml");

            ResultsDatabase database = GetInstanceDatabase();
            var list = Task.Run(() => database.GetResultsAsync()).GetAwaiter().GetResult();
            Serialize(path, list);

            //TODO copy file to downloads
            Task.Run(() => CheckPermission()).GetAwaiter().GetResult();
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
                //Context context = CrossCurrentActivity.Current.AppContext;
                //var folder2 = context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads);
                //var ffff = await folder2.ListFilesAsync();

                //var folder = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads);
                //var files = await folder.ListFilesAsync();
                //var f = Directory.GetFiles(folder.AbsolutePath);

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