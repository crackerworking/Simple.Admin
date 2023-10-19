using Mi.Domain.Shared.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Domain.Shared.Core
{
    [ApiRoute]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class MiControllerBase : ControllerBase
    {
    }
}