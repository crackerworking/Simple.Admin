# 基于角色的权限访问控制管理项目（RBAC）

框架基于.NET8.0，使用RazorPage构建的后台管理，实现了页面、按钮（接口）权限拦截。整体项目技术简单，非常适合作为初学项目和升级到企业后台，并且有自定义无权限页面、禁止访问错误页面、动态定时任务、内存缓存key管理等特色功能。
## 演示
* 预览地址：http://www.cracker.ink 
* 账号密码：demo1 ZDnyf2 
## 菜单
* 工作空间/仪表板
* 工作空间/消息中心
* 系统管理
  * 用户管理
  * 角色管理
  * 数据字典
  * UI配置
  * 系统日志（登录、行为日志）
* 开发维护
  * iconfont
  * 定时任务
  * 缓存Key
  * 表单生成
## 使用技术
* EFCore
* Dapper
* LayUI
* AutoMapper
* Quartz.net
* SQLite
* ImageSharp
* Nito.AsyncEx
* SignalR
## 使用
1. 确保安装了.NET8 SDK
2. 克隆项目：
`git clone https://github.com/yoursession/Simple.Admin.git`
3. 使用vs2022打开解决方案，找到Simple.Admin.Web.Host下的libman.json右击还原客户端库
4. 成功启动项目
5. 实现抽象类`Startup.cs`动态管道配置
6. 实现`IScoped`或`ISingleton`或`ITransient`动态依赖注入
## 引用关系
* 应用层：Simple.Admin.Application => Simple.Admin.Application.Contracts,Simple.Admin.Domain
* 应用抽象层：Simple.Admin.Application.Contracts => Simple.Admin.Domain.Shared
* 控制器层：Simple.Admin.ControllerLibrary => Simple.Admin.Application.Contracts
* 数据访问层：Simple.Admin.DataDriver => Simple.Admin.Domain
* 领域层：Simple.Admin.Domain => Simple.Admin.Domain.Shared
* 领域共享层：Simple.Admin.Domain.Shared
* RazorPage/UI层：Simple.Admin.RazorLibrary => Simple.Admin.Application.Contracts
* Web应用入口层：Simple.Admin.Web.Host => Simple.Admin.Application,Simple.Admin.ControllerLibrary,Simple.Admin.DataDriver,Simple.Admin.RazorLibrary