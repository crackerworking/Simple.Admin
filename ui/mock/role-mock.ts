import { MockMethod } from "vite-plugin-mock";

export default [
  {
    url: "/getRoleList",
    method: "post",
    response: ({ body }) => {
      console.debug(body);
      return {
        code: 10000,
        result: {
          total: 1,
          rows: [
            {
              id: "54798392349945",
              rolename: "演示",
              remark: "只读权限",
              status: 1
            }
          ]
        }
      };
    }
  },
  {
    url: "/getRoleFunctions",
    method: "post",
    response: ({ body }) => {
      console.debug(body);
      return {
        code: 10000,
        result: [
          {
            id: "54798392349945",
            functionName: "系统管理",
            functionType: 10,
            children: [
              {
                id: "7430934834934",
                functionName: "用户管理",
                functionType: 10
              },
              {
                id: "5795654054023",
                functionName: "角色管理",
                functionType: 10
              }
            ]
          }
        ]
      };
    }
  }
] as MockMethod[];
