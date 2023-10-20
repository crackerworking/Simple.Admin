using Mi.Domain.Shared.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Domain.Shared.Core
{
    [ApiRoute]
    [ApiController]
    public abstract class MiControllerBase : ControllerBase
    {
    }
}