import type { PaginationProps } from "@pureadmin/table";
import { reactive, ref, onMounted, toRaw } from "vue";
import { getLoginLogList } from "@/api/system/login-logs";

export function useLoginLogs() {
  const form = reactive({
    userName: null,
    succeed: null,
    createdOn: null
  });
  const dataList = ref([]);
  const loading = ref(true);
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true,
    pageSizes: [10, 25, 50, 75, 100]
  });
  const columns: TableColumnList = [
    {
      type: "index"
    },
    {
      label: "用户名",
      prop: "userName"
    },
    {
      label: "状态",
      width: 80,
      cellRenderer: scope => (
        <span v-text={scope.row.status === 0 ? "失败" : "成功"}></span>
      )
    },
    {
      label: "响应",
      prop: "operationInfo"
    },
    {
      label: "浏览器",
      prop: "browser"
    },
    {
      label: "操作系统",
      prop: "system"
    },
    {
      label: "IP",
      prop: "ipAddress"
    },
    {
      label: "区域",
      prop: "regionInfo"
    },
    {
      label: "登录时间",
      minWidth: 120,
      prop: "createdOn"
    }
  ];

  function handleSizeChange(val: number) {
    pagination.pageSize = val;
    onSearch();
  }

  function handleCurrentChange(val: number) {
    pagination.currentPage = val;
    onSearch();
  }

  function handleSelectionChange(val) {
    console.log("handleSelectionChange", val);
  }

  async function onSearch() {
    loading.value = true;
    const { result: data } = await getLoginLogList({
      ...toRaw(form),
      page: pagination.currentPage,
      size: pagination.pageSize
    });
    dataList.value = data.rows;
    pagination.total = data.total;

    setTimeout(() => {
      loading.value = false;
    }, 500);
  }

  const resetForm = formEl => {
    console.log(formEl);
    if (!formEl) return;
    formEl.resetFields();
    onSearch();
  };

  onMounted(() => {
    onSearch();
  });

  return {
    form,
    loading,
    columns,
    dataList,
    pagination,
    onSearch,
    resetForm,
    handleSizeChange,
    handleCurrentChange,
    handleSelectionChange
  };
}
