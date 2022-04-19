using System.ComponentModel.DataAnnotations;

namespace RequestsService.Models
{
    public class WebService
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}
