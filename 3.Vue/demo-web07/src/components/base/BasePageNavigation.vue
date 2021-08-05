<template>
     <div class="page-navigator">
        <!-- <div class="ml-10" id="div1-paging"></div> -->
        <div class="ml-10">Hiển thị 1-10/1000 nhân viên</div>
        <div class="paging">
          <div class="btn common-page first-page"></div>
          <div class="btn common-page prev-page"></div>
          <div class="btn page-number">1</div>
          <div class="btn page-number">2</div>
          <div class="btn page-number">3</div>
          <div class="btn page-number">4</div>
          <div class="btn common-page next-page"></div>
          <div class="btn common-page last-page"></div>
        </div>
        <div class="mr-10" id="div2-paging">10 nhân viên/trang</div>
      </div>
</template>

<script>
export default {
  name: "BasePageNavigation",
  props: {
      
  },
};
</script>

<style></style>