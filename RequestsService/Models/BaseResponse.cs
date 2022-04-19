namespace RequestsService.Models
{
    public class BaseResponse
    {
        public string? Url { get; set; }
        public string? Status { get; set; }
        public bool IsSuccess { get; set; }

        public BaseResponse(string url, string status, bool isSuccess)
        {
            Url = url;
            Status = status;
            IsSuccess = isSuccess;
        }

        public static BaseResponse Failure(string url, string status) =>
            new BaseResponse(url, status, false);

        public static BaseResponse Success(string url, string status) =>
            new BaseResponse(url, status, true);
    }
}
