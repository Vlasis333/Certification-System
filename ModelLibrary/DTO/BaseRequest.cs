using System.ComponentModel.DataAnnotations;

namespace ModelLibrary.Models.DTO;

public class BaseRequest<T> where T : class
{
    // [Required]
    public string RequestId { get; set; }

    public T Data { get; set; }
}
