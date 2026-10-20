using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAppointmentSystem.Controllers
{
    // Represents a sample post returned by the API.
    public class SamplePost
    {
        // Stores the post's unique ID.
        public int Id { get; set; }

        // Stores the post's title.
        public string Title { get; set; } = string.Empty;
    }

    public class PostsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        // Receives the HTTP client factory.
        public PostsController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        // Fetches the sample posts without blocking the request thread.
        public async Task<IActionResult> Index()
        {
            // Creates an HTTP client.
            var client = _clientFactory.CreateClient();

            try
            {
                // Requests the posts asynchronously from the API.
                var posts = await client.GetFromJsonAsync<List<SamplePost>>(
                    "https://jsonplaceholder.typicode.com/posts");

                // Displays the retrieved posts in a table.
                return View(posts ?? new List<SamplePost>());
            }
            catch (HttpRequestException)
            {
                // Displays an empty list if the API is unavailable.
                ViewBag.Error =
                    "Unable to load posts. Please try again later.";

                return View(new List<SamplePost>());
            }
        }
    }
}