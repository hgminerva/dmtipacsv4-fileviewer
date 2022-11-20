using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dmtipacs_viewer_openner
{
    class DicomFile
    {
        public int Id { get; set; }
        public string TransactionNumber { get; set; }
        public string TransactionDateTime { get; set; }
        public string DICOMFileName { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public int Age { get; set; }
        public string Particulars { get; set; }
        public int ModalityId { get; set; }
        public int BodyPartId { get; set; }
        public string BodyPart { get; set; }
        public string User { get; set; }
        public string PatientAddress { get; set; }
        public string ReferringPhysician { get; set; }
        public string StudyDate { get; set; }
        public string HospitalNumber { get; set; }
        public string HospitalWardNumber { get; set; }
        public string StudyInstanceId { get; set; }
    }
}
