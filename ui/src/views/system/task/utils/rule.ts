import { reactive } from "vue";
import type { FormRules } from "element-plus";

/** 自定义表单规则校验 */
export const formRules = reactive(<FormRules>{
  name: [{ required: true, message: "任务名称为必填项", trigger: "blur" }],
  cron: [{ required: true, message: "cron表达式为必填项", trigger: "blur" }],
  isEnabled: [{ required: true, message: "启动状态为必填项", trigger: "blur" }]
});
