using System.Collections.Generic;
using System.IO;
using System.Web;
using Alpha.Maintenance.Core.Model;

namespace Alpha.Maintenance.Core.Extension
{
    public static class FileExtensions
    {
        public static byte[] ToByteArray(this HttpPostedFileBase file)
        {
            byte[] buffer;

            using (var inputStream = file.InputStream)
            {
                var target = new MemoryStream();
                inputStream.CopyTo(target);
                buffer = target.ToArray();
            }

            return buffer;
        }

        public static List<UploadFileModel> GetInfo(this HttpFileCollectionBase files)
        {
            var result = new List<UploadFileModel>();

            foreach (string key in files)
            {
                var file = files[key];

                result.Add(new UploadFileModel
                {
                    Name = file.FileName,
                    Target = key,
                    Content = ToByteArray(file),
                });
            }

            return result;
        }
    }
}
