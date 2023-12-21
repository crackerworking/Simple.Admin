import { ApiResponse, response_type } from "./types";

/** 确保成功 */
export function EnsureSuccess(res: any | ApiResponse) {
  return res.code === response_type.Success;
}
