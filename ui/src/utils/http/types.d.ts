import Axios, {
  Method,
  AxiosError,
  AxiosResponse,
  AxiosRequestConfig
} from "axios";

export type resultType = {
  accessToken?: string;
};

export type RequestMethods = Extract<
  Method,
  "get" | "post" | "put" | "delete" | "patch" | "option" | "head"
>;

export interface PureHttpError extends AxiosError {
  isCancelRequest?: boolean;
}

export interface PureHttpResponse extends AxiosResponse {
  config: PureHttpRequestConfig;
}

export interface PureHttpRequestConfig extends AxiosRequestConfig {
  beforeRequestCallback?: (request: PureHttpRequestConfig) => void;
  beforeResponseCallback?: (response: PureHttpResponse) => void;
}

export default class PureHttp {
  request<T>(
    method: RequestMethods,
    url: string,
    param?: AxiosRequestConfig,
    axiosConfig?: PureHttpRequestConfig
  ): Promise<T>;
  post<T, P>(
    url: string,
    params?: T,
    config?: PureHttpRequestConfig
  ): Promise<P>;
  get<T, P>(
    url: string,
    params?: T,
    config?: PureHttpRequestConfig
  ): Promise<P>;
}

/** 原始响应 */
export enum response_type {
  /** 成功 */
  Success = 10000,
  /** 参数错误 */
  ParameterError = 10001,
  /** 未登录 */
  NonAuth = 10002,
  /** 禁止访问 */
  Forbidden = 10003,
  /** 找不到，不存在 */
  NonExist = 10004,
  /** 程序错误 */
  Error = 10005,
  /** 失败 */
  Fail = 10006,
  /** 请求频繁 */
  FrequentRequests = 10007
}

export interface ApiResponse {
  code: response_type;
  message: string;
}

export interface ApiGenericResponse<T> extends ApiResponse {
  result: T;
}

export interface PagingModel<T> {
  total: number;
  rows: Array<T>;
  page: number;
  size: number;
}
