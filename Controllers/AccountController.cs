namespace Blog.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {

        [HttpPost("v1/accounts/")]
        public async Task<IActionResult> Post(
            [FromBody]RegisterViewModel model,
            [FromServices]BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Slug = model.Email.Replace("@", "-").Replace(".", "-")
            };

            return Ok(user);
        }

        [HttpPost("v1/accounts/login")]
        public IActionResult Login([FromServices]TokenService tokenService)
        {
            var token = tokenService.GenerateToken(null);

            return Ok(token);
        }
    }
}
