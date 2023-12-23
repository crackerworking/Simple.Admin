import { http } from "@/utils/http";
import { ApiGenericResponse } from "@/utils/http/types";

/** 功能列表 */
export const getFunctions = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Function/GetFunctions",
    {
      data
    }
  );
};

/** 菜单下拉数据 */
export const getMenuTree = () => {
  return http.request<ApiGenericResponse<any>>("post", "/getMenuTree");
};
