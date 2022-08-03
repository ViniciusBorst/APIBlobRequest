using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProvaTest.Helpers.AzureHelper;
using ProvaTest.Helpers.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProvaTest.REST
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDataForDeviceAPIController : ControllerBase
    {
        /// <summary>
        /// API to get the data from Azure blobs, based on the day 
        /// </summary>
        /// <param name="deviceId">device type choosed</param>
        /// <param name="date">date choosed</param>
        /// <returns> ResponseObject, class serialized to json format, it contains 3 parameter, all arrays</returns>
        [HttpPost]
        public async Task<string> Post([FromForm] string deviceId, [FromForm] DateTime date)
        {
            ///this class will be used to transport the data
            ResponseObject responseObject = new ResponseObject();

            string data;
            ///loop through all enum values to get each type
            foreach (string type in Enum.GetNames(typeof(SensorType)))
            {                
                data = await BlobConnect.GetCSVBlobData(deviceId, type, date.ToString("yyyy-MM-dd"));
                string[] spliteData = data.Split("\r\n");

                switch (type)
                {
                    case "temperature":
                        responseObject.Temperature = spliteData;
                        break;
                    case "humidity":
                        responseObject.Humidity = spliteData;
                        break;
                    case "rainfall":
                        responseObject.Rainfall = spliteData;
                        break;
                }
            }

            return JsonConvert.SerializeObject(responseObject);
        }
    }

    public class ResponseObject
    {
        public string[] Temperature { get; set; }
        public string[] Humidity { get; set; }
        public string[] Rainfall { get; set; }
    }
}
