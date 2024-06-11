namespace WebDoctorApp.DTO_s;

public class GetPatientDTO
{
    public PatientDTO patient { get; set; }
    public ICollection<PrescriptionDTO> prescriptions { get; set; } = new List<PrescriptionDTO>();
}

public class PrescriptionDTO{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }

    public ICollection<MedicamentDTO> medicaments { get; set; } = new List<MedicamentDTO>();
   // public ICollection<DoctorDTO> doctors { get; set; }
    public DoctorDTO doctor { get; set; }
}

public class DoctorDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
}