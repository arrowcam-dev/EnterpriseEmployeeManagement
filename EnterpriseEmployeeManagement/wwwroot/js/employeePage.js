document.addEventListener("DOMContentLoaded", () => {

    CrudPage.init({

        pageContainer: "#employeeCard",

        tableUrl: "/Employee/LoadEmployees",

        createUrl: "/Employee/CreateModal",
        editUrl: "/Employee/EditModal/{id}",
        deleteUrl: "/Employee/Delete/{id}",

        tableSelector: "#employeeTable",
        loaderSelector: "#employeeTableLoader",

        searchSelector: "#searchBox",
        pageSizeSelector: "#pageSize",

        stateKey: "employee_table_state",

        columns: ["name", "email", "department"],
        defaultSort: "name",

        modalId: "#employeeModal",
        modalBodyId: "employeeModalBody",

        deleteModalId: "#deleteModal",
        deleteConfirmBtn: "#confirmDeleteBtn"
    });

});