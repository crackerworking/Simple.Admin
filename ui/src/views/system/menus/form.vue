<script setup lang="ts">
import { ref, onMounted } from "vue";
import { FormProps } from "./utils/types";
import { formRules } from "./utils/rule";
import { getMenuTree } from "@/api/system/menu";

const props = withDefaults(defineProps<FormProps>(), {
  formInline: () => ({
    parentId: 0,
    functionName: "",
    functionType: 10,
    url: "",
    icon: "",
    authorizationCode: "",
    sort: 0
  })
});
const editForm = ref(props.formInline);
const filterTree = ref([]);

function filterMethod(value) {
  console.log(value);
}

onMounted(async () => {
  const { result } = await getMenuTree();
  filterTree.value = result;
});
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
        node-key="value"
        value-key="value"
        v-model="editForm.parentId"
        show-checkbox
        :check-strictly="true"
        clearable
        :filter-method="filterMethod"
        filterable
        :data="filterTree"
        :render-after-expand="false"
        :props="{ label: 'name', children: 'children' }"
      />
    </el-form-item>
    <el-form-item label="名称" prop="functionName">
      <el-input v-model="editForm.functionName" />
    </el-form-item>
    <el-form-item label="类型" prop="functionType">
      <el-radio-group v-model="editForm.functionType">
        <el-radio :label="10">菜单</el-radio>
        <el-radio :label="20">按钮</el-radio>
        <el-radio :label="30">资源</el-radio>
        <el-radio :label="40">功能</el-radio>
      </el-radio-group>
    </el-form-item>
    <el-form-item label="地址">
      <el-input v-model="editForm.url" />
    </el-form-item>
    <el-form-item label="图标" v-if="editForm.functionType == 10">
      <el-input v-model="editForm.icon" />
    </el-form-item>
    <el-form-item label="排序">
      <el-input-number v-model="editForm.sort" :min="1" :max="999" />
    </el-form-item>
    <el-form-item label="授权码">
      <el-input v-model="editForm.authorizationCode" />
    </el-form-item>
  </el-form>
</template>
