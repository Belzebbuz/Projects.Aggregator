using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Routes;
using SharedLibrary.Wrapper;
using IResult = SharedLibrary.Wrapper.IResult;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Host.Controllers.Common;

[Route(CheckEndpoints.Base)]
public class CheckController : BaseApiController
{
    [HttpGet]
    public async Task<IResult<string>> Check()
        => await Result<string>.SuccessAsync(data: $"v{GitVersionInformation.MajorMinorPatch} - Git sha: {GitVersionInformation.ShortSha}");

    [HttpGet("jwt-auth")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IResult> CheckJwtAuth()
        => await Result.SuccessAsync();
}
