using Microsoft.AspNetCore.Mvc;

namespace Mi.ControllerLibrary
{
    [Route("/api/values")]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public string Hello() => "hello";
    }
}