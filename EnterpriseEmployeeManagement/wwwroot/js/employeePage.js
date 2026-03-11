let employeeTable;

document.addEventListener("DOMContentLoaded", () => {

    employeeTable = AjaxTable({
        url: "/Employee/LoadEmployees",
        table: "#employeeTable",
        loader: "#employeeTableLoader",
        searchBox: "#searchBox",
        pageSize: "#pageSize",
        stateKey: "employee_table_state",
        columns: ["name", "email", "department"],
        defaultSort: "name"
    });

    ModalForms.bindActions("#employeeCard", {

        createUrl: "/Employee/CreateModal",
        editUrl: "/Employee/EditModal/{id}",
        deleteUrl: "/Employee/Delete/{id}",

        modalId: "#employeeModal",
        bodyId: "employeeModalBody",

        deleteModalId: "#deleteModal",
        deleteConfirmBtn: "#confirmDeleteBtn",

        afterDelete: () => employeeTable.load()
    });


    employeeTable.restore();
});