<template>
  <div :class="className">
    <div class="select">
      <div class="inp" @click="SelectonClick">{{ currentName }}</div>
      <div :class="['X', { 'v-hidden': HideX }]" @click="clearChoice"></div>
      <div
        :class="['select-arrow', rotate ? 'rot-180' : 'rot-0']"
        @click="SelectonClick"
      ></div>
    </div>
   
      <div
        :class="['dropdown', dd_dropdown,{ 'd-none': dnone }  ]"
        :Url="Url"
        :itemId="itemId"
        :itemName="itemName"
      > 
      <slide-up-down :active="active" :duration="1000"  >
        <div
          :class="[
            'dropdown-item',
            currentId == item[itemId] ? 'bg-select' : '',
          ]"
          @click="chooseDropdownItem(item[itemId], item[itemName])"
          v-for="item in items"
          :key="item[itemId]"
          :Value="item[itemId]"
        >
          <div class="dropdown-icon"></div>
          <div class="dropdown-text">{{ item[itemName] }}</div>
        </div>
         </slide-up-down>
      </div>
   
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "BaseDropdown",
  props: {
    defaultName: String,
    className: String,
    dd_dropdown: String,
    Url: String,
    itemId: String,
    itemName: String,
    selectedId: String,
  },

  data() {
    return {
      items: [],
      HideX: true,
      rotate: false,
      dnone: true,
      currentId: -1,
      currentName: this.defaultName,
      active: false,
    };
  },
  methods: {
    /**
     * Sự kiện bấm arrow
     * Ngọc 31/07/2021
     */
    SelectonClick() {
      this.rotate = !this.rotate;
      this.dnone = !this.dnone;
      this.active = !this.active;
    },

    /**
     * Sự kiện chọn 1 lựa chọn
     * Ngọc 31/07/2021
     */
    chooseDropdownItem(itemValue, itemName) {
      this.currentId = itemValue;
      this.HideX = false;
      this.SelectonClick();
      this.currentName = itemName;
      this.$emit("chooseDropdownItem", itemValue, this.itemId);
    },

    /**
     * Sự kiện bấm nút X để xóa lựa chọn
     * Ngọc 31/07/2021
     */
    clearChoice() {
      this.currentId = "";
      this.HideX = true;
      this.currentName = this.defaultName;
      this.dnone = true;

      if (this.rotate) {
        this.rotate = !this.rotate;
      }
      if (this.active) {
        this.active = !this.active;
      }
    },

    /**
     * xem hàm sửa có gọi không để lưu giá trị vào dropdown
     * Ngọc 31/07/2021
     */
    setValueDropdown() {
      let me = this;
      if ((me.selectedId + "").length > 0) {
        me.items.forEach(function (item) {
          if (me.selectedId == item[me.itemId]) {
            me.currentId = item[me.itemId];
            me.currentName = item[me.itemName];
          }
        });
      } else {
        me.currentId = -1;
        me.currentName = " ";
      }
    },
  },

  created() {
    let me = this;
    switch (me.itemName) {
      case "GenderName":
        this.items = [
          {
            Gender: 0,
            GenderName: "Nữ",
          },
          {
            Gender: 1,
            GenderName: "Nam",
          },
          {
            Gender: 2,
            GenderName: "Khác",
          },
        ];
        break;
      case "WorkStatusName":
        this.items = [
          {
            WorkStatus: 0,
            WorkStatusName: "Chờ phỏng vấn",
          },
          {
            WorkStatus: 1,
            WorkStatusName: "Thử việc",
          },
          {
            WorkStatus: 2,
            WorkStatusName: "Đang làm việc",
          },
          {
            WorkStatus: 3,
            WorkStatusName: "Bị đuổi việc",
          },
        ];
        break;

      default:
        axios
          .get(`http://cukcuk.manhnv.net/${me.Url}`)
          .then((res) => {
            me.items = res.data;
          })
          .catch((res) => {
            console.log(res);
          });
        break;
    }

    me.currentName = me.defaultName;
  },

  watch: {
    selectedId: function () {
      this.setValueDropdown();
    },
  },
};
</script>

<style></style>
