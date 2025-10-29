namespace ASPNETCoreApplicationFinal.Models;

public class Student
{
    public int ID { get; set; }
    public string Surname { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public int Age { get; set; }
    public DateTime Birthday { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string? Remarks { get; set; }
}
