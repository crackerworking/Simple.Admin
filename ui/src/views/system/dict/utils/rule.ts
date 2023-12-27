import { reactive } from "vue";
import type { FormRules } from "element-plus";
// import { isPhone, isEmail } from "@pureadmin/utils";

/** 自定义表单规则校验 */
export const formRules = reactive(<FormRules>{
  name: [{ required: true, message: "名称为必填项", trigger: "blur" }],
  Key: [{ required: true, message: "key为必填项", trigger: "blur" }]
});
