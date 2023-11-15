using Mi.Domain.Shared.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Domain.Shared.Core
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    [ApiRoute]
    [ApiController]
    public abstract class MiControllerBase : ControllerBase
    {
    }
}