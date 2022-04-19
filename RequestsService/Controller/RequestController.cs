using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RequestsService.Data;

namespace RequestsService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("get-services-status")]
        public async Task<string[]> GetValuesAsync()
        {
            var urls = await _context.WebServices.AsNoTracking().Select(w => w.Url).ToListAsync();
            var tasks = new List<Task<string>>();

            try
            {
                using var httpClient = new HttpClient();

                urls.ForEach(url => tasks.Add(MakeRequestAsync(httpClient, url))); 

                return await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<string> MakeRequestAsync(HttpClient httpClient, string url)
        {
            try
            {
                var response = await httpClient
                    .GetAsync(url, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

                return response.StatusCode.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
