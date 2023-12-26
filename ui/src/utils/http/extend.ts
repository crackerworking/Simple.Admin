/** 确保成功 */
export function EnsureSuccess(res: any) {
  return res && parseInt(res?.code) === 10000;
}
