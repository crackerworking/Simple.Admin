import { http } from "@/utils/http";
import { ApiGenericResponse } from "@/utils/http/types";

/** 通知列表 */
export const getMessageList = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Message/GetMessageList",
    {
      data
    }
  );
};

/** 已读通知 */
export const readed = (data?: object) => {
  return http.request<ApiGenericResponse<any>>("post", "/api/Message/Readed", {
    data
  });
};

/** 顶部通知 */
export const getHeaderMsg = () => {
  return http.request<ApiGenericResponse<any>>(
    "get",
    "/api/Personal/GetHeaderMsg"
  );
};
