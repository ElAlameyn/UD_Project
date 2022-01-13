namespace VeterinaryClinic.Application.Models;

public class Diagnosis
{
    public int Id { get; set; }

    public string Name { get; set; } = "";
    
    public DateTime SetupDate { get; set; }
    
    public int DoctorId { get; set; }
    
    public int AnimalId { get; set; }
}