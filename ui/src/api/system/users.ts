import { http } from "@/utils/http";
import {
  ApiGenericResponse,
  ApiResponse,
  PagingModel
} from "@/utils/http/types";

/** 用户列表 */
export const getUserList = (data?: object) => {
  return http.request<ApiGenericResponse<PagingModel<any>>>(
    "post",
    "/api/User/GetUserList",
    {
      data
    }
  );
};

/** 新增用户 */
export const addUser = (data?: object) => {
  return http.request<ApiResponse>("post", "/api/User/AddUser", {
    data
  });
};

/** 切换用户状态 */
export const switchUserState = (data?: object) => {
  return http.request<ApiResponse>("post", "/api/User/SwitchState", {
    data
  });
};

/** 重置用户密码 */
export const resetUserPassword = (data?: object) => {
  return http.request<ApiGenericResponse<string>>(
    "post",
    "/api/User/UpdatePassword",
    {
      data
    }
  );
};
