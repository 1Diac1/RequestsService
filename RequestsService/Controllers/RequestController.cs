using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RequestsService.Models;
using RequestsService.Data;

namespace RequestsService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _context;

        public RequestController(ApplicationDbContext context, 
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
        }

        [HttpGet("get-services-status")]
        public async Task<BaseResponse[]> GetValuesAsync(CancellationToken ct)
        {
            //  var urls = new List<string> { "https://vk.com", "https://ya.ru", "https://google.com" };
            //  можно использовать https://twitter.com, этот сервис долгий (>5sec) и в таком случае он выведет ошибку Operation Timeout
            var urls = await _context.WebServices.AsNoTracking().Select(w => w.Url).ToListAsync(ct);
            var tasks = new List<Task<BaseResponse>>();

            try
            {
                var httpClient = _httpClientFactory.CreateClient();

                urls.ForEach(url => tasks.Add(MakeRequestAsync(httpClient, url, ct))); 

                return await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                throw new Exception($"Catching {ex.GetType().FullName} {ex is AggregateException}");
            }
        }

        private async Task<BaseResponse> MakeRequestAsync(HttpClient httpClient, string url, CancellationToken ct)
        {
            using var childCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            childCts.CancelAfter(TimeSpan.FromSeconds(5));

            try
            {
                var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, url), childCts.Token);
                
                if (response.IsSuccessStatusCode)
                    return BaseResponse.Success(url, response.StatusCode.ToString());

                return BaseResponse.Failure(url, response.StatusCode.ToString());
            }
            catch (OperationCanceledException) when (childCts.IsCancellationRequested)
            {
                return BaseResponse.Failure(url, "Operation Timeout");
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure(url, ex.Message);
            }
        }
    }
}
