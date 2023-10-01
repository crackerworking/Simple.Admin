using Mi.Domain.Helper;
using Mi.Domain.Shared.Attributes;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Web.Host
{
    [ApiRoute]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public string Hello() => "hello";

        [HttpGet]
        public FileResult CreateImageWithVerifyCode()
        {
            var bytes = DrawingHelper.CreateByteByImgVerifyCode("3456", 160, 40);
            return File(bytes, "image/png");
        }
    }
}