namespace MottuApi.Services
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ServiceResponse() { }

        public ServiceResponse(T data, string message = "")
        {
            Data = data;
            Message = message;
        }
    }
}

