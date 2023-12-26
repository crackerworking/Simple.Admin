interface FormItemProps {
  parentId: number;
  title: string;
  name: string;
  functionType: number;
  url: string;
  frameSrc: string;
  icon: string;
  authorizationCode: string;
  sort: number;
  id: string;
}
interface FormProps {
  formInline: FormItemProps;
}

export type { FormItemProps, FormProps };
