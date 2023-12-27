import { http } from "@/utils/http";
import { ApiGenericResponse } from "@/utils/http/types";

/** 登录日志列表 */
export const getLoginLogList = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/SysLog/GetLoginLogList",
    {
      data
    }
  );
};
