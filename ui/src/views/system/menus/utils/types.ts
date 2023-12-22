interface FormItemProps {
  parentId: number;
  functionName: string;
  functionType: number;
  url: string;
  icon: string;
  authorizationCode: string;
  sort: number;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
