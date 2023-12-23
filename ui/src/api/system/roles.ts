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

/** 功能树 */
export const getFunctionTree = (data = { test: "hello" }) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Function/GetFunctionList",
    { data }
  );
};

/** 角色功能ID */
export const getRoleFunctionIds = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Role/GetRoleFunctionIds",
    { data }
  );
};

/** 设置角色功能 */
export const setRoleFunctions = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Role/SetRoleFunctions",
    { data }
  );
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
