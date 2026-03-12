document.addEventListener("DOMContentLoaded", () => {

    CrudPage.init({

        pageContainer: "#employeeCard",

        tableUrl: "/Employee/LoadEmployees",

        createUrl: "/Employee/CreateModal",
        editUrl: "/Employee/EditModal/{id}",
        detailsUrl: "/Employee/DetailsModal/{id}",
        deleteUrl: "/Employee/Delete/{id}",

        tableSelector: "#employeeTable",
        loaderSelector: "#employeeTableLoader",

        searchSelector: "#searchBox",
        pageSizeSelector: "#pageSize",

        stateKey: "employee_table_state",

        columns: ["name", "email", "department"],
        defaultSort: "name"
    });

});