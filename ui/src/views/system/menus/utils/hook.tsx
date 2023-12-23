import editForm from "../form.vue";
import { handleTree } from "@/utils/tree";
import { message } from "@/utils/message";
import { getFunctions } from "@/api/system/functions";
import { addDialog } from "@/components/ReDialog";
import { reactive, ref, onMounted, h, toRaw } from "vue";
import type { FormItemProps } from "../utils/types";
import { isAllEmpty } from "@pureadmin/utils";

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
      prop: "icon"
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
      title: `${title}菜单`,
      props: {
        formInline: {
          parentId: row?.parentId,
          functionName: row?.functionName,
          functionType: row?.functionType,
          url: row?.url,
          icon: row?.icon,
          authorizationCode: row?.authorizationCode,
          sort: row?.sort
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
        function chores() {
          message(`您${title}了部门名称为${curData.functionName}的这条数据`, {
            type: "success"
          });
          done(); // 关闭弹框
          onSearch(); // 刷新表格数据
        }
        FormRef.validate(valid => {
          if (valid) {
            console.log("curData", curData);
            // 表单规则校验通过
            if (title === "新增") {
              // 实际开发先调用新增接口，再进行下面操作
              chores();
            } else {
              // 实际开发先调用编辑接口，再进行下面操作
              chores();
            }
          }
        });
      }
    });
  }

  function handleDelete(row) {
    message(`您删除了部门名称为${row.name}的这条数据`, { type: "success" });
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
    /** 新增、编辑部门 */
    openDialog,
    /** 删除部门 */
    handleDelete,
    handleSelectionChange
  };
}
