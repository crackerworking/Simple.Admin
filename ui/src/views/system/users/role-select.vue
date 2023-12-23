<script setup lang="ts">
import { ref, onMounted } from "vue";
import { RoleFormProps } from "./utils/types";
import { getRoleOptions } from "@/api/system/users";

const props = withDefaults(defineProps<RoleFormProps>(), {
  formInline: () => ({
    userId: "",
    roleIds: []
  })
});

const roleFormRef = ref();
const newFormInline = ref(props.formInline);
const options = ref([]);

function getRef() {
  return roleFormRef.value;
}

onMounted(() => {
  getRoleOptions().then(res => {
    options.value = res.result;
  });
});

defineExpose({ getRef });
</script>

<template>
  <el-form ref="roleFormRef" :model="newFormInline" label-width="82px">
    <el-form-item label="配置角色">
      <el-select
        v-model="newFormInline.roleIds"
        class="m-2"
        placeholder="请选择角色"
        multiple
      >
        <el-option
          v-for="item in options"
          :key="item.value"
          :label="item.name"
          :value="item.value"
        />
      </el-select>
    </el-form-item>
  </el-form>
</template>
