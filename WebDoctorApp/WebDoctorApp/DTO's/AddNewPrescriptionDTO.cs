namespace WebDoctorApp.DTO_s;

public class AddNewPrescriptionDTO
{
    public PatientDTO patient { get; set; }
    public DoctorDTO doctor { get; set; }
    public ICollection<MedicamentDTO> medicaments { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
}

public class PatientDTO
{
    public int IdPatient { get; set; }
   
    public string FirstName { get; set; }
   
    public string LastName { get; set; }
    public DateOnly Birthday { get; set; }
}

public class MedicamentDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
}