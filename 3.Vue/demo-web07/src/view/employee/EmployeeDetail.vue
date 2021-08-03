<template>
  <div
    id="popup"
    :class="{ 'd-none': dnone }"
    :employeeId="employeeId"
    :formMode="formMode"
    Item="EmployeeCode"
  >
    <div class="head">
      <div class="head-text">THÔNG TIN NHÂN VIÊN</div>
      <div class="head-close" @click="btnDialogCancelonClick"></div>
    </div>
    <div class="main">
      <div class="avatar">
        <div class="image"></div>
        <div class="ta-center">(Vui lòng chọn ảnh có định</div>
        <div class="ta-center">dạng</div>
        <div class="ta-center">.jqg,.jepg,.png,.gif.)</div>
        <input class="d-none" type="file" name="" id="myFile" />
      </div>
      <div class="info">
        <div class="A">
          <div>A. THÔNG TIN CHUNG:</div>
          <div class="line"></div>
          <div class="row mt-8">
            <div class="col">
              <label for="">Mã nhân viên<span class="cl-red">(*)</span></label
              ><br />
              <input
                type="text"
                FieldName="EmployeeCode"
                class="textbox-default"
                ref="txtEmployeeCode"
                required
                v-model="employee.EmployeeCode"
              />
            </div>
            <div class="col">
              <label for="">Họ và tên<span class="cl-red">(*)</span></label
              ><br />
              <input
                type="text"
                FieldName="FullName"
                class="textbox-default"
                id="txtFullName"
                v-model="employee.FullName"
                required
              />
            </div>
          </div>
          <div class="row">
            <div class="col">
              <label for="">Ngày sinh</label><br />
              <input
                type="date"
                FieldName="DateOfBirth"
                DataType="Date"
                class="textbox-default"
                id="txtDateOfBirth"
                v-model="employee.DateOfBirth"
              />
            </div>
            <div class="col">
              <label for="">Giới tính</label><br />
              <Dropdown
                defaultName="Chưa chọn"
                itemId="Gender"
                itemName="GenderName"
                :selectedId="employee.Gender + ''"
                @chooseDropdownItem="chooseDropdownItem"
              />
            </div>
          </div>
          <div class="row">
            <div class="col">
              <label for=""
                >Số CMTND/ Căn cước<span class="cl-red">(*)</span></label
              ><br />
              <input
                type="text"
                FieldName="IdentityNumber"
                DataType="OnlyNumber"
                class="textbox-default"
                id="txtIdentityNumber"
                required
                v-model="employee.IdentityNumber"
              />
            </div>
            <div class="col">
              <label for="">Ngày cấp</label><br />
              <input
                type="date"
                FieldName="IdentityDate"
                DataType="Date"
                class="textbox-default"
                id="txtIdentityDate"
                v-model="employee.IdentityDate"
              />
            </div>
          </div>
          <div class="row">
            <div class="col">
              <label for="">Nơi cấp</label><br />
              <input
                type="text"
                FieldName="IdentityPlace"
                class="textbox-default"
                id="txtIdentityPlace"
                v-model="employee.IdentityPlace"
              />
            </div>
          </div>
          <div class="row">
            <div class="col">
              <label for="">Email<span class="cl-red">(*)</span></label
              ><br />
              <input
                type="text"
                FieldName="Email"
                class="textbox-default"
                id="txtEmail"
                required
                v-model="employee.Email"
              />
            </div>
            <div class="col">
              <label for="">Số điện thoại<span class="cl-red">(*)</span></label
              ><br />
              <input
                type="text"
                FieldName="PhoneNumber"
                DataType="OnlyNumber"
                class="textbox-default"
                id="txtPhoneNumber"
                required
                v-model="employee.PhoneNumber"
              />
            </div>
          </div>
        </div>
        <div class="B">
          <div>B.THÔNG TIN CÔNG VIỆC</div>
          <div class="line"></div>
          <div class="row mt-8">
            <div class="col">
              <label for="">Vị trí</label><br />
              <Dropdown
                :defaultName="employee.PositionName"
                dd_dropdown="dd-Position"
                Url="v1/Positions"
                itemId="PositionId"
                :selectedId="employee.PositionId + ''"
                itemName="PositionName"
                @chooseDropdownItem="chooseDropdownItem"
              />
            </div>
            <div class="col">
              <label for="">Phòng ban</label><br />
              <Dropdown
                :defaultName="employee.DepartmentName"
                dd_dropdown="dd-Department"
                Url="api/Department"
                :selectedId="employee.DepartmentId + ''"
                itemId="DepartmentId"
                itemName="DepartmentName"
                @chooseDropdownItem="chooseDropdownItem"
              />
            </div>
          </div>
          <div class="row">
            <div class="col">
              <label for="">Mã số thuế cá nhân</label><br />
              <input
                type="text"
                FieldName="PersonalTaxCode"
                DataType="OnlyNumber"
                class="textbox-default"
                id="txtPersonalTaxCode"
                v-model="employee.PersonalTaxCode"
              />
            </div>
            <div class="col">
              <label for="">Mức lương cơ bản</label><br />
              <input
                type="text"
                FieldName="Salary"
                DataType="Number"
                ref="txtSalary"
                class="textbox-default ta-r pd-19"
                maxlength="15"
                v-model="employee.Salary"
                @input="convertMoney"
              />
              <div class="currency">(VNĐ)</div>
            </div>
          </div>
          <div class="row">
            <div class="col">
              <label for="">Ngày gia nhập công ty</label><br />
              <input
                type="date"
                FieldName="JoinDate"
                DataType="Date"
                class="textbox-default"
                id="txtJoinDate"
                v-model="employee.JoinDate"
              />
            </div>
            <div class="col">
              <label for="">Tình trạng công việc</label><br />
              <Dropdown
                defaultName="Chưa chọn"
                itemId="WorkStatus"
                itemName="WorkStatusName"
                :selectedId="employee.WorkStatus + ''"
                @chooseDropdownItem="chooseDropdownItem"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="foot">
      <Button
        @btn-click="btnSaveonClick"
        buttonText="Lưu"
        id="btnSave"
        subClass="save d-flex"
      />

      <Button
        @btn-click="btnDialogCancelonClick"
        buttonText="Hủy"
        id="btnCancel"
        subClass="cancel d-flex"
      />
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { CommonFn } from "../../js/mixins.js";

import Button from "../../components/base/BaseButton.vue";
import Dropdown from "../../components/base/BaseDropdown.vue";
export default {
  mixins: [CommonFn],
  name: "EmployeeDetail",
  components: {
    Dropdown,
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
    formMode: Number,
    reopen: Boolean,
  },

  data() {
    return {
      employee: {},
      defaultName: "",
      selectedId: "",
    };
  },

  watch: {
    reopen: function () {
      let me = this;
      //Gọi Api lấy thông tin chi tiết:
      if (me.formMode == 0) {
        this.employee = {};
        this.getNewCode();
      } else {
        axios
          .get(`http://cukcuk.manhnv.net/v1/Employees/${this.employeeId}`)
          .then((res) => {
            me.employee = res.data;
            me.employee.Salary = me.formatMoney(res.data.Salary);
            me.employee.DateOfBirth = me.convertDate(res.data.DateOfBirth);
            me.employee.JoinDate = me.convertDate(res.data.JoinDate);
            me.employee.IdentityDate = me.convertDate(res.data.IdentityDate);
          })
          .catch((err) => {
            console.log(err);
          });
      }
    },
    formMode: function () {
      if (this.formMode == 0) {
        this.employee = {};
        this.getNewCode();
      }
    },
  },

  methods: {
    /**
     * Hàm đóng popup
     * Ngọc 29/07/2021
     */
    btnDialogCancelonClick() {
      let me = this;
      me.$emit("btnDialogCancelonClick");
    },

    /**
     * Sự kiện bấm nút lưu
     * Ngọc 29/07/2021
     */
    btnSaveonClick() {
      let me = this;
      me.employee.Salary = me.formatNumber(me.employee.Salary);
      if (this.formMode == 0) {
        axios
          .post(`http://cukcuk.manhnv.net/v1/Employees/`, this.employee)
          .then(() => {
            me.$emit(
              "callToastMessage",
              "Thêm dữ liệu thành công",
              "message-green"
            );
            me.$emit("btnSaveonClick");
          })
          .catch((err) => {
            console.log(err);
          });
      } else {
        axios
          .put(
            `http://cukcuk.manhnv.net/v1/Employees/${this.employeeId}`,
            this.employee
          )
          .then(() => {
            me.$emit(
              "callToastMessage",
              "Sửa dữ liệu thành công",
              "message-green"
            );
            me.$emit("btnSaveonClick");
          })
          .catch((err) => {
            console.log(err);
          });
      }
    },

    /**
     * Lấy mã nhân viên mới
     * Ngọc 1/8/2021
     */
    getNewCode() {
      let me = this;
      axios
        .get(`http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode`)
        .then((res) => {
          let newEmployee = {};
          newEmployee.EmployeeCode = res.data;
          me.employee = newEmployee;
          me.$refs.txtEmployeeCode.focus();
        })
        .catch((err) => {
          console.log(err);
        });
    },

    /**
     * Hàm tự động format tiền lương được nhập
     * Ngọc 2/8/2021
     */
    convertMoney() {
      let me = this;
      let val = me.employee.Salary;
      let val1 = me.formatNumber(val);
      me.employee.Salary = me.formatMoney(val1);
    },
    /**
     * Hàm lấy id của dropdown được chọn gán vào obj employee
     * Ngọc 31/07/2021
     */
    chooseDropdownItem(itemValue, itemID) {
      this.employee[itemID] = itemValue;
    },

    /**
     * Hàm format số tiền
     * Ngọc 18/7/2021
     */
    formatMoney(money) {
      if (money && !isNaN(money)) {
        return money.toString().replace(/(\d)(?=(\d{3})+(?:\.\d+)?$)/g, "$1.");
      } else {
        return money;
      }
    },

    /**
     * Chuyển đổi ngày tháng
     * Ngọc 18/7/2021
     */
    convertDate(dateSrc) {
      let dateString = "";
      if (dateSrc) {
        let date = new Date(dateSrc),
          year = date.getFullYear().toString(),
          month = (date.getMonth() + 1).toString().padStart(2, "0"),
          day = date.getDate().toString().padStart(2, "0");

        dateString = `${year}-${month}-${day}`;
      } else {
        dateString = "123";
      }
      return dateString;
    },

    /**
     * Hàm format số tiền về số bth
     * Ngọc 18/7/2021
     */
    formatNumber(money) {
      let salary = money.replaceAll(".", "");
      return salary;
    },
  },

  created() {
    // this.formMode = 1;
  },
};
</script>

<style scoped>
</style>