using Microsoft.AspNetCore.Mvc;
using Study1CApi.Interfaces;

namespace Study1CApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticeController : ControllerBase
    {
        private readonly IPracticeRepository _practiceRepository;

        public PracticeController(IPracticeRepository practiceRepository)
        {
            _practiceRepository = practiceRepository;
        }


    }
}
