﻿//using SharpCompress.Archive.Zip;
//using SharpCompress.Common;
//using SharpCompress.Reader;
using Catrobat.IDE.Core.Services.Storage;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Catrobat.IDE.Core.Services.Common
{
    public class ZipService : IZipService
    {
        public async Task UnzipCatrobatPackageIntoIsolatedStorage(Stream zipStream, string localStoragePath)
        {
            using (var storage = StorageSystem.GetStorage())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
                {
                    foreach (var entry in archive.Entries)
                    {
                        using (var stream = entry.Open())
                        {
                            if (Path.GetExtension(entry.Name) == ".nomedia")
                                continue;

                            var filePath = Path.Combine(localStoragePath, entry.FullName);


                            if (Path.GetFileName(filePath) == "") continue;
                            
                            var outStream = await storage.OpenFileAsync(filePath, 
                                StorageFileMode.CreateNew, StorageFileAccess.Write);
                            
                            await stream.CopyToAsync(outStream);
                            outStream.Dispose();
                            stream.Dispose();
                        }
                    }
                }
            }





            //if (zipStream != null)
            //{
            //    var reader = ReaderFactory.Open(zipStream);

            //    using (var storage = StorageSystem.GetStorage())
            //    {
            //        while (reader.MoveToNextEntry())
            //        {
            //            var absolutPath = Path.Combine(localStoragePath, reader.Entry.FilePath);
            //            absolutPath = absolutPath.Replace("\\", "/");

            //            if (!reader.Entry.IsDirectory)
            //            {
            //                if (await storage.FileExistsAsync(absolutPath))
            //                {
            //                    await storage.DeleteFileAsync(absolutPath);
            //                }

            //                var fileStream = await storage.OpenFileAsync(absolutPath,
            //                                                  StorageFileMode.Create,
            //                                                  StorageFileAccess.Write);
            //                reader.WriteEntryTo(fileStream);
            //                fileStream.Dispose();
            //            }
            //        }
            //    }

            //    reader.Dispose();
            //}
        }

        public async Task ZipCatrobatPackage(Stream zipStream, string localStoragePath)
        {
            using (var storage = StorageSystem.GetStorage())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                {
                    await WriteFilesRecursiveToZip(archive, storage, localStoragePath, "");
                }
            }
        }

        private async Task WriteFilesRecursiveToZip(ZipArchive archive, IStorage storage,
            string sourceBasePath, string destinationBasePath)
        {
            try
            {
                var searchPattern = sourceBasePath;
                var fileNames = await storage.GetFileNamesAsync(searchPattern);

                foreach (string fileName in fileNames)
                {
                    if (fileName.EndsWith(StorageConstants.ImageThumbnailExtension))
                        continue;

                    var tempPath = Path.Combine(sourceBasePath, fileName);
                    using (var fileStream = await storage.OpenFileAsync(tempPath,
                        StorageFileMode.Open, StorageFileAccess.Read))
                    {
                        var destinationPath = Path.Combine(destinationBasePath, fileName);
                        var newEntry = archive.CreateEntry(destinationPath);
                        using (var stream = newEntry.Open())
                        {
                            await fileStream.CopyToAsync(stream);
                        }
                    }
                }

                var directrryNames = await storage.GetDirectoryNamesAsync(searchPattern);
                foreach (string directoryName in directrryNames)
                {
                    var tempZipPath = Path.Combine(sourceBasePath, directoryName);
                    var nextDestinationBasePath = Path.Combine(destinationBasePath, directoryName);
                    await WriteFilesRecursiveToZip(archive, storage, tempZipPath, nextDestinationBasePath);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}