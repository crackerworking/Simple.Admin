/** 确保成功 */
export function EnsureSuccess(res: any) {
  return parseInt(res?.code) === 10000;
}
