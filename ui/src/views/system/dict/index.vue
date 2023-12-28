<script setup lang="ts">
import { PureTable } from "@pureadmin/table";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { PureTableBar } from "@/components/RePureTableBar";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import AddFill from "@iconify-icons/ri/add-circle-line";
import EditPen from "@iconify-icons/ep/edit-pen";
import Delete from "@iconify-icons/ep/delete";
import { useDict } from "./utils/hook";
import { ref } from "vue";

defineOptions({
  name: "Dict"
});

const formRef = ref();
const {
  form,
  loading,
  columns,
  dataList,
  pagination,
  onSearch,
  resetForm,
  openDialog,
  handleSizeChange,
  handleCurrentChange,
  handleSelectionChange,
  removeDict
} = useDict();
</script>

<template>
  <div class="main">
    <el-form
      ref="formRef"
      :inline="true"
      :model="form"
      class="search-form bg-bg_color w-[99/100] pl-8 pt-[12px]"
    >
      <el-form-item label="模糊查询：" prop="vague">
        <el-input
          v-model="form.vague"
          placeholder="请输入字典名称/key"
          clearable
          class="!w-[200px]"
        />
      </el-form-item>
      <el-form-item label="分类：" prop="type">
        <el-input
          v-model="form.type"
          placeholder="请输入分类"
          clearable
          class="!w-[200px]"
        />
      </el-form-item>
      <el-form-item label="备注：" prop="remark">
        <el-input
          v-model="form.remark"
          placeholder="请输入备注"
          clearable
          class="!w-[200px]"
        />
      </el-form-item>
      <el-form-item>
        <el-button
          type="primary"
          :icon="useRenderIcon(Search)"
          :loading="loading"
          @click="onSearch"
        >
          搜索
        </el-button>
        <el-button :icon="useRenderIcon(Refresh)" @click="resetForm(formRef)">
          重置
        </el-button>
      </el-form-item>
    </el-form>
    <PureTableBar title="用户列表" :columns="columns" @refresh="onSearch">
      <template #buttons>
        <el-button
          type="primary"
          :icon="useRenderIcon(AddFill)"
          @click="openDialog()"
          v-auth="'System:Dict:Add'"
        >
          新增字典
        </el-button>
      </template>
      <template v-slot="{ size, dynamicColumns }">
        <pure-table
          :data="dataList"
          :columns="dynamicColumns"
          :size="size"
          :pagination="pagination"
          :paginationSmall="size === 'small' ? true : false"
          :header-cell-style="{
            background: 'var(--el-fill-color-light)',
            color: 'var(--el-text-color-primary)'
          }"
          @selection-change="handleSelectionChange"
          @page-size-change="handleSizeChange"
          @page-current-change="handleCurrentChange"
        >
          <template #operation="{ row }">
            <el-button
              class="reset-margin"
              link
              type="primary"
              :size="size"
              :icon="useRenderIcon(EditPen)"
              @click="openDialog('编辑', row)"
              v-auth="'System:Dict:Update'"
            >
              修改
            </el-button>
            <el-button
              class="reset-margin"
              link
              type="primary"
              :size="size"
              :icon="useRenderIcon(Delete)"
              @click="removeDict(row)"
              v-auth="'System:Dict:Remove'"
            >
              删除
            </el-button>
          </template>
        </pure-table>
      </template>
    </PureTableBar>
  </div>
</template>
