interface FormItemProps {
  username: string;
  remark: string;
  status: number;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
