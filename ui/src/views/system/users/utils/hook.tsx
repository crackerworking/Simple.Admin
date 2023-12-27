import editForm from "../form.vue";
import roleForm from "../role-select.vue";
import { message } from "@/utils/message";
import { ElMessageBox } from "element-plus";
import { usePublicHooks } from "../../hooks";
import { addDialog } from "@/components/ReDialog";
import type { FormItemProps, RoleFormItemProps } from "./types";
import type { PaginationProps } from "@pureadmin/table";
import { reactive, ref, onMounted, h, toRaw } from "vue";
import {
  addUser,
  getUserList,
  resetUserPassword,
  switchUserState,
  setUserRole
} from "@/api/system/users";
import { EnsureSuccess } from "@/utils/http/extend";

export function useUser() {
  const form = reactive({
    userName: null,
    nickName: null,
    sex: null,
    isEnabled: null
  });
  const formRef = ref();
  const roleFormRef = ref();
  const dataList = ref([]);
  const loading = ref(true);
  const switchLoadMap = ref({});
  const { switchStyle } = usePublicHooks();
  const pagination = reactive<PaginationProps>({
    total: 0,
    pageSize: 10,
    currentPage: 1,
    background: true,
    pageSizes: [10, 25, 50, 75, 100]
  });
  const columns: TableColumnList = [
    {
      label: "ID",
      prop: "id",
      width: 180
    },
    {
      label: "用户名",
      prop: "userName"
    },
    {
      label: "昵称",
      prop: "nickName"
    },
    {
      label: "性别",
      cellRenderer: scope => (
        <span v-text={scope.row.sex === 0 ? "女" : "男"}></span>
      )
    },
    {
      label: "状态",
      minWidth: 130,
      cellRenderer: scope => (
        <el-switch
          size={scope.props.size === "small" ? "small" : "default"}
          loading={switchLoadMap.value[scope.index]?.loading}
          v-model={scope.row.isEnabled}
          active-value={1}
          inactive-value={0}
          active-text="已启用"
          inactive-text="已停用"
          inline-prompt
          style={switchStyle.value}
          onChange={() => onChange(scope as any)}
        />
      )
    },
    {
      label: "创建时间",
      minWidth: 120,
      prop: "createdOn"
    },
    {
      label: "操作",
      fixed: "right",
      width: 240,
      slot: "operation"
    }
  ];

  function onChange({ row, _ }) {
    ElMessageBox.confirm(
      `确认要<strong>${
        row.isEnabled === 0 ? "停用" : "启用"
      }</strong><strong style='color:var(--el-color-primary)'>${
        row.userName
      }</strong>吗?`,
      "系统提示",
      {
        confirmButtonText: "确定",
        cancelButtonText: "取消",
        type: "warning",
        dangerouslyUseHTMLString: true,
        draggable: true
      }
    )
      .then(() => {
        switchUserState({ id: row.id }).then(res => {
          if (EnsureSuccess(res)) {
            message(res.message, { type: "success" });
            onSearch();
          } else {
            message(res.message, { type: "error" });
          }
        });
      })
      .catch(() => {
        row.isEnabled = row.isEnabled === 1 ? 0 : 1;
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

  function handleSelectionChange(val) {
    console.log("handleSelectionChange", val);
  }

  function resetPassword(row) {
    ElMessageBox.confirm("确定重置" + row.userName + "密码?", "系统提示", {
      confirmButtonText: "确定",
      cancelButtonText: "取消",
      type: "warning",
      dangerouslyUseHTMLString: true,
      draggable: true
    })
      .then(() => {
        resetUserPassword({ id: row.id }).then(res => {
          if (EnsureSuccess(res)) {
            message(res.message, { type: "success" });
            ElMessageBox.alert("重置后密码：" + res.result);
          } else {
            message(res.message, { type: "error" });
          }
        });
      })
      .catch(() => {});
  }

  function assignRoles(row) {
    addDialog({
      title: `配置角色`,
      props: {
        formInline: {
          userId: row?.id,
          roleIds: row?.roleIds
        }
      },
      width: "40%",
      draggable: true,
      fullscreenIcon: true,
      closeOnClickModal: false,
      contentRenderer: () => h(roleForm, { ref: roleFormRef }),
      beforeSure: (done, { options }) => {
        const RoleFormRef = roleFormRef.value.getRef();
        const curData = options.props.formInline as RoleFormItemProps;
        RoleFormRef.validate(valid => {
          if (valid) {
            setUserRole(curData).then(res => {
              if (EnsureSuccess(res)) {
                message(res.message, { type: "success" });
                done(); // 关闭弹框
                onSearch(); // 刷新表格数据
              } else {
                message(res.message, { type: "error" });
              }
            });
          }
        });
      }
    });
  }

  async function onSearch() {
    loading.value = true;
    const { result: data } = await getUserList({
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

  function openDialog(title = "新增", row?: FormItemProps) {
    addDialog({
      title: `${title}用户`,
      props: {
        formInline: {
          userName: row?.userName ?? ""
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
            // 表单规则校验通过
            if (title === "新增") {
              addUser(curData).then(res => {
                if (EnsureSuccess(res)) {
                  message(res.message, { type: "success" });
                  done(); // 关闭弹框
                  onSearch(); // 刷新表格数据
                } else {
                  message(res.message, { type: "error" });
                }
              });
            } else {
              //
            }
          }
        });
      }
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
    pagination,
    onSearch,
    resetForm,
    openDialog,
    handleSizeChange,
    handleCurrentChange,
    handleSelectionChange,
    resetPassword,
    assignRoles
  };
}
