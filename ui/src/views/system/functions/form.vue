<script setup lang="ts">
import { ref, onMounted } from "vue";
import { FormProps } from "./utils/types";
import { formRules } from "./utils/rule";
import { getFunctionTree } from "@/api/system/roles";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    parentId: 0,
    title: "",
    name: "",
    functionType: 10,
    url: "",
    frameSrc: "",
    icon: "",
    authorizationCode: "",
    sort: 1,
    id: ""
  })
});
const editForm = ref(props.formInline);
const filterTree = ref([]);
const editFormRef = ref();

function filterMethod(value) {
  console.log(value);
}

function getRef() {
  return editFormRef.value;
}

onMounted(() => {
  getFunctionTree().then(res => {
    filterTree.value = res.result;
  });
});

defineExpose({ getRef });
</script>

<template>
  <el-form
    :model="editForm"
    label-width="120px"
    :rules="formRules"
    ref="editFormRef"
  >
    <el-form-item label="上级">
      <el-tree-select
        node-key="id"
        value-key="id"
        v-model="editForm.parentId"
        show-checkbox
        :check-strictly="true"
        clearable
        :filter-method="filterMethod"
        filterable
        :data="filterTree"
        :render-after-expand="false"
        :props="{ label: 'title', children: 'children' }"
      />
    </el-form-item>
    <el-form-item label="标题" prop="title">
      <el-input v-model="editForm.title" placeholder="请输入标题" />
    </el-form-item>
    <el-form-item label="类型" prop="functionType">
      <el-radio-group v-model="editForm.functionType">
        <el-radio :label="10">菜单</el-radio>
        <el-radio :label="20">按钮</el-radio>
      </el-radio-group>
    </el-form-item>
    <el-form-item label="名称" v-if="editForm.functionType == 10" prop="name">
      <el-input
        v-model="editForm.name"
        placeholder="请输入名称（和页面name一致）"
      />
    </el-form-item>
    <el-form-item label="路由" v-if="editForm.functionType == 10" prop="url">
      <el-input v-model="editForm.url" placeholder="请输入路由" />
    </el-form-item>
    <el-form-item
      label="外链"
      v-if="editForm.functionType == 10"
      prop="frameSrc"
    >
      <el-input v-model="editForm.frameSrc" placeholder="请输入外部链接" />
    </el-form-item>
    <el-form-item label="图标" v-if="editForm.functionType == 10">
      <el-input v-model="editForm.icon" placeholder="请输入支持图标字符串" />
    </el-form-item>
    <el-form-item label="排序" prop="sort">
      <el-input-number v-model="editForm.sort" :min="1" :max="999" />
    </el-form-item>
    <el-form-item label="授权码">
      <el-input
        v-model="editForm.authorizationCode"
        placeholder="请输入授权码"
      />
    </el-form-item>
  </el-form>
</template>
