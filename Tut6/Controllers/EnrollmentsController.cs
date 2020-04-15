using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stut6.Models;
using Stut6.Service;

namespace Stut6.Controllers
{
    [Route("/api/enrollment")]
    [ApiController]
    class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _service;
        public EnrollmentsController(IStudentsDbService service)
        {
            this._service = service;
        }
        // POST: api/Enrollments
        [HttpPost(Name = nameof(EnrollStudent))]
        [Route("enroll")]
        public IActionResult EnrollStudent(Student s)
        {

            var result = _service.EnrollStudent(s);
            if (result != null) return CreatedAtAction(nameof(EnrollStudent), result);
            return BadRequest(result.IndexNumber);

        }
        [HttpPost(Name = nameof(Promote))]
        [Route("promote")]
        public IActionResult Promote(Enrollment e)
        {
            var result = _service.PromoteStudent(e);
            if (result != null) return CreatedAtAction(nameof(Promote), result);
            return BadRequest(result);
        }




    }
}
