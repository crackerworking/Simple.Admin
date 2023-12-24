import { reactive } from "vue";
import type { FormRules } from "element-plus";
// import { isPhone, isEmail } from "@pureadmin/utils";

/** 自定义表单规则校验 */
export const formRules = reactive(<FormRules>{
  functionName: [{ required: true, message: "名称为必填项", trigger: "blur" }],
  functionType: [{ required: true, message: "类型为必填项", trigger: "blur" }]
  // email: [
  //   {
  //     validator: (rule, value, callback) => {
  //       if (value === "") {
  //         callback();
  //       } else if (!isEmail(value)) {
  //         callback(new Error("请输入正确的邮箱格式"));
  //       } else {
  //         callback();
  //       }
  //     },
  //     trigger: "blur"
  //   }
  // ]
});
