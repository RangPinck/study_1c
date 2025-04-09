using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Study1CApi.DTOs.StudyStateDTOs
{
    public class StudyStateDTO
    {
        public int StateId { get; set; }
        
        public string StateName { get; set; } = null!;
    }
}
