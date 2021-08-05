<template>
  <div>
    <Header />
    <Menu :isToggle="isToggle" @toggleMenu="toggleMenu" />
    <div :class="['content', { narrow: isToggle }, { expand: !isToggle }]">
      <div class="content-header">
        <div class="title">Danh sách nhân viên</div>
        <div class="d-flex">
          <Button
            @btn-click="btnDeleteOnClick"
            iconName="icon-delete"
            buttonText="Xóa nhân viên"
            id="btnDelete"
            :class="{ 'v-hidden': HideBtnDelete }"
          />
          <Button
            @btn-click="btnAddOnClick"
            iconName="icon-add"
            buttonText="Thêm nhân viên"
            id="btnAdd"
          />
        </div>
      </div>
      <div class="filter">
        <div class="d-flex">
          <FieldInputIcon />
          <Dropdown
            className="department"
            defaultName="Tất cả phòng ban"
            dd_dropdown="dd-Department"
            Url="api/Department"
            itemId="DepartmentId"
            itemName="DepartmentName"
          />
          <Dropdown
            className="position"
            defaultName="Tất cả vị trí"
            dd_dropdown="dd-Position"
            Url="v1/Positions"
            itemId="PositionId"
            itemName="PositionName"
          />
        </div>
        <div class="refresh"></div>
      </div>
      <div id="gridEntity">
        <table>
          <thead>
            <tr>
              <th>
                <div
                  :class="['checkbox', { checked: checked }]"
                  @click="clickCheckboxTh"
                ></div>
              </th>
              <th>Mã nhân viên</th>
              <th>Họ và tên</th>
              <th>Giới tính</th>
              <th>Ngày sinh</th>
              <th>Điện thoại</th>
              <th>Email</th>
              <th>Chức vụ</th>
              <th>Phòng ban</th>
              <th>Mức lương cơ bản</th>
              <th>Tình trạng công việc</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(employee, index) in employees"
              :key="employee.EmployeeId"
              @dblclick="onDoubleClick(employee.EmployeeId)"
              @click="trOnClick(index)"
              :class="{ 'tr-select': isSelected[index] }"
            >
              <td>
                <CheckBox
                  @clickCheckboxTd="clickCheckboxTd"
                  :checked="isSelected[index]"
                />
              </td>
              <td>{{ employee.EmployeeCode }}</td>
              <td>{{ employee.FullName }}</td>
              <td>{{ employee.GenderName }}</td>
              <td>{{ convertDate(employee.DateOfBirth) }}</td>
              <td>{{ employee.PhoneNumber }}</td>
              <td>{{ employee.Email }}</td>
              <td>{{ employee.PositionName }}</td>
              <td>{{ employee.DepartmentName }}</td>
              <td>{{ formatMoney(employee.Salary) }}</td>
              <td>{{ employee.WorkStatus }}</td>
            </tr>
          </tbody>
        </table>
      </div>
      <PageNavigation />
    </div>
    <EmployeeDetail
      v-bind:dnone="DialogHasDnone"
      :employeeId="employeeId"
      :formMode="formMode"
      @btnDialogCancelOnClick="btnDialogCancelOnClick"
      @btnSaveOnClick="btnSaveOnClick"
      @callToastMessage="callToastMessage"
      :reopen="reopen"
    />

    <WarningPopup
      :idPopup="idPopup"
      v-bind:dnone="WarningHasDnone"
      :employeeId="employeeId"
      @btnCancelOnClick="btnCancelOnClick"
      :warning="warning"
      :warningText="warningText"
      :btnCancelText="btnCancelText"
      :btnConfirmText="btnConfirmText"
      @btnConfirmOnClick="btnConfirmOnClick"
    />

    <ToastMessage
      :subClass="subClass"
      :HideToastMessage="HideToastMessage"
      :ToastMessageText="ToastMessageText"
      @closeToastMessage="closeToastMessage"
      :active="active"
    />
  </div>
</template>

<script>
import axios from "axios";
import { CommonFn } from "../../js/mixins.js";

import Header from "../../components/layout/TheHeader.vue";
import Menu from "../../components/layout/TheMenu.vue";
import Button from "../../components/base/BaseButton.vue";
import FieldInputIcon from "../../components/base/BaseFieldInputIcon.vue";
import Dropdown from "../../components/base/BaseDropdown.vue";
import CheckBox from "../../components/base/BaseCheckBox.vue";
import ToastMessage from "../../components/base/BaseToastMessage.vue";
import PageNavigation from "../../components/base/BasePageNavigation.vue";

import EmployeeDetail from "./EmployeeDetail.vue";
import WarningPopup from "../../components/layout/WarningPopup.vue";
export default {
  mixins: [CommonFn],
  name: "EmployeeList",
  components: {
    Header,
    Menu,
    Button,
    Dropdown,
    CheckBox,
    ToastMessage,
    FieldInputIcon,
    PageNavigation,
    EmployeeDetail,
    WarningPopup,
  },
  data() {
    return {
      //EmployeeList
      employees: [],
      //checkbox,tr
      isSelected: [],
      checked: false,
      //button delete
      HideBtnDelete: true,
      //ToastMessage
      HideToastMessage: true,
      ToastMessageText: "",
      //EmployeeDetail
      DialogHasDnone: true,
      employeeId: null,
      reopen: true,
      formMode: -1,
      subClass: "",
      // popup
      WarningHasDnone: true,
      warning: "",
      warningText: "",
      idPopup: "",
      btnCancelText: "",
      btnConfirmText: "",
      //menu
      isToggle: true,
      //silde
      active: false,
    };
  },

  methods: {
    /**
     * Hàm mở popup
     * Ngọc 29/07/2021
     */
    btnAddOnClick() {
      let me = this;
      me.DialogHasDnone = false;
      this.formMode = 0;
    },

   

    /**
     * Hàm mở form detail để sửa
     * Ngọc 29/07/2021
     */
    onDoubleClick(employeeId) {
      this.DialogHasDnone = false;
      this.employeeId = employeeId;
      this.formMode = 1;
      this.reopen = !this.reopen;
    },

    /**
     * Hàm bấm checkbox ở td được BaseCheckBox gửi lên
     * Ngọc 1/8/2021
     */
    clickCheckboxTd(index) {
      let me = this;
      me.isSelected[index] = !me.isSelected[index];
      me.HideBtnDelete = false;
      setTimeout(function () {
        me.checked = me.CheckAllCBTd();
      }, 10);
    },

    /**
     * Hàm kiểm tra tất cả checkbox của td có được chọn không
     * Ngọc 1/8/2021
     */
    CheckAllCBTd() {
      let me = true;
      for (var i = 0; i < this.isSelected.length; i++) {
        if (!this.isSelected[i]) {
          me = false;
          break;
        }
      }
      return me;
    },

    /**
     * Hàm bấm checkbox ở th được BaseCheckBox gửi lên
     * Ngọc 1/8/2021
     */
    clickCheckboxTh() {
      this.checked = !this.checked;
      if (this.checked) {
        this.isSelected.fill(true);
        this.HideBtnDelete = false;
      } else {
        this.isSelected.fill(false);
        this.HideBtnDelete = true;
      }
    },

    /**
     * Hàm chọn 1 hàng
     * Ngọc 29/07/2021
     */
    trOnClick(index) {
      this.$set(this.isSelected, index, !this.isSelected[index]);
      this.HideBtnDelete = false;
      this.CheckRowIsSelected();
    },

    /**
     * Hàm kiểm tra các hàng vẫn đang được chọn để hiện nút xóa
     * Ngọc 1/8/2021
     */
    CheckRowIsSelected() {
      let me = this;
      this.HideBtnDelete = true;
      this.isSelected.forEach(function (item) {
        if (item) {
          me.HideBtnDelete = false;
          return;
        }
      });
    },

    /**
     * Hàm bấm nút xóa
     * Ngọc 30/07/2021
     */
    btnDeleteOnClick() {
      let me = this;
      var count = 0;
      this.isSelected.forEach((selected) => {
        if (selected) {
          count = count + 1;
        }
      });
      if (count > 1) {
        me.warning = `Xóa thông tin của ${count} nhân viên`;
        me.warningText = `Xác nhận xóa thông tin của <b>${count} nhân viên </b> này không`;
      } else {
        let fullname = "";
        this.isSelected.forEach((selected, index) => {
          if (selected) {
            fullname = me.employees[index].FullName;
          }
        });
        me.warning = `Xóa thông tin của nhân  viên ${fullname}`;
        me.warningText = `Xác nhận xóa thông tin của nhân viên <b>${fullname} </b>`;
      }
      me.WarningHasDnone = false;
      me.idPopup = "warning-popup";
      me.btnCancelText = "Hủy";
      me.btnConfirmText = "Xóa";
    },

    /**
     * Hàm bấm nút xác nhận 
     * Ngọc 30/07/2021
     */
    btnConfirmOnClick(idPopup) {
      let me = this;
      if (idPopup == "warning-popup") {
        this.isSelected.forEach((selected, index) => {
          if (selected) {
            me.employeeId = me.employees[index]["EmployeeId"];
            axios
              .delete(`http://cukcuk.manhnv.net/v1/Employees/${me.employeeId}`)
              .then(() => {
                me.callToastMessage("Xóa dữ liệu thành công", "message-green");
                me.isSelected = [];
                me.loadDataTable();
              })
              .catch((err) => {
                console.log(err);
              });
          }
        });
        me.WarningHasDnone = true;
        me.HideBtnDelete = true;
      } else {
        me.WarningHasDnone = true;
        me.DialogHasDnone = true;
        me.formMode = -1;
      }
      me.idPopup = "";
    },

    /**
     * Hàm loaddata cho table
     * Ngọc 30/07/2021
     */
    loadDataTable() {
      let me = this;
      axios
        .get("http://cukcuk.manhnv.net/v1/Employees")
        .then((res) => {
          me.employees = res.data.slice(0, 100);
          me.resetTr();
        })
        .catch((res) => {
          console.log(res);
        });
    },

    /**
     * Hàm reset các tr về không được chọn
     * Ngọc 30/07/2021
     */
    resetTr() {
      let me = this;
      me.employees.forEach(() => {
        me.isSelected.push(false);
      });
    },

     /**
     * Hàm đóng popup
     * Ngọc 29/07/2021
     */
    btnCancelOnClick() {
      let me = this;
      me.WarningHasDnone = true;
    },

    /**
     * Hàm bấm hủy hoặc nút X ở form detail 
     * Ngọc 30/07/2021
     */
    btnDialogCancelOnClick() {
      let me = this;
      let custom = "";
      me.WarningHasDnone = false;
      me.idPopup = "notify-popup";
      me.warning = "Đóng Form thông tin chung ";
      if (this.formMode == 0) {
        custom = "nhập";
      } else {
        custom = "sửa";
      }
      me.warningText = `Bạn có chắc muốn đóng thông tin form ${custom} <b>"Thông tin nhân viên"</b> hay không`;
      me.btnCancelText = "Tiếp tục nhập";
      me.btnConfirmText = "Đóng";
    },

    /**
     * Hàm bấm lưu xong thì đóng popup được EmployeeDetail gửi lên
     * Ngọc 30/07/2021
     */
    btnSaveOnClick() {
      //Đống form thêm/sửa
      this.DialogHasDnone = true;
      // reset formMode
      this.formMode = -1;
      // reset mảng đánh dấu
      this.isSelected = [];
      //tải lại dữ liệu cho table
      this.loadDataTable();
    },

    /**
     * Hàm hiển thị toastmessage
     * Ngọc 2/8/2021
     */
    callToastMessage(text, subClass) {
      this.HideToastMessage = false;
      this.ToastMessageText = text;
      this.subClass = subClass;
      this.active = true;
    },

    /**
     * Hàm đóng toastmessage
     * Ngọc 2/8/2021
     */
    closeToastMessage() {
      this.HideToastMessage = true;
      this.ToastMessageText = "";
      this.subClass = "";
      this.active = false;
    },

    /**
     * Hàm chuyển đổi menu do <Menu/> gửi lên
     * Ngọc 3/8/2021
     */
    toggleMenu() {
      this.isToggle = !this.isToggle;
    },
  },

  created() {
    this.loadDataTable();
  },

  watch: {},
};
</script>

<style>
</style>
