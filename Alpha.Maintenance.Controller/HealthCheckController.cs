using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Alpha.Maintenance.Controller
{
    public class HealthCheckController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Proxy(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return new HttpStatusCodeResult(400, "Missing url parameter");

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    var content = await response.Content.ReadAsStringAsync();
                    var contentType = response.Content.Headers.ContentType?.ToString() ?? "text/plain";

                    if (!response.IsSuccessStatusCode)
                    {
                        // Return the status code and content if not successful
                        return new HttpStatusCodeResult((int)response.StatusCode, "An error occurred while sending the request.");
                    }

                    return Content(content, contentType);
                }
                catch (HttpRequestException ex)
                {
                    return new HttpStatusCodeResult(502, "An error occurred while sending the request.");
                }
            }
        }
    }
}