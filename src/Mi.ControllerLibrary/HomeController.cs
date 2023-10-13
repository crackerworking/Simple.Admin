namespace Mi.ControllerLibrary
{
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// 系统入口
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return Redirect("/index.html");
        }
    }
}