import { http } from "@/utils/http";
import { ApiGenericResponse, ApiResponse } from "@/utils/http/types";

/** 获取所有缓存key */
export const getAllKeys = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/CacheKeyManager/GetAllKeys",
    {
      data
    }
  );
};

/** 获取缓存数据 */
export const getData = (data?: object) => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/CacheKeyManager/GetData",
    {
      data
    }
  );
};

/** 移除缓存 */
export const removeKey = (data?: object) => {
  return http.request<ApiResponse>("post", "/api/CacheKeyManager/RemoveKey", {
    data
  });
};
