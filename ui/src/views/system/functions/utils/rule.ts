import { reactive } from "vue";
import type { FormRules } from "element-plus";
// import { isPhone, isEmail } from "@pureadmin/utils";

/** 自定义表单规则校验 */
export const formRules = reactive(<FormRules>{
  title: [{ required: true, message: "名称为必填项", trigger: "blur" }],
  functionType: [{ required: true, message: "类型为必填项", trigger: "blur" }]
});
