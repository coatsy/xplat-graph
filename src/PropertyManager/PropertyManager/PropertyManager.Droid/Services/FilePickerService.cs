using System.Threading.Tasks;
using PropertyManager.Models;
using PropertyManager.Services;
using Android.App;
using Android.Content;
using System.IO;
using Android.Net;
using Android.Database;
using Android.Provider;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Java.Lang;

namespace PropertyManager.Droid.Services
{
    public class FilePickerService : IFilePickerService
    {
        public const int RequestCode = 1;

        private TaskCompletionSource<PickedFileModel> _taskCompletionSource;

        public Task<PickedFileModel> GetFileAsync()
        {
            // Get the top activity.
            var topActivity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

            if (topActivity == null)
            {
                throw new Exception("Could not find the top Activity.");
            }

            // Start activity.
            var intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");
            topActivity.StartActivityForResult(intent, RequestCode);

            // Create the task completion source.
            _taskCompletionSource = new TaskCompletionSource<PickedFileModel>();
            return _taskCompletionSource.Task;
        }

        public void ResolveTask(ContentResolver contentResolver, int requestCode, 
            Result resultCode, Intent data)
        {
            if (requestCode == RequestCode && resultCode == Result.Ok && data != null &&
                _taskCompletionSource != null)
            {
                Uri uri = data.Data;

                // Get the input stream, which by some reason cannot be seeked.
                // Workaround is to read the bytes and create a new stream.
                var inputStream = contentResolver.OpenInputStream(uri);
                var bytes = GetByteArray(inputStream);
                var stream = new MemoryStream(bytes);

                var path = GetRealPathFromUri(contentResolver, uri);
                var name = Path.GetFileName(path);

                // Complete the task.
                _taskCompletionSource.SetResult(new PickedFileModel
                {
                    Stream = stream,
                    Name = name
                });
                _taskCompletionSource = null;
            }
        }

        private string GetRealPathFromUri(ContentResolver contentResolver, Uri uri)
        {
            ICursor cursor = contentResolver.Query(uri, null, null, null, null);
            cursor.MoveToFirst();
            string documentId = cursor.GetString(0);
            documentId = documentId.Split(':')[1];
            cursor.Close();

            cursor = contentResolver.Query(
            Android.Provider.MediaStore.Images.Media.ExternalContentUri,
            null, MediaStore.Images.Media.InterfaceConsts.Id + " = ? ", new[] { documentId }, null);
            cursor.MoveToFirst();
            string path = cursor.GetString(cursor.GetColumnIndex(MediaStore.Images.Media.InterfaceConsts.Data));
            cursor.Close();

            return path;
        }


        public static byte[] GetByteArray(Stream inputStream)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}