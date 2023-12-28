import editForm from "../form.vue";
import { message } from "@/utils/message";
import { addDialog } from "@/components/ReDialog";
import type { FormItemProps } from "../utils/types";
import { reactive, ref, onMounted, h, toRaw } from "vue";
import { getTaskList, updateTask } from "@/api/system/task";
import { EnsureSuccess } from "@/utils/http/extend";

export function useTask() {
  const form = reactive({
    roleName: null,
    remark: null
  });
  const formRef = ref();
  const dataList = ref([]);
  const loading = ref(true);
  const columns: TableColumnList = [
    {
      label: "名称",
      prop: "taskName"
    },
    {
      label: "状态",
      cellRenderer: scope =>
        scope.row.isEnabled === 1 ? (
          <el-tag type="success">已启动</el-tag>
        ) : (
          <el-tag type="danger">已暂停</el-tag>
        )
    },
    {
      label: "额外参数",
      prop: "extraParams"
    },
    {
      label: "cron表达式",
      prop: "cron"
    },
    {
      label: "备注",
      prop: "remark"
    },
    {
      label: "执行时间",
      prop: "executedTime",
      minWidth: 120
    },
    {
      label: "操作",
      fixed: "right",
      width: 80,
      slot: "operation"
    }
  ];
  async function onSearch() {
    loading.value = true;
    const { result } = await getTaskList(toRaw(form));
    dataList.value = result;

    setTimeout(() => {
      loading.value = false;
    }, 500);
  }

  const resetForm = formEl => {
    if (!formEl) return;
    formEl.resetFields();
    onSearch();
  };

  function openDialog(title = "编辑", row?: FormItemProps | any) {
    addDialog({
      title: `${title}定时任务`,
      props: {
        formInline: {
          id: row?.id ?? "",
          name: row?.taskName ?? "",
          isEnabled: row?.isEnabled ?? 0,
          extraParams: row?.extraParams ?? 0,
          cron: row?.cron ?? 0,
          remark: row?.remark ?? ""
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
            updateTask(curData).then(res => {
              if (EnsureSuccess(res)) {
                message(res.message, { type: "success" });
                done();
                onSearch();
              } else {
                message(res.message, { type: "error" });
              }
            });
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
    onSearch,
    resetForm,
    openDialog
  };
}
