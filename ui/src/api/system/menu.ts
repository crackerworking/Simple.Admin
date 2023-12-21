import { http } from "@/utils/http";
import { ApiGenericResponse } from "@/utils/http/types";

/** 菜单列表 */
export const getMenuList = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/getUserList", {
    data
  });
};
