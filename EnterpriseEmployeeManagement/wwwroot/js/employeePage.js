document.addEventListener("DOMContentLoaded", () => {

    CrudPage.init({

        pageContainer: "#employeePage",

        tableUrl: "/Employees/LoadEmployees",

        createUrl: "/Employees/CreateModal",
        editUrl: "/Employees/EditModal/{id}",
        detailsUrl: "/Employees/DetailsModal/{id}",
        deleteUrl: "/Employees/Delete/{id}",

        tableSelector: "#employeeTable",
        loaderSelector: "#employeeTableLoader",

        searchSelector: "#searchBox",
        pageSizeSelector: "#pageSize",

        stateKey: "employee_table_state",

        columns: ["name", "email", "department"],
        defaultSort: "name"
    });

});