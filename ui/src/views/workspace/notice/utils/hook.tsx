import { message } from "@/utils/message";
import { ElMessageBox } from "element-plus";
import type { PaginationProps } from "@pureadmin/table";
import { reactive, ref, onMounted, toRaw } from "vue";
import { getMessageList, readed } from "@/api/workspace/notice";
import { EnsureSuccess } from "@/utils/http/extend";

export function useUser(tableRef) {
  const form = reactive({
    vague: null
  });
  const dataList = ref([]);
  const loading = ref(true);
  const selectedNum = ref(0);
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true,
    pageSizes: [10, 25, 50, 75, 100]
  });
  const columns: TableColumnList = [
    {
      type: "selection",
      width: 50
    },
    {
      label: "ID",
      prop: "id",
      width: 180
    },
    {
      label: "标题",
      prop: "title"
    },
    {
      label: "内容",
      minWidth: 200,
      prop: "content"
    },
    {
      label: "状态",
      cellRenderer: scope =>
        scope.row.readed === 1 ? (
          <el-tag type="success">已读</el-tag>
        ) : (
          <el-tag type="info">未读</el-tag>
        )
    },
    {
      label: "创建时间",
      minWidth: 180,
      prop: "createdOn"
    },
    {
      label: "操作",
      fixed: "right",
      width: 80,
      slot: "operation"
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
    selectedNum.value = val.length;
  }

  /** 取消选择 */
  function onSelectionCancel() {
    selectedNum.value = 0;
    // 用于多选表格，清空用户的选择
    tableRef.value.getTableRef().clearSelection();
  }

  function sureReaded(row?) {
    let ids = [row?.id];
    if (!row) {
      const checkedRows = tableRef.value.getTableRef().getSelectionRows();
      ids = checkedRows.map(x => x.id);
    }
    if (ids.length === 0) {
      message("至少选择一条数据", { type: "warning" });
      return;
    }
    ElMessageBox.confirm("确定标记已读吗?", "系统提示", {
      confirmButtonText: "确定",
      cancelButtonText: "取消",
      type: "warning",
      dangerouslyUseHTMLString: true,
      draggable: true
    })
      .then(() => {
        readed({ array_id: ids }).then(res => {
          if (EnsureSuccess(res)) {
            message(res.message, { type: "success" });
            onSearch();
          } else {
            message(res.message, { type: "error" });
          }
        });
      })
      .catch(() => {});
  }

  async function onSearch() {
    loading.value = true;
    const { result: data } = await getMessageList({
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
    sureReaded,
    handleSizeChange,
    handleCurrentChange,
    handleSelectionChange,
    onSelectionCancel,
    selectedNum
  };
}
