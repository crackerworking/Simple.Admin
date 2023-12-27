<script setup lang="ts">
import { ref } from "vue";
import { FormProps } from "./utils/types";
import { formRules } from "./utils/rule";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    name: "",
    key: "",
    value: "",
    type: "",
    remark: "",
    sort: 1
  })
});
const editForm = ref(props.formInline);
const editFormRef = ref();

function getRef() {
  return editFormRef.value;
}

defineExpose({ getRef });
</script>

<template>
  <el-form
    :model="editForm"
    label-width="120px"
    :rules="formRules"
    ref="editFormRef"
  >
    <el-form-item label="名称" prop="name">
      <el-input v-model="editForm.name" placeholder="请输入名称" />
    </el-form-item>
    <el-form-item label="字典KEY" prop="key">
      <el-input v-model="editForm.key" placeholder="请输入字典KEY" />
    </el-form-item>
    <el-form-item label="字典值" prop="value">
      <el-input v-model="editForm.value" placeholder="请输入字典值" />
    </el-form-item>
    <el-form-item label="分类" prop="type">
      <el-input v-model="editForm.type" placeholder="请输入分类" />
    </el-form-item>
    <el-form-item label="备注" prop="remark">
      <el-input v-model="editForm.remark" placeholder="请输入备注" />
    </el-form-item>
    <el-form-item label="排序" prop="sort">
      <el-input-number v-model="editForm.sort" :min="1" :max="999" />
    </el-form-item>
  </el-form>
</template>
