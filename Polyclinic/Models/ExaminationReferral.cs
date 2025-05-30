﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Polyclinic.Models
{
    public class ExaminationReferral
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Diagnosis")]
        [Display(Name = "Предварительный диагноз")]
        public string? DiagnosisId { get; set; }
        [Display(Name = "Предварительный диагноз")]

        public virtual Diagnosis? Diagnosis { get; set; }
        [ForeignKey("Doctor")]
        [Display(Name = "Доктор")]
        public int? DoctorId { get; set; }
        [Display(Name = "Доктор")]

        public virtual Doctor? Doctor { get; set; }
        [ForeignKey("Patient")]
        [Display(Name = "Пациент")]
        public int? PatientId { get; set; }
        [Display(Name = "Пациент")]

        public virtual Patient? Patient { get; set; }
        [Display(Name = "Тип")]
        public string? Type { get; set; }
        [ForeignKey("FunctionalDiagnosticsDoctor")]
        [Display(Name = "Врач функциональной диагностики")]
        public int? FunctionalDiagnosticsDoctorId { get; set; }
        [Display(Name = "Врач функциональной диагностики")]

        public virtual FunctionalDiagnosticsDoctor? FunctionalDiagnosticsDoctor { get; set; }
        [Display(Name = "Номер кабинета")]
        public string? СabinetNum { get; set; }
        [Display(Name = "Дата")]
        public DateTime? DateTime { get; set; }
    }
}
