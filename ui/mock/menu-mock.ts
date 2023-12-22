import { MockMethod } from "vite-plugin-mock";

export default [
  {
    url: "/getMenuList",
    method: "post",
    response: ({ body }) => {
      console.debug(body);
      return {
        code: 10000,
        result: [
          {
            id: "525574322618777600",
            functionName: "系统管理",
            functionType: 10,
            url: "#",
            icon: "",
            sort: 1,
            authorizationCode: "System"
          },
          {
            id: "3459840930954",
            functionName: "用户管理",
            functionType: 10,
            url: "/system/user",
            icon: "",
            sort: 1,
            authorizationCode: "System:User",
            parentId: "525574322618777600"
          }
        ]
      };
    }
  },
  {
    url: "/getMenuTree",
    method: "post",
    response: () => {
      return {
        code: 10000,
        result: [
          {
            name: "系统管理",
            value: "23567687",
            children: [
              {
                name: "用户管理",
                value: "145545656"
              }
            ]
          }
        ]
      };
    }
  }
] as MockMethod[];
