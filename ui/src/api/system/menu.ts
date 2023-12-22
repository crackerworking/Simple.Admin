import { http } from "@/utils/http";
import { ApiGenericResponse } from "@/utils/http/types";

/** 菜单列表 */
export const getMenuList = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/getMenuList", {
    data
  });
};

/** 菜单下拉数据 */
export const getMenuTree = () => {
  return http.request<ApiGenericResponse<any>>("post", "/getMenuTree");
};
