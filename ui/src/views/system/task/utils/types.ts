interface FormItemProps {
  id: string;
  name: string;
  isEnabled: number;
  remark: string;
  cron: string;
  extraParams: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
