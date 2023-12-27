import { http } from "@/utils/http";
import { ApiGenericResponse } from "@/utils/http/types";

/** 字典列表 */
export const getDictList = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Dict/GetDictList",
    {
      data
    }
  );
};

/** 新增字典 */
export const addDict = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/api/Dict/Add", {
    data
  });
};

/** 修改字典 */
export const updateDict = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/api/Dict/Update", {
    data
  });
};

/** 移除字典 */
export const deleteDict = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/api/Dict/RemoveDict", {
    data
  });
};
