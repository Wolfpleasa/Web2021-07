<template>
  <div :id="idPopup" :class="{ 'd-none': dnone }" :employeeId="employeeId">
    <div class="head">
      <div class="head-text">{{ warning }}</div>
      <div class="head-close" @click="btnCancelOnClick"></div>
    </div>
    <div class="main">
      <div class="warning"></div>
      <div class="text" v-html="warningText"></div>
    </div>
    <div class="foot">
      <Button
        tabindex="0"
        @btn-click="btnConfirmOnClick"
        :buttonText="btnConfirmText"
        id="btnConfirm"
        subClass="confirm d-flex"
      />

      <Button
        tabindex="0"
        @btn-click="btnCancelOnClick"
        :buttonText="btnCancelText"
        id="btnCancel"
        subClass="cancel d-flex"
      />
    </div>
  </div>
</template>

<script>
import Button from "../base/BaseButton.vue";

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
    btnCancelText: String,
    btnConfirmText: String,
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
    btnCancelOnClick() {
      let me = this;
      me.$emit("btnCancelOnClick");
    },
    /**
     * Hàm bấm nút xác nhận xóa
     * Ngọc 29/07/2021
     */
    btnConfirmOnClick() {
      this.$emit("btnConfirmOnClick", this.idPopup);
    },

    /**
     * Hàm tự focus vào nút xác nhận
     * Ngọc 4/8/2021
     */
    // focusOnConfirm() {
    //   this.refs.btnConfirm.focus();
    //   alert("123");
    // },
  },

  // created() {
  //   this.focusOnConfirm();
  // },
};
</script>

<style scoped></style>
