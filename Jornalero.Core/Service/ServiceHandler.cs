using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using Jornalero.Core;

namespace Jornalero.Core
{
	public static class ServiceHandler
	{
		//static object lockObj;
		//static readonly RestClient Client;
		static ServiceHandler ()
		{
			//lockObj = new object();
			//Client = new RestClient(Constants.WebApiUATUrl);  // Should be change with live
		}

		public static async Task<T> PostData<T, Tr> (string endpoint, HttpMethod method, Tr content)
		{
			T returnResult = default(T);
			HttpClient client = null;
			try {
				client = new HttpClient ();
				client.BaseAddress = new Uri (Constants.WebApiUATUrl);
				client.DefaultRequestHeaders.Add ("Accept", "application/json");
				client.Timeout = new TimeSpan (0, 0, 15);

				HttpResponseMessage result = null;
				StringContent data = null;

				if (content != null)
					data = new StringContent (JsonConvert.SerializeObject (content), UTF8Encoding.UTF8, "application/json");

				if (method == HttpMethod.Get)
					result = await client.GetAsync (endpoint);

				if (method == HttpMethod.Put)
					result = await client.PutAsync (endpoint, data);

				if (method == HttpMethod.Delete)
					result = await client.DeleteAsync (endpoint);

				if (method == HttpMethod.Post)
					result = await client.PostAsync (endpoint, data);

				if (result != null) {
					if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK) {
						var json = result.Content.ReadAsStringAsync ().Result;
						returnResult = JsonConvert.DeserializeObject<T> (json);
					}
				}
			} catch (Exception ex) {
				Debug.WriteLine ("Error fetching data: " + ex.Message);
			} finally {
				if (client != null)
					client.Dispose ();
			}

			return returnResult;
		}

		public static string PostDataSync (System.Object objects, string resource)
		{
			//            var Client = new RestClient(Constants.WebApiUATUrl);
			//            var request = new RestRequest();
			//            request.Method = HttpMethod.Post;
			//            request.Resource = resource;
			//            request.AddHeader("Content-type", "application/json");
			//            request.AddHeader("Accept", "application/json");
			//            request.AddBody(objects);
			//            try
			//            {
			//                Client.Execute(request).ContinueWith((completed) =>
			//                    {
			//                        if (!completed.IsFaulted)
			//                        {
			//                            System.Diagnostics.Debug.WriteLine(Convert.ToString(completed.Result));
			//                        }
			//                    });
			//            }
			//            catch (System.Exception ex)
			//            {
			//                System.Diagnostics.Debug.WriteLine("Yess we have an Exception here : " + ex.InnerException);
			//            }
			return string.Empty;
		}

		public class BadRequestException : Exception
		{
			public BadRequestException ()
			{
			}
		}

		public class InternalServerErrorException : Exception
		{
			public InternalServerErrorException ()
			{
			}
		}

		public class NetworkUnavailableException : Exception
		{
			public NetworkUnavailableException ()
			{
			}
		}
	}
}