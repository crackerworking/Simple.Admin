export default {
  path: "/system",
  meta: {
    icon: "uil:setting",
    title: "系统管理"
  },
  children: [
    {
      path: "/users/index",
      name: "Users",
      component: () => import("@/views/system/users/index.vue"),
      meta: {
        title: "用户管理"
      }
    },
    {
      path: "/roles/index",
      name: "Roles",
      component: () => import("@/views/system/roles/index.vue"),
      meta: {
        title: "角色管理"
      }
    },
    {
      path: "/functions/index",
      name: "Functions",
      component: () => import("@/views/system/functions/index.vue"),
      meta: {
        title: "功能管理"
      }
    }
  ]
};
