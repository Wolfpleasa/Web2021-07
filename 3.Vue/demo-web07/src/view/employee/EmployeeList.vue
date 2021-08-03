<template>
  <div>
    <Header />
    <Menu />
    <div class="content">
      <div class="content-header">
        <div class="title">Danh sách nhân viên</div>
        <div class="d-flex">
          <Button
            @btn-click="btnDeleteonClick"
            iconName="icon-delete"
            buttonText="Xóa nhân viên"
            id="btnDelete"
            :class="{ 'v-hidden': HideBtnDelete }"
          />
          <Button
            @btn-click="btnAddonClick"
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
    </div>
    <EmployeeDetail
      v-bind:dnone="DialogHasDnone"
      :employeeId="employeeId"
      :formMode="formMode"
      @btnDialogCancelonClick="btnDialogCancelonClick"
      @btnSaveonClick="btnSaveonClick"
      @callToastMessage="callToastMessage"
      :reopen="reopen"
    />

    <WarningPopup
      :idPopup="idPopup"
      v-bind:dnone="WarningHasDnone"
      :employeeId="employeeId"
      @btnCancelonClick="btnCancelonClick"
      :warning="warning"
      :warningText="warningText"
      :btnCancelText="btnCancelText"
      :btnConfirmText="btnConfirmText"
      @btnConfirmonClick="btnConfirmonClick"
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

import EmployeeDetail from "./EmployeeDetail.vue";
import WarningPopup from "./WarningPopup.vue";
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
    EmployeeDetail,
    WarningPopup,
  },
  data() {
    return {
      employees: [],
      DialogHasDnone: true,
      employeeId: null,
      subClass: "",
      formMode: -1,
      isSelected: [],
      WarningHasDnone: true,
      warning: "",
      warningText: "",
      reopen: true,
      HideBtnDelete: true,
      HideToastMessage: true,
      ToastMessageText: "",
      checked: false,
      active: false,
      idPopup: "",
      btnCancelText: "",
      btnConfirmText: "",
    };
  },

  methods: {
    /**
     * Hàm mở popup
     * Ngọc 29/07/2021
     */
    btnAddonClick() {
      let me = this;
      me.DialogHasDnone = false;
      this.formMode = 0;
    },

    /**
     * Hàm đóng popup
     * Ngọc 29/07/2021
     */
    btnCancelonClick() {
      let me = this;
      me.WarningHasDnone = true;
    },

    /**

     */
    btnDialogCancelonClick() {
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
      me.warningText = `Bạn có chắc muốn đóng thông tin form ${custom} thông tin nhân viên hay không`;
      me.btnCancelText = "Tiếp tục nhập";
      me.btnConfirmText = "Đóng";
    },

    /**
     * Hàm mở popup để sửa
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
      this.$set(this.isSelected, index, !this.isSelected[index]);
      this.HideBtnDelete = false;
      this.checked = this.CheckAllCBTd();
    },

    /**
     * Hàm kiểm tra tất cả checkbox của td có được chọn không
     * Ngọc 1/8/2021
     */
    CheckAllCBTd() {
      for (var i = 0; i < this.isSelected.length; i++) {
        if (!this.isSelected[i]) {
          return false;
        }
      }
      return true;
    },

    /**
     * Hàm bấm checkbox ở th được BaseCheckBox gửi lên
     * Ngọc 1/8/2021
     */
    clickCheckboxTh() {
      this.HideBtnDelete = false;
      this.checked = !this.checked;
      if (this.checked) {
        for (var i = 0; i < this.isSelected.length; i++) {
          this.isSelected[i] = true;
        }
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
    btnDeleteonClick() {
      let me = this;
      var count = 0;
      this.isSelected.forEach((selected) => {
        if (selected) {
          count = count + 1;
        }
      });
      if (count > 1) {
        me.warning = `Xóa thông tin của ${count} nhân viên`;
        me.warningText = `Xác nhận xóa thông tin của ${count} nhân viên này không`;
      } else {
        let fullname = "";
        this.isSelected.forEach((selected, index) => {
          if (selected) {
            fullname = me.employees[index].FullName;
          }
        });
        me.warning = `Xóa thông tin của nhân  viên ${fullname}`;
        me.warningText = `Xác nhận xóa thông tin của nhân viên ${fullname} `;
      }
      me.WarningHasDnone = false;
      me.idPopup = "warning-popup";
      me.btnCancelText = "Hủy";
      me.btnConfirmText = "Xóa";
    },

    /**
     * Hàm bấm nút xác nhận xóa
     * Ngọc 30/07/2021
     */
    btnConfirmonClick(idPopup) {
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
                //me.resetTr();
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
          me.employees = res.data;
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
     * Hàm bấm lưu xong thì đóng popup được EmployeeDetail gửi lên
     * Ngọc 30/07/2021
     */
    btnSaveonClick() {
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
  },

  created() {
    this.loadDataTable();
  },

  watch: {},
};
</script>

<style>
</style>
