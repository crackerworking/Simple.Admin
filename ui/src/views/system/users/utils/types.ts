interface FormItemProps {
  userName: string;
}
interface FormProps {
  formInline: FormItemProps;
}

interface RoleFormItemProps {
  userId: string;
  roleIds: Array<string>;
}

interface RoleFormProps {
  formInline: RoleFormItemProps;
}

export type { FormItemProps, FormProps, RoleFormItemProps, RoleFormProps };
