# 基于角色的权限访问控制管理项目（RBAC）

项目名叫做**Simple.Admin**，意为简单，简化开发。框架基于.NET8.0，使用AspNetCore+Vue.js(前端项目基于<a href='https://github.com/pure-admin/vue-pure-admin'>pure-admin</a>)构建的后台管理，实现了页面、按钮（接口）权限拦截。整体项目技术简单，非常适合作为初学项目和升级到企业后台，并且有自定义无权限页面、禁止访问错误页面、动态定时任务、内存缓存key、登录日志、右下角弹窗通知等特色功能。
## 作者
* 邮箱： crackerwork@outlook.com
## 仓库
* 记得点个star！（非常感谢）
* github: https://github.com/yoursession/Simple.Admin
* gitee:  https://gitee.com/yoursession/simple-admin
## 演示
* 预览地址：http://www.cracker.ink 
* 账号密码：demo1 IuPyjN 
## 菜单
* 工作空间/通知管理
* 系统管理
  * 用户管理
  * 角色管理
  * 功能管理
  * 字典管理
  * 定时任务
  * 内存缓存
  * 登录日志
* 演示站点
## 使用技术
* EFCore
* Dapper
* Vue.js
* ElementUI
* AutoMapper
* Quartz.net
* SQLite
* ImageSharp
* Nito.AsyncEx
* SignalR
## 开发
1. 确保安装了.NET8 SDK
2. 克隆项目：
`git clone https://github.com/yoursession/Simple.Admin.git`
3. 使用vs2022打开解决方案，找到Simple.Admin.Web.Host下的libman.json右击还原客户端库
4. 成功启动项目
5. 实现抽象类`Startup.cs`动态管道配置
6. 实现`IScoped`或`ISingleton`或`ITransient`动态依赖注入
7. 本地启动项目超级管理员： admin 123456.
## 引用关系
* 应用层：Simple.Admin.Application => Simple.Admin.Application.Contracts,Simple.Admin.Domain
* 应用抽象层：Simple.Admin.Application.Contracts => Simple.Admin.Domain.Shared
* 控制器层：Simple.Admin.ControllerLibrary => Simple.Admin.Application.Contracts
* 数据访问层：Simple.Admin.DataDriver => Simple.Admin.Domain
* 领域层：Simple.Admin.Domain => Simple.Admin.Domain.Shared
* 领域共享层：Simple.Admin.Domain.Shared
* Web应用入口层：Simple.Admin.Web.Host => Simple.Admin.Application,Simple.Admin.ControllerLibrary,Simple.Admin.DataDriver,Simple.Admin.RazorLibrary
