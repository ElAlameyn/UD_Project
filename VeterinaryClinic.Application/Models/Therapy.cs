namespace VeterinaryClinic.Application.Models;

public class Therapy
{
    public int Id { get; set; }
    
    public int DiagnosisId { get; set; }

    public string Name { get; set; } = "";

    public decimal Price { get; set; } = decimal.MaxValue;
}