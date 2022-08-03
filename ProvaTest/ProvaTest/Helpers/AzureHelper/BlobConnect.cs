using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaTest.Helpers.AzureHelper
{
    public class BlobConnect
    {
        /// <summary>
        /// Creates the connection on Azure's containers
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        internal static CloudBlobContainer StartConnectionAsync(string folder)
        {
            //// Retrieve storage account from connection string on string properties.
            var storageConnect = CloudStorageAccount.Parse(StringProperties.BLOB_SAS);
            //// Create the blob client.
            CloudBlobClient blob = storageConnect.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blob.GetContainerReference(StringProperties.BLOB_CONTAINER_NAME +folder);
            return container;
        }

        /// <summary>
        /// return all csv data from blob in a string
        /// </summary>
        /// <param name="deviceId">device choosed, also first folder </param>
        /// <param name="type">type choosed, also second folder</param>
        /// <param name="date">name of the file</param>
        /// <returns></returns>
        internal static async Task<string> GetCSVBlobData(string deviceId, string type, string date)
        {
            CloudBlobContainer container = StartConnectionAsync("/" + deviceId + "/" + type );
            CloudAppendBlob blob = container.GetAppendBlobReference(date + ".csv");

            string text;
            using (var memoryStream = new MemoryStream())
            {
                //downloads blob's content to a stream
                await blob.DownloadToStreamAsync(memoryStream);

                //puts the byte arrays to a string
                text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            return text;
        }
    }
    
}
