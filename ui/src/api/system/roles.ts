import { http } from "@/utils/http";
import { ApiGenericResponse, PagingModel } from "@/utils/http/types";

/** 角色列表 */
export const getRoleList = (data?: object) => {
  return http.request<ApiGenericResponse<PagingModel<any>>>(
    "post",
    "/getRoleList",
    {
      data
    }
  );
};

/** 角色功能 */
export const getRoleFunctions = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/getRoleFunctions", {
    data
  });
};
