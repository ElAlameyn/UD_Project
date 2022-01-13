namespace VeterinaryClinic.Application.Models;

public class Animal
{
    public int Id { get; set; }
    
    public int MasterId { get; set; }

    public string Name { get; set; } = "";

    public string Breed { get; set; } = "";
}