<template>
  <div :class="className">
    <div
      class="select"
      :tabindex="tabindex"
      v-on:keyup.enter="SelectOnClick"
      v-on:keydown="keydownOnSelect($event)"
      v-on:keyup.space="
        chooseDropdownItemByKey($event)
      "
    >
      <div class="inp" @click="SelectOnClick">{{ currentName }}</div>
      <div :class="['X', { 'v-hidden': HideX }]" @click="clearChoice"></div>
      <div
        :class="['select-arrow', rotate ? 'rot-180' : 'rot-0']"
        @click="SelectOnClick"
      ></div>
    </div>

    <div
      :class="['dropdown', dd_dropdown, { 'd-none': dnone }]"
      :Url="Url"
      :itemId="itemId"
      :itemName="itemName"
    >
      <slide-up-down :active="active" :duration="1000">
        <div
          v-for="(item, index) in items"
          :key="item[itemId]"
          :Value="item[itemId]"
          :class="['dropdown-item', index == currentIndex ? 'bg-select' : '']"
          @click="chooseDropdownItem(item[itemId], item[itemName], index)"
          
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
    tabindex: String,
  },

  data() {
    return {
      items: [],
      HideX: true,
      rotate: false,
      dnone: true,
      currentName: this.defaultName,
      currentIndex: -1,
      active: false,
    };
  },
  methods: {
    /**
     * Hàm ngăn chặn sự kiện mặc định của nút tab, dùng thì mới thực hiện được nút enter
     * Ngọc 4/8/2021
     */
    keydownOnSelect(event) {
      if (event.code == "Enter") {
        event.preventDefault();
      }
      if (event.code == "ArrowDown") {
        event.preventDefault();
        this.currentIndex = this.currentIndex < 0 ? -1 : this.currentIndex;
        this.currentIndex = (this.currentIndex + 1) % this.items.length;
        
      } else if (event.code == "ArrowUp") {
        event.preventDefault();
        this.currentIndex = this.currentIndex < 0 ? 0 : this.currentIndex;
        this.currentIndex =
          this.currentIndex == 0
            ? this.items.length - 1
            : this.currentIndex - 1;
      }
    },

    /**
     * Sự kiện chọn 1 lựa chọn bằng phím
     * Ngọc 31/07/2021
     */
    chooseDropdownItemByKey(event) {
      if (event.code == "Space") {
        event.preventDefault();
        let item = this.items[this.currentIndex],
            itemName = item[this.itemName],
            itemValue = item[this.itemId];
        this.chooseDropdownItem(itemValue, itemName, this.currentIndex)
      }
    },

    /**
     * Sự kiện chọn 1 lựa chọn
     * Ngọc 31/07/2021
     */
    chooseDropdownItem(itemValue, itemName, index) {
      this.currentIndex = index;
      this.HideX = false;
      this.SelectOnClick();
      this.currentName = itemName;
      this.$emit("chooseDropdownItem", itemValue, this.itemId);
    },
    
    /**
     * Sự kiện bấm để hiện/ ẩn dropdown
     * Ngọc 31/07/2021
     */
    SelectOnClick() {
      this.rotate = !this.rotate;
      this.dnone = !this.dnone;
      this.active = !this.active;
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
        me.items.forEach(function (item, index) {
          if (me.selectedId == item[me.itemId]) {
            me.currentIndex = index;
            me.currentName = item[me.itemName];
          }
        });
      } else {
        me.currentId = -1;
        me.currentIndex = -1;
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
