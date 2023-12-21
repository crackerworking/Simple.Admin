import { MockMethod } from "vite-plugin-mock";

export default [
  {
    url: "/getMenuList",
    method: "post",
    response: ({ body }) => {
      console.debug(body);
      return {
        code: 10000,
        result: {
          total: 1,
          rows: [
            {
              id: "525574322618777600",
              functionName: "系统管理",
              functionType: 10,
              url: "#",
              icon: "",
              sort: 1,
              authorizationCode: "System",
              children: [
                {
                  id: "3459840930954",
                  functionName: "用户管理",
                  functionType: 10,
                  url: "/system/user",
                  icon: "",
                  sort: 1,
                  authorizationCode: "System:User"
                }
              ]
            }
          ]
        }
      };
    }
  }
] as MockMethod[];
