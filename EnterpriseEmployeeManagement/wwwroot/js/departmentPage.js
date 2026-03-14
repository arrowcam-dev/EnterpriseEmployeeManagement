document.addEventListener("DOMContentLoaded", () => {

    CrudPage.init({

        pageContainer: "#departmentPage",

        tableUrl: "/Departments/LoadDepartments",

        createUrl: "/Departments/CreateModal",
        editUrl: "/Departments/EditModal/{id}",
        detailsUrl: "/Departments/DetailsModal/{id}",
        deleteUrl: "/Departments/Delete/{id}",

        tableSelector: "#departmentTable",
        loaderSelector: "#departmentTableLoader",

        searchSelector: "#searchBox",
        pageSizeSelector: "#pageSize",

        stateKey: "department_table_state",

        columns: ["name", "description"],
        defaultSort: "name"
    });

});