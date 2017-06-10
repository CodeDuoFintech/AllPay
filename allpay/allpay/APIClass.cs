using System;
using System.Threading.Tasks;
using System.Json;
using System.IO;
using System.Net;

namespace allpay
{
    
    public class APIClass
    {
        private string apiURL = "http://codeduomobileapi.azurewebsites.net/api/";

        public APIClass()
        {

        } // APIClass
		  
		public async Task<JsonValue> CallAPI(string url)
		{
            // Create an HTTP web request using the URL:

		  HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(apiURL += url));
		  request.ContentType = "application/json";
		  request.Method = "GET";

		  // Send the request to the server and wait for the response:
		  using (WebResponse response = await request.GetResponseAsync())
		  {
			  // Get a stream representation of the HTTP web response:
			  using (Stream stream = response.GetResponseStream())
			  {
				  // Use this stream to build a JSON document object:
				  JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));

				  // Return the JSON document:
				  return jsonDoc;
			  }
		  }
		} // CallAPI

	} // APIClass

}
