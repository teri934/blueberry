using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PP = Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using Plugin.CurrentActivity;
using Xamarin.Forms;

namespace Dictionary.Database
{
	static class DatabaseFileManager
	{
        const string message = "In order to save and extract your results the permission for storage access is needed.";
        const string results_list = "results_list";

        public static void GetDatabaseToDownloads()
		{
            string basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string path = Path.Combine(basePath, "results.xml");

            var list = Task.Run(() => GetDatabaseResults()).GetAwaiter().GetResult();
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

        /// <summary>
        /// checks file in downloads folder, deserializes it and updates the database table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Synchronize(List<Result> list)
		{
            Task.Run(() => ResultsDatabase.Database.DropTableAsync<Result>()).GetAwaiter();  
            Task.Run(() => ResultsDatabase.Database.CreateTableAsync<Result>()).GetAwaiter();

            //TODO insert the list records
        }

        private static async Task<List<Result>> GetDatabaseResults()
		{
            MainActivity activity = (MainActivity)CrossCurrentActivity.Current.Activity;
            ResultsDatabase database = (ResultsDatabase)activity.GetType().GetField("database", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(activity);

            return await database.GetResultsAsync();
        }

        //        using (var reader = new StreamReader(path))
        //        {
        //            string line;
        //            while ((line = reader.ReadLine()) != null)
        //{
        //                Console.WriteLine();
        //}
        //        }
    }
}