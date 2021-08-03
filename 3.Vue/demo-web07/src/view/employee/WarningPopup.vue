<template>
  <div
    :id= "idPopup"
    :class="{ 'd-none': dnone }"
    :employeeId= "employeeId"
  >
    <div class="head">
      <div class="head-text">{{warning}}</div>
      <div class="head-close" @click="btnCancelonClick"></div>
    </div>
    <div class="main">
      <div class="warning"></div>
      <div class="text">{{warningText}}</div>
    </div>
    <div class="foot">
      <Button
        @btn-click="btnConfirmonClick"
        :buttonText="btnConfirmText"
        id="btnConfirm"
        subClass="confirm d-flex"
      />

      <Button
        @btn-click="btnCancelonClick"
        :buttonText="btnCancelText"
        id="btnCancel"
        subClass="cancel d-flex"
      />
    </div>
  </div>
</template>

<script>

import Button from "../../components/base/BaseButton.vue";

export default {
  name: "WarningPopup",
  components: {
    Button,
  },

  props: {
    dnone: {
      type: Boolean,
      default: true,
      require: true,
    },
    employeeId: {
      type: String,
      default: "",
      require: true,
    },
    warning: String,
    warningText: String,
    idPopup: String,
    btnCancelText:String,
    btnConfirmText:String,
  },

  data() {
    return {
      employee: {},
    };
  },
  methods: {
    /**
     * Hàm đóng popup
     * Ngọc 29/07/2021
     */
    btnCancelonClick() {
      let me = this;
      me.$emit("btnCancelonClick");
    },
    /**
     * Hàm bấm nút xác nhận xóa
     * Ngọc 29/07/2021
     */
    btnConfirmonClick() {
     this.$emit("btnConfirmonClick" , this.idPopup);
    },
  },
};
</script>

<style scoped>
</style>