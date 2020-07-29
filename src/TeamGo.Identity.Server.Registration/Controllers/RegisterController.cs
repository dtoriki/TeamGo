using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TeamGo.Identity.Server.Registration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<IdentityUser<Guid>> _userManager;
        private readonly ILogger _logger;
        public RegisterController(UserManager<IdentityUser<Guid>> userManager, ILogger logger)
        {
            _userManager = userManager;
            _logger = logger;
        }


    }
}
