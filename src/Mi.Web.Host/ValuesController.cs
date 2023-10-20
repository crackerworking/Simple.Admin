using Mi.Domain.Helper;
using Mi.Domain.Shared.Core;

using Microsoft.AspNetCore.Mvc;

namespace Mi.Web.Host
{
    public class ValuesController : MiControllerBase
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