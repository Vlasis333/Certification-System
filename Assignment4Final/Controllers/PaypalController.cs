using Assignment4Final.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelLibrary.Models;
using ModelLibrary.Models.DTO.Certificates;
using PayPal.Api;
using System.Net;
using static Humanizer.On;

namespace Assignment4Final.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaypalController : ControllerBase
    {
        private PaypalService _paypalService;
        private readonly UserManager<AppUser> _userManager;
        private readonly CandidateExamService _candExamService;
        private readonly CertificatesService _certificateService;

        public PaypalController(
            PaypalService paypalService,
            UserManager<AppUser> userManager,
            CandidateExamService candExamService,
            CertificatesService certificateService
        )
        {
            _certificateService = certificateService;
            _paypalService = paypalService;
            _userManager = userManager;
            _candExamService = candExamService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> CreatePayment(int id)
        {
            var userid = _userManager.GetUserId(User);
            var cert = await _certificateService.GetAsync(id);
            Payment result = await _paypalService.CreatePayment(cert);

            foreach (var link in result.links)
            {
                if (link.rel.Equals("approval_url"))
                {
                    return Ok(link.href);
                }
            }

            return NotFound();
        }

        [HttpGet("success/{id}")]
        public async Task<IActionResult> Success(int id)
        {
            var userId = _userManager.GetUserId(User);
            var candExamDto =
                await _candExamService.GetCandidateExamByCertificateAsync(id, userId);

            return Redirect("https://localhost:44473/candidate/availableexams");
        }
    }
}
