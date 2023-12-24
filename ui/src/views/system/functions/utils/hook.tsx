import editForm from "../form.vue";
import { handleTree } from "@/utils/tree";
import { message } from "@/utils/message";
import {
  getFunctions,
  addFunction,
  updateFunction,
  deleteFunction
} from "@/api/system/functions";
import { addDialog } from "@/components/ReDialog";
import { reactive, ref, onMounted, h, toRaw } from "vue";
import type { FormItemProps } from "../utils/types";
import { EnsureSuccess } from "@/utils/http/extend";
import { ElMessageBox } from "element-plus";
import { Icon } from "@iconify/vue";

export function useMenu() {
  const form = reactive({
    functionName: "",
    functionType: null
  });

  const formRef = ref();
  const dataList = ref([]);
  const loading = ref(true);

  const columns: TableColumnList = [
    {
      label: "菜单名称",
      prop: "functionName",
      width: 180,
      align: "left"
    },
    {
      label: "图标",
      width: 60,
      cellRenderer: scope => <Icon icon={scope.row.icon} />
    },
    {
      label: "地址",
      prop: "url"
    },
    {
      label: "权限编码",
      prop: "authorizationCode"
    },
    {
      label: "排序",
      prop: "sort",
      minWidth: 70
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
    const { result } = await getFunctions(toRaw(form));
    dataList.value = handleTree(result); // 处理成树结构
    setTimeout(() => {
      loading.value = false;
    }, 500);
  }

  function openDialog(title = "新增", row?: FormItemProps) {
    addDialog({
      title: `${title}功能`,
      props: {
        formInline: {
          parentId: row?.parentId <= 0 ? "" : row?.parentId,
          functionName: row?.functionName,
          functionType: row?.functionType,
          url: row?.url,
          icon: row?.icon,
          authorizationCode: row?.authorizationCode,
          sort: row?.sort,
          id: row?.id
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
            console.log("curData", curData);
            // 表单规则校验通过
            if (title === "新增") {
              addFunction(curData).then(res => {
                if (EnsureSuccess(res)) {
                  message(res.message, { type: "success" });
                  done(); // 关闭弹框
                  onSearch(); // 刷新表格数据
                } else {
                  message(res.message, { type: "error" });
                }
              });
            } else {
              updateFunction(curData).then(res => {
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

  function removeFunction(row) {
    ElMessageBox.confirm(
      "确定删除【" + row.functionName + "】吗?",
      "系统提示",
      {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        dangerouslyUseHTMLString: true,
        draggable: true
      }
    ).then(() => {
      deleteFunction({ array_id: [row.id] }).then(res => {
        if (EnsureSuccess(res)) {
          message(res.message, { type: "success" });
          onSearch();
        } else {
          message(res.message, { type: "error" });
        }
      });
    });
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
    /** 删除功能 */
    removeFunction,
    handleSelectionChange
  };
}
