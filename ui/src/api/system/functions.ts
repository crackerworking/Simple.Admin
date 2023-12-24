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

/** 新增 */
export const addFunction = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Function/AddFunction",
    {
      data
    }
  );
};

/** 修改 */
export const updateFunction = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Function/UpdateFunction",
    {
      data
    }
  );
};

/** 移除 */
export const deleteFunction = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Function/RemoveFunction",
    {
      data
    }
  );
};
