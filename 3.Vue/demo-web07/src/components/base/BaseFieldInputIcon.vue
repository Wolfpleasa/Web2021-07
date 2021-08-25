<template>
  <div class="d-flex p-relative">
    <input
      class="field-input-icon"
      type="text"
      placeholder="Tìm theo mã, Tên hoặc Số điện thoại"
      v-model="valueFinding"
      @input="startFinding"
    />
    <div
      :class="['input-clear-icon', { 'v-hidden': isHide }]"
      @click="clearFinding"
    ></div>
  </div>
</template>

<script>
export default {
  name: "BaseFieldInputIcon",

  props: {
    searchContent: String,
  },

  data() {
    return {
      isHide: true,
      valueFinding: "",
    };
  },

  methods: {
    /**
     * Hàm bắt sự kiện nhập vào ô tìm kiém
     * Ngọc 14/8/2021
     */
    startFinding() {
      let me = this;
      me.isHide = false;

      if (me.valueFinding == "") {
        me.clearFinding();
      }
      
      me.$emit("input", me.valueFinding);
    },

    /**
     * Hàm bấm vào dấu X
     * Ngọc 14/8/2021
     */
    clearFinding() {
      let me = this;
      me.isHide = true;
      me.valueFinding = "";
      me.$emit("input", me.valueFinding);
    },
  },

  watch: {
    searchContent: function () {
      this.valueFinding = this.searchContent;
    },
  },
};
</script>

<style></style>
