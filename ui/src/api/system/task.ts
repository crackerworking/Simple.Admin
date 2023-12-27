import { http } from "@/utils/http";
import { ApiGenericResponse } from "@/utils/http/types";

/** 定时任务列表 */
export const getTaskList = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/api/SysTask/GetList", {
    data
  });
};

/** 修改任务列表 */
export const updateTask = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/api/SysTask/Update", {
    data
  });
};
