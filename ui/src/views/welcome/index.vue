<script setup lang="ts">
defineOptions({
  name: "Welcome"
});
</script>

<template>
  <div>
    <el-card class="mb-2">
      <el-descriptions title="Simple.Admin">
        <el-descriptions-item label="联系作者"
          >crackerwork@outlook.com</el-descriptions-item
        >
        <el-descriptions-item label="github"
          ><el-link
            href="https://github.com/crackerworking/Simple.Admin"
            type="primary"
            target="_blank"
            >https://github.com/crackerworking/Simple.Admin</el-link
          ></el-descriptions-item
        >
        <el-descriptions-item label="gitee"
          ><el-link
            href="https://gitee.com/crackerworking/simple-admin"
            type="primary"
            target="blank"
            >https://gitee.com/crackerworking/simple-admin</el-link
          ></el-descriptions-item
        >
      </el-descriptions>
    </el-card>
    <el-card>
      项目名叫做<b>Simple.Admin</b>，意为简单，简化开发。框架基于.NET8.0，使用AspNetCore+Vue.js(前端项目基于<a
        href="https://github.com/pure-admin/vue-pure-admin"
        >pure-admin</a
      >)构建的后台管理，实现了页面、按钮（接口）权限拦截。整体项目技术简单，非常适合作为初学项目和升级到企业后台，并且有自定义无权限页面、禁止访问错误页面、动态定时任务、内存缓存key、登录日志、右下角弹窗通知等特色功能。
    </el-card>
  </div>
</template>
