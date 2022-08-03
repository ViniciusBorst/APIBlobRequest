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
    public class CollectMeasurementsAPIController : ControllerBase
    {
        /// <summary>
        /// API to Collect measuments for the day and type of the sensor choosed
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="date"></param>
        /// <param name="sensorType"></param>
        /// <returns>   array serialized to json format </returns>
        [HttpPost]
        public async Task<string> Post([FromForm] string deviceId, [FromForm] DateTime date, [FromForm] string sensorType)
        {
            ///return all csv data from blob in a string
            string measurements =await BlobConnect.GetCSVBlobData(deviceId, sensorType, date.ToString("yyyy-MM-dd"));
            ///split to create an  string array of each day with his own measurement
            string[] splitedData = measurements.Split("\r\n");
            
            return JsonConvert.SerializeObject(splitedData);
        }
    }
}
