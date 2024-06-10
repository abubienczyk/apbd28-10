namespace WebDoctorApp.DTO_s;

public class GetPatientDTO
{
    public PatientDTO patient { get; set; }
    public ICollection<PrescriptionDTO> prescriptions { get; set; }
}

public class PrescriptionDTO{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public ICollection<MedicamentDTO> medicaments { get; set; }
    public ICollection<DoctorDTO> doctors { get; set; }
}

public class DoctorDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
}