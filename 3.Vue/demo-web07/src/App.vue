<template>
  <div id="app">
      <router-view></router-view>
  </div>
</template>

<script>
// import EmployeeList from "./view/employee/EmployeeList.vue";
// import CustomerList from "./view/employee/CustomerList.vue";

export default {
  name: 'App',
  components: {
    // EmployeeList,
    // CustomerList,
  }
}
</script>

<style>
  @import "./css/main.css";
  @import "./css/common/common.css";
</style>
