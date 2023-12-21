import { MockMethod } from "vite-plugin-mock";

export default [
  {
    url: "/getUserList",
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
              username: "admin",
              nickname: "管理员",
              status: 1,
              remark: "超级管理员",
              createTime: "2023-12-19"
            }
          ]
        }
      };
    }
  }
] as MockMethod[];
