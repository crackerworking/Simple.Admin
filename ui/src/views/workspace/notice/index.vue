<script setup lang="ts">
import { PureTable } from "@pureadmin/table";
import { useRenderIcon } from "@/components/ReIcon/src/hooks";
import { PureTableBar } from "@/components/RePureTableBar";
import Search from "@iconify-icons/ep/search";
import Refresh from "@iconify-icons/ep/refresh";
import { useUser } from "./utils/hook";
import CheckIcon from "@iconify-icons/ep/check";
import { ref } from "vue";

defineOptions({
  name: "Notice"
});

const formRef = ref();
const tableRef = ref();
const {
  form,
  loading,
  columns,
  dataList,
  pagination,
  onSearch,
  resetForm,
  handleSizeChange,
  handleCurrentChange,
  handleSelectionChange,
  sureReaded,
  selectedNum,
  onSelectionCancel
} = useUser(tableRef);
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
          placeholder="通知标题/内容"
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
      <template v-slot="{ size, dynamicColumns }">
        <div
          v-if="selectedNum > 0"
          v-motion-fade
          class="bg-[var(--el-fill-color-light)] w-full h-[46px] mb-2 pl-4 flex items-center"
        >
          <div class="flex-auto">
            <span
              style="font-size: var(--el-font-size-base)"
              class="text-[rgba(42,46,54,0.5)] dark:text-[rgba(220,220,242,0.5)]"
            >
              已选 {{ selectedNum }} 项
            </span>
            <el-button type="primary" text @click="onSelectionCancel">
              取消选择
            </el-button>
          </div>
          <el-button
            class="mr-1"
            type="primary"
            @click="sureReaded()"
            :icon="useRenderIcon(CheckIcon)"
          >
            批量已读
          </el-button>
        </div>
        <pure-table
          ref="tableRef"
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
              v-if="row.readed === 0"
              class="reset-margin"
              link
              type="primary"
              :size="size"
              :icon="useRenderIcon(CheckIcon)"
              @click="sureReaded(row)"
            >
              已读
            </el-button>
          </template>
        </pure-table>
      </template>
    </PureTableBar>
  </div>
</template>
