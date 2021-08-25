<template>
  <div class="page-navigator">
    <!-- <div class="ml-10" id="div1-paging"></div> -->
    <div class="ml-10">
      Hiển thị <b>{{ realEntityPerPage }}/{{ totalEntity }}</b> nhân viên
    </div>
    <div class="paging">
      <div
        class="btn common-page first-page"
        @click="pageNumberOnClick('first-page')"
      ></div>
      <div
        class="btn common-page prev-page"
        @click="pageNumberOnClick('prev-page')"
      ></div>
      <div
        v-for="index in totalPageNumber"
        :key="index"
        :class="[
          'btn page-number',
          { 'd-none': index < lowerLimit || index > upperLimit },
          { 'page-selected': index == currentPageNumber },
        ]"
        @click="pageNumberOnClick(index)"
      >
        {{ index }}
      </div>
      <div
        class="btn common-page next-page"
        @click="pageNumberOnClick('next-page')"
      ></div>
      <div
        class="btn common-page last-page"
        @click="pageNumberOnClick('last-page')"
      ></div>
    </div>
    <div class="mr-10 d-flex NumberPerPage">
      <div>
        <b>{{ entityPerPage }}&nbsp;</b>nhân viên/trang
      </div>
      <div class="modifyNumber">
        <div
          class="increaseNumber"
          @click="modifyNumber('increaseNumber')"
        ></div>
        <div
          class="decreaseNumber"
          @click="modifyNumber('decreaseNumber')"
        ></div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "BasePageNavigation",
  props: {
    totalEntity: Number,
    totalPageNumber: Number,
    searchContent: String,
    entityPerPage: Number,
    realEntityPerPage: Number,
  },

  data() {
    return {
      totalShow: 5,
      currentPageNumber: 1,
    
      lowerLimit: 1,
      upperLimit: 5,
    };
  },

  methods: {
    /**
     * Hàm dùng để chuyển trang
     * Ngọc 12/8/2021
     */
    pageNumberOnClick(btnPage) {
      let me = this;
      switch (btnPage) {
        case "first-page":
          me.currentPageNumber = 1;
          break;
        case "prev-page":
          if (me.currentPageNumber > 1) me.currentPageNumber -= 1;
          break;
        case "next-page":
          if (me.currentPageNumber < me.totalPageNumber)
            me.currentPageNumber += 1;
          break;
        case "last-page":
          me.currentPageNumber = me.totalPageNumber;
          break;
        default:
          me.currentPageNumber = btnPage;
          break;
      }
      me.updatePage();

    },

    /**
     * Hàm cập nhật trang đang được xem
     * Ngọc 12/8/2021
     */
    updateCenterNumber() {
      let me = this;
      me.currentPageNumber = Math.min(me.currentPageNumber, me.totalPageNumber);
      me.lowerLimit = me.upperLimit = me.currentPageNumber;
      for (var b = 1; b < me.totalShow && b < me.totalPageNumber; ) {
        if (me.lowerLimit > 1) {
          me.lowerLimit--;
          b++;
        }
        if (b < me.totalShow && me.upperLimit < me.totalPageNumber) {
          me.upperLimit++;
          b++;
        }
      }
    },

    /**
     * Hàm tăng/giảm số lượng thực thể theo trang
     * Ngọc 12/8/2021
     */
    modifyNumber(modifyStatus) {
      let me = this;
      me.$emit("modifyNumber" , modifyStatus);
   
      me.updatePage();
    },

    /**
     * Hàm gọi lên hàm update ở cha
     * Ngọc 22/8/2021
     */
    updatePage() {
      let me = this;
      me.$emit("updatePage", me.currentPageNumber);
      me.updateCenterNumber();
    },
  },

  created() {
    this.updatePage();
  },

  watch: {
    searchContent: function () {   
        this.updatePage();     
    },

    currentPageNumber: function(){
      this.updatePage();
    },

    totalPageNumber: function(){
      this.updatePage();
    },

    totalEntity:function(){
      this.updatePage();
    }
  },
};
</script>

<style></style>