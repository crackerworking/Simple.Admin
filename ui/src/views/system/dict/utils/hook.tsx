import editForm from "../form.vue";
import { message } from "@/utils/message";
import {
  getDictList,
  addDict,
  updateDict,
  deleteDict
} from "@/api/system/dict";
import { addDialog } from "@/components/ReDialog";
import { reactive, ref, onMounted, h, toRaw } from "vue";
import type { FormItemProps } from "../utils/types";
import { EnsureSuccess } from "@/utils/http/extend";
import { ElMessageBox } from "element-plus";
import type { PaginationProps } from "@pureadmin/table";

export function useDict() {
  const form = reactive({
    vague: null,
    remark: null,
    type: null
  });

  const formRef = ref();
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
      label: "名称",
      prop: "name"
    },
    {
      label: "键/key",
      prop: "key"
    },
    {
      label: "值",
      prop: "value"
    },
    {
      label: "分类",
      prop: "type"
    },
    {
      label: "备注",
      prop: "remark"
    },
    {
      label: "排序",
      prop: "sort"
    },
    {
      label: "操作",
      fixed: "right",
      width: 160,
      slot: "operation"
    }
  ];

  function handleSelectionChange(val) {
    console.log("handleSelectionChange", val);
  }

  function resetForm(formEl) {
    if (!formEl) return;
    formEl.resetFields();
    onSearch();
  }

  async function onSearch() {
    loading.value = true;
    const { result: data } = await getDictList({
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

  function openDialog(title = "新增", row?: FormItemProps) {
    addDialog({
      title: `${title}字典`,
      props: {
        formInline: {
          name: row?.name,
          key: row?.key,
          value: row?.value,
          type: row?.type,
          remark: row?.remark,
          sort: row?.sort ?? 1,
          id: row?.id ?? 0
        }
      },
      width: "40%",
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      contentRenderer: () => h(editForm, { ref: formRef }),
      beforeSure: (done, { options }) => {
        const FormRef = formRef.value.getRef();
        const curData = options.props.formInline as FormItemProps;
        FormRef.validate(valid => {
          if (valid) {
            if (title === "新增") {
              addDict(curData).then(res => {
                if (EnsureSuccess(res)) {
                  message(res.message, { type: "success" });
                  done(); // 关闭弹框
                  onSearch(); // 刷新表格数据
                } else {
                  message(res.message, { type: "error" });
                }
              });
            } else {
              updateDict(curData).then(res => {
                if (EnsureSuccess(res)) {
                  message(res.message, { type: "success" });
                  done();
                  onSearch();
                } else {
                  message(res.message, { type: "error" });
                }
              });
            }
          }
        });
      }
    });
  }

  function removeDict(row) {
    ElMessageBox.confirm("确定删除【" + row.title + "】吗?", "系统提示", {
      confirmButtonText: "确定",
      cancelButtonText: "取消",
      type: "warning",
      dangerouslyUseHTMLString: true,
      draggable: true
    }).then(() => {
      deleteDict({ array_id: [row.id] }).then(res => {
        if (EnsureSuccess(res)) {
          message(res.message, { type: "success" });
          onSearch();
        } else {
          message(res.message, { type: "error" });
        }
      });
    });
  }

  function handleSizeChange(val: number) {
    pagination.pageSize = val;
    onSearch();
  }

  function handleCurrentChange(val: number) {
    pagination.currentPage = val;
    onSearch();
  }

  onMounted(() => {
    onSearch();
  });

  return {
    form,
    loading,
    columns,
    dataList,
    /** 搜索 */
    onSearch,
    /** 重置 */
    resetForm,
    /** 新增、编辑功能 */
    openDialog,
    removeDict,
    handleSelectionChange,
    pagination,
    handleSizeChange,
    handleCurrentChange
  };
}
