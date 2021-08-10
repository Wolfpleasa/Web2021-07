<template>
  <div>
    <div :class="{ 'o-40': opacity }">
      <Header />
      <Menu :isToggle="isToggle" @toggleMenu="toggleMenu" />
      <div :class="['content', { narrow: isToggle }, { expand: !isToggle }]">
        <div class="content-header">
          <div class="title">Danh sách nhân viên</div>
          <div class="d-flex">
            <ButtonIcon
              @btn-click="btnDuplicateOnClick"
              iconName="icon-add"
              buttonText="Nhân bản nhân viên"
              id="btnDuplicate"
              :class="{ 'v-hidden': HideBtnDuplicate }"
            />
            <ButtonIcon
              @btn-click="btnDeleteOnClick"
              iconName="icon-delete"
              buttonText="Xóa nhân viên"
              id="btnDelete"
              :class="{ 'v-hidden': HideBtnDelete }"
            />
            <ButtonIcon
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
        <PageNavigation :totalEntity = "totalEntity"/>
      </div>
    </div>
    <EmployeeDetail
      v-bind:dnone="DialogHasDnone"
      :employeeId="employeeId"
      :formMode="formMode"
      @btnDialogCancelOnClick="btnDialogCancelOnClick"
      @btnSaveOnClick="btnSaveOnClick"
      @resetAfterSaveData="resetAfterSaveData"
      @callToastMessage="callToastMessage"
      :reopen="reopen"
      :response="response"
      :blurr="blurr"
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
      :subClass="bonusClass"
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
import ButtonIcon from "../../components/base/BaseButtonIcon.vue";
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
    ButtonIcon,
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
      //làm mờ trang
      opacity: false,
      //checkbox,tr
      isSelected: [],
      checked: false,
      //nút xóa
      HideBtnDelete: true,
      //nút nhân bản
      HideBtnDuplicate: true,
      //ToastMessage
      HideToastMessage: true,
      ToastMessageText: "",
      //EmployeeDetail
      DialogHasDnone: true,
      employeeId: null,
      reopen: true,
      formMode: -1,
      subClass: "",
      blurr: false,
      // form cảnh báo/thông báo
      WarningHasDnone: true,
      warning: "",
      warningText: "",
      idPopup: "",
      btnCancelText: "",
      btnConfirmText: "",
      notifyMode: -1,
      response: "",
      bonusClass: "",
      //menu
      isToggle: true,
      //silde
      active: false,
      //tổng số nhân viên
      totalEntity: ""
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
      me.opacity = true;
      me.blurr = false;
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
      me.HideBtnDuplicate = false;
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
        this.HideBtnDuplicate = false;
      } else {
        this.isSelected.fill(false);
        this.HideBtnDelete = true;
        this.HideBtnDuplicate = true;
      }
    },

    /**
     * Hàm chọn 1 hàng
     * Ngọc 29/07/2021
     */
    trOnClick(index) {
      this.$set(this.isSelected, index, !this.isSelected[index]);
      this.HideBtnDelete = false;
      this.HideBtnDuplicate = false;
      this.CheckRowIsSelected();
    },

    /**
     * Hàm kiểm tra các hàng vẫn đang được chọn để hiện nút xóa
     * Ngọc 1/8/2021
     */
    CheckRowIsSelected() {
      let me = this;
      me.HideBtnDelete = true;
      me.HideBtnDuplicate = true;
      me.isSelected.forEach(function (item) {
        if (item) {
          me.HideBtnDelete = false;
          me.HideBtnDuplicate = false;
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
      me.opacity = true;
      me.WarningHasDnone = false;
      me.idPopup = "warning-popup";
      me.btnCancelText = "Hủy";
      me.btnConfirmText = "Xóa";
    },

    /**
     * Hàm bấm nút nhân bản
     * Ngọc 30/07/2021
     */
    btnDuplicateOnClick() {
      let me = this;
      var count = 0;
      this.isSelected.forEach((selected) => {
        if (selected) {
          count = count + 1;
        }
      });
      if (count > 1) {
        me.warning = `Nhân bản thông tin của ${count} nhân viên`;
        me.warningText = `Xác nhận nhân bản thông tin của <b>${count} nhân viên </b> này không`;
      } else {
        let fullname = "";
        this.isSelected.forEach((selected, index) => {
          if (selected) {
            fullname = me.employees[index].FullName;
          }
        });
        me.warning = `Nhân bản thông tin của nhân  viên ${fullname}`;
        me.warningText = `Xác nhận nhân bản thông tin của nhân viên <b>${fullname} </b>`;
      }
      me.opacity = true;
      me.WarningHasDnone = false;
      me.idPopup = "notify-popup";
      me.btnCancelText = "Hủy";
      me.bonusClass = "w-100";
      me.btnConfirmText = "Lưu";
      me.notifyMode = 2;
    },

    /**
     * Hàm bấm nút xác nhận
     * Ngọc 30/07/2021
     */
    btnConfirmOnClick(idPopup) {
      let me = this;
      // Nếu form cảnh báo được gọi
      me.WarningHasDnone = true;
      if (idPopup == "warning-popup") {
        me.isSelected.forEach((selected, index) => {
          if (selected) {
            me.employeeId = me.employees[index]["EmployeeId"];
            axios
              .delete(`http://cukcuk.manhnv.net/v1/Employees/${me.employeeId}`)
              .then(() => {
                me.callToastMessage("Xóa dữ liệu thành công", "message-green");
                // reset các hàng thành không được chọn
                me.isSelected = [];
                // Tải lại bảng
                me.loadDataTable();
              })
              .catch((err) => {
                console.log(err);
              });
          }
        });
        me.HideBtnDelete = true;
        me.HideBtnDuplicate = true;
        me.opacity = false;
      }
      //Form thông báo được gọi
      else {
        //Nút lưu của from detail được bấm
        if (me.notifyMode == 0) {
          let responseTime = Number(new Date());
          me.response = `Lưu__${responseTime}`;
        }
        //Nút Hủy của from detail được bấm
        else if (me.notifyMode == 1) {
          me.WarningHasDnone = true;
          me.DialogHasDnone = true;
          me.opacity = false;
          me.blurr = false;
          me.formMode = -1;
        }
        // Nút nhân bản được bấm
        else {
          // Đóng form cảnh báo/thông báp
          me.WarningHasDnone = true;
          (async () => {
            for (var i = 0; i < me.isSelected.length; i++) {
              if (me.isSelected[i]) {
                // lấy id của nhân viên được chọn
                let employeeId = me.employees[i]["EmployeeId"];
                // lấy thông tin của nhân viên được chọn
                let employeeDuplicated = await me.getInfo(employeeId);
                // lấy mã nhân viên mới
                let newCode = await me.getNewCode();
                console.log(
                  newCode,
                  " | ",
                  employeeDuplicated,
                  Number(new Date())
                );
                // gán mã nhân viên vừa lấy dc vào mã nhân viên của nhân viên được chọn để tạo ra 1 nhân viên nhân bản
                employeeDuplicated["EmployeeCode"] = newCode;
                // gửi nhân viên nhân bản lên để thêm vào db
                let ok = await me.InsertDuplicate(employeeDuplicated);
                console.log("Lần: ", ok, Number(new Date()));
              }
            }
            // gọi toast-message
            me.callToastMessage("Nhân bản dữ liệu thành công", "message-green");
            // reset các hàng thành không được chọn
            me.isSelected = [];
            // Tải lại bảng
            me.loadDataTable();
            // Ẩn nút nhân bản và xóa đi
            me.HideBtnDelete = true;
            me.HideBtnDuplicate = true;
            me.opacity = false;
          })();
        }
      }
      me.idPopup = "";
    },

    /**
     * Hàm lấy mã nhân viên mới
     * Ngọc 6/8/2021
     */
    getNewCode() {
      return new Promise((resolve) => {
        axios
          .get(`http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode`)
          .then((res) => {
            console.log(res.data, Number(new Date()));
            resolve(res.data);
          })
          .catch((err) => {
            console.log(err);
          });
      });
    },

    /**
     * Hàm lấy nhân viên theo id nhân viên
     * Ngọc 6/8/2021
     */
    getInfo(employeeId) {
      return new Promise((resolve) => {
        axios
          .get(`http://cukcuk.manhnv.net/v1/Employees/${employeeId}`)
          .then((res) => {
            let newEmployee = res.data;
            console.log(newEmployee.FullName, Number(new Date()));
            resolve(newEmployee);
          })
          .catch((err) => {
            console.log(err);
          });
      });
    },

    /**
     * Thêm nhân viên được nhân bản
     * Ngọc 6/8/2021
     */
    InsertDuplicate(employeeDuplicated) {
      return new Promise((resolve) => {
        axios
          .post(`http://cukcuk.manhnv.net/v1/Employees/`, employeeDuplicated)
          .then(() => {
            resolve(1);
          })
          .catch((err) => {
            console.log(err);
          });
      });
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
          //me.employees = res.data.slice(0, 100);
          me.employees = res.data;
          me.totalEntity = me.employees.length + "";
          // reset các tr về không được chọn
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
      if(me.formMode == -1){
        me.opacity = false;
      }else{
        me.blurr = false;
      }
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
        //nếu form thêm gọi
        custom = "nhập";
      } else {
        //nếu form sửa gọi
        custom = "sửa";
      }
      me.warningText = `Bạn có chắc muốn đóng thông tin form ${custom} <b>"Thông tin nhân viên"</b> hay không`;
      me.btnCancelText = "Tiếp tục nhập";
      me.btnConfirmText = "Đóng";
      me.bonusClass = "w-auto";
      me.notifyMode = 1;
      me.blurr = true;
    },

    /**
     * Hàm thực hiện những reset sau khi lưu
     * Ngọc 5/8/2021
     */
    resetAfterSaveData() {
      let me = this;
      //reset formMode
      me.formMode = -1;
      //Đống form thêm/sửa
      me.DialogHasDnone = true;
      //Đóng popup
      me.WarningHasDnone = true;
      // reset mảng đánh dấu
      me.isSelected = [];
      //tải lại dữ liệu cho table
      me.loadDataTable();
      //làm rõ page
      me.opacity = false;
    },

    /**
     * Hàm bấm lưu được EmployeeDetail gửi lên
     * Ngọc 30/07/2021
     */
    btnSaveOnClick(employeeFullName) {
      let me = this,
        custom = "";
      if (this.formMode == 0) {
        custom = "thêm";
      } else {
        custom = "sửa";
      }
      //Hiển thị popup thông báo lưu
      me.WarningHasDnone = false;
      me.idPopup = "notify-popup";
      me.warning = "Lưu thông tin nhân viên ";
      me.warningText = `Xác nhận ${custom} nhân viên <b>${employeeFullName}</b> `;
      me.btnCancelText = "Hủy";
      me.btnConfirmText = "Lưu";
      me.bonusClass = "w-100";
      me.notifyMode = 0;
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
