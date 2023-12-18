using Microsoft.AspNetCore.Mvc;

using Simple.Admin.Domain.Shared.Attributes;

namespace Simple.Admin.Domain.Shared.Core
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