<script setup lang="ts">
import { ref, onMounted, reactive } from "vue";
import { getAllKeys, getData, removeKey } from "@/api/system/memorycache";
import { EnsureSuccess } from "@/utils/http/extend";
import { message } from "@/utils/message";
import { ElMessageBox } from "element-plus";

defineOptions({
  name: "Caches"
});

const allKeys = ref([]);
const loading = ref(false);
const form = reactive({
  currentKey: null,
  keyData: null
});

function initData() {
  loading.value = true;
  getAllKeys({}).then(res => {
    loading.value = false;
    if (EnsureSuccess(res)) {
      allKeys.value = res.result;
    }
  });
}

async function getKeyData() {
  if (!form.currentKey) {
    message("请点击左侧列表选择缓存Key", { type: "warning" });
    return;
  }
  const { result } = await getData({ key: form.currentKey });
  form.keyData = result;
}

function handleRowClick(val) {
  if (val) {
    form.currentKey = val.name;
    getKeyData();
  }
}

function handleDelete(row) {
  ElMessageBox.confirm("敏感操作，是否继续？", "提示", {
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    type: "warning"
  })
    .then(() => {
      removeKey({ key: row?.name }).then(res => {
        if (EnsureSuccess(res)) {
          message(res.message, { type: "success" });
          initData();
        } else {
          message(res.message, { type: "error" });
        }
      });
    })
    .catch(() => {});
}

onMounted(() => {
  initData();
});
</script>

<template>
  <el-row>
    <el-col :span="10">
      <el-card shadow="never">
        <template #header>
          <span>内存缓存</span>
          <el-button
            style="float: right; padding: 3px 0"
            @click="initData"
            link
            type="primary"
            >刷新</el-button
          >
        </template>
        <el-table
          v-loading="loading"
          :data="allKeys"
          style="width: 100%"
          highlight-current-row
          @row-click="handleRowClick"
        >
          <el-table-column type="index" width="50" />
          <el-table-column prop="name" label="Key名" />
          <el-table-column label="操作" fixed="right" width="70px">
            <template #default="scope">
              <el-button
                size="small"
                type="danger"
                @click="handleDelete(scope.row)"
                >移除</el-button
              >
            </template>
          </el-table-column>
        </el-table>
      </el-card>
    </el-col>
    <el-col :span="14">
      <el-card shadow="never" class="ml-1">
        <template #header>
          <span>缓存详情</span>
        </template>
        <div v-if="form.keyData">
          <el-form label-position="left" label-width="100px" :model="form">
            <el-form-item label="缓存Key">
              <el-input v-model="form.currentKey" disabled />
            </el-form-item>
            <el-form-item label="数据">
              <el-input
                v-model="form.keyData"
                type="textarea"
                disabled
                :autosize="{ minRows: 4, maxRows: 24 }"
              />
            </el-form-item>
          </el-form>
        </div>
        <el-empty v-else description="无缓存数据" />
      </el-card>
    </el-col>
  </el-row>
</template>
