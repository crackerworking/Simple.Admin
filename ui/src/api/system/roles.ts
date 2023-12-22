import { http } from "@/utils/http";
import { ApiGenericResponse, PagingModel } from "@/utils/http/types";

/** 角色列表 */
export const getRoleList = (data?: object) => {
  return http.request<ApiGenericResponse<PagingModel<any>>>(
    "post",
    "/api/Role/GetRoleList",
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

/** 删除角色 */
export const deleteRole = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/api/Role/RemoveRole", {
    data
  });
};

/** 新增角色 */
export const addRole = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/api/Role/AddRole", {
    data
  });
};

/** 修改角色 */
export const updateRole = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/api/Role/UpdateRole", {
    data
  });
};
