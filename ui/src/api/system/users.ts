import { http } from "@/utils/http";
import { ApiGenericResponse, PagingModel } from "@/utils/http/types";

/** 用户列表 */
export const getUserList = (data?: object) => {
  return http.request<ApiGenericResponse<PagingModel<any>>>(
    "post",
    "/getUserList",
    {
      data
    }
  );
};
