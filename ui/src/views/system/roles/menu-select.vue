<script setup lang="ts">
import { onMounted, watch, ref } from "vue";
import { getFunctionTree, getRoleFunctionIds } from "@/api/system/roles";
import "./menu-select.css";

const props = defineProps(["id"]);
const treeData = ref<Array<any>>([]);
const filterText = ref<string>("");
const treeRef = ref();
const defaultKeys = ref([]);
const defaultExpandedKeys = ref([]);
const defaultProps = {
  label: "functionName",
  children: "children"
};

function filterNode(value, data) {
  if (!value) return true;
  return data.functionName.includes(value);
}
watch(filterText, newVal => {
  treeRef.value.filter(newVal);
});

onMounted(async () => {
  const { result } = await getFunctionTree();
  treeData.value = result;
  const { result: ids } = await getRoleFunctionIds({ id: props.id });
  defaultKeys.value = ids;
  defaultExpandedKeys.value = ids;
});

function getRef() {
  return treeRef.value;
}

defineExpose({ getRef });
</script>

<template>
  <div>
    <el-input v-model="filterText" placeholder="请输入功能名称" />
    <el-tree
      :data="treeData"
      show-checkbox
      :props="defaultProps"
      node-key="id"
      ref="treeRef"
      :default-checked-keys="defaultKeys"
      :check-strictly="true"
      class="my-1"
      :default-expanded-keys="defaultExpandedKeys"
      :filter-node-method="filterNode"
    >
      <template #default="{ node, data }">
        <span class="custom-tree-node">
          <span>{{ node.label }}</span>
          <span class="tag-span">
            <el-tag v-if="data.functionType == 10" type="success" size="small"
              >菜单</el-tag
            >
            <el-tag
              v-else-if="data.functionType == 20"
              type="warning"
              size="small"
              >按钮</el-tag
            >
            <el-tag v-else-if="data.functionType == 30" type="info" size="small"
              >资源</el-tag
            >
            <el-tag v-else-if="data.functionType == 40" size="small"
              >功能</el-tag
            >
          </span>
        </span>
      </template>
    </el-tree>
  </div>
</template>

function ref(arg0: undefined[]) { throw new Error("Function not implemented.");
}
