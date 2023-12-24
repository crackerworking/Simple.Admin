import { http } from "@/utils/http";
import { ApiGenericResponse } from "@/utils/http/types";

export const getAsyncRoutes = () => {
  return http.request<ApiGenericResponse<any>>(
    "post",
    "/api/Personal/GetSiderMenu"
  );
};
