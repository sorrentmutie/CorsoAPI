using System.ComponentModel.DataAnnotations;

namespace FruitsAPI.Models;

public class FruitModel
{
    public int id { get; set; }
    [Required(ErrorMessage = "Il nome è obbligatorio")]
    [MaxLength(10, ErrorMessage ="Massimo 10 caratteri")]
    public string name { get; set; } = string.Empty;
    public bool instock { get; set; }
}
