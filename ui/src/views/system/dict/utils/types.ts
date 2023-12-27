interface FormItemProps {
  name: string;
  key: string;
  value: string;
  type: string;
  remark: string;
  sort: number;
  id: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
