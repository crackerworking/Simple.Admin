<script setup lang="ts">
import { ref } from "vue";
import { formRules } from "./utils/rule";
import { FormProps } from "./utils/types";
import { usePublicHooks } from "../hooks";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    id: "",
    name: "",
    isEnabled: 0,
    remark: "",
    extraParams: "",
    cron: ""
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
    label-width="120px"
  >
    <el-form-item label="任务名称" prop="name">
      <el-input v-model="newFormInline.name" disabled />
    </el-form-item>
    <el-form-item label="状态" prop="isEnabled">
      <el-switch
        v-model="newFormInline.isEnabled"
        :active-value="1"
        :inactive-value="0"
        active-text="已启动"
        inactive-text="已暂停"
        inline-prompt
        :style="switchStyle"
      />
    </el-form-item>
    <el-form-item label="cron表达式" prop="cron">
      <el-input v-model="newFormInline.cron" placeholder="请输入cron表达式" />
    </el-form-item>
    <el-form-item label="额外参数" prop="extraParams">
      <el-input
        v-model="newFormInline.extraParams"
        placeholder="请输入额外参数"
      />
    </el-form-item>

    <el-form-item label="备注">
      <el-input
        v-model="newFormInline.remark"
        placeholder="请输入备注信息"
        type="textarea"
      />
    </el-form-item>
  </el-form>
</template>
