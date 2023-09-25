using Newtonsoft.Json;

namespace ModelLibrary.Models.DTO;

public class BaseResponse<T> where T : class
{
    public string RequestId { get; set; }
    public bool Success { get; set; }

    // [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string? Message { get; set; }

    public T? Data { get; set; }
}
