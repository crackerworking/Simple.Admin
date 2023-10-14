# mi-admin
AspNetCoreMvc架构通用后台权限管理，权限设计（用户-角色-功能），功能类型分为目录、菜单、按钮、资源，精确到各个界面的调用权限以及按钮的显示和隐藏； 支持数据库切换、跨平台、自动注入、防抖节流、消息推送。

## WebAPI参数命名规范

**注意：使用公用对象优先**

* 列表查询入参对象拼上"Search"，例：UserSearch
* 列表查询出参对象拼上"Item"，例：UserItem
* 详情查询入参对象拼上"DetailSearch"（如果只有一个ID参数，使用公用对象`PrimaryKey`），例：UserDetailSearch
* 详情查询出参对象拼上"ItemDetail"，例：UserItemDetail
* 新增操作入参对象拼上"Plus"，例如：UserPlus
* 编辑操作入参对象拼上"Edit"，例如：UserEdit
* 删除（含软删除）操作统一使用ID作为参数，使用公用对象`PrimaryKey`和`PrimaryKeys`
* 其它操作，比如：启用状态变更、复制一条数据到XX、用户角色数据，入参加上"In"后缀，出参加上"Out"后缀，例：ChangeRoleEnabledStatusIn、CopyRoleDataToUserIn、UserRoleDataOut
* 同一类型参数，例：接口A和接口B都只有一个字符串参数，考虑参数名释义，需建立两个对象