<script setup lang="ts">
import { ref } from "vue";
import { usePublicHooks } from "../hooks";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    username: "",
    remark: "",
    status: 1
  })
});

const { switchStyle } = usePublicHooks();

const ruleFormRef = ref();
const newFormInline = ref(props.formInline);

function getRef() {
  return ruleFormRef.value;
}

defineExpose({ getRef });
</script>

<template>
  <el-form
    ref="ruleFormRef"
    :model="newFormInline"
    :rules="formRules"
    label-width="82px"
  >
    <el-form-item label="用户名" prop="username">
      <el-input
        v-model="newFormInline.username"
        clearable
        placeholder="请输入用户名"
      />
    </el-form-item>

    <el-form-item label="备注">
      <el-input
        v-model="newFormInline.remark"
        placeholder="请输入备注信息"
        type="textarea"
      />
    </el-form-item>

    <el-form-item label="状态">
      <el-switch
        v-model="newFormInline.status"
        :style="switchStyle"
        :active-value="1"
        :inactive-value="0"
        active-text="已启用"
        inactive-text="已停用"
        inline-prompt
      />
    </el-form-item>
  </el-form>
</template>
