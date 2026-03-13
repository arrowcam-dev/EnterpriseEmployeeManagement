document.addEventListener("DOMContentLoaded", () => {

    CrudPage.init({

        pageContainer: "#usersPage",

        tableUrl: "/Users/LoadUsers",

        createUrl: "/Users/CreateModal",
        editUrl: "/Users/EditModal/{id}",
        detailsUrl: "/Users/DetailsModal/{id}",
        deleteUrl: "/Users/Delete/{id}",

        tableSelector: "#usersTable",
        loaderSelector: "#usersTableLoader",

        searchSelector: "#searchBox",
        pageSizeSelector: "#pageSize",

        stateKey: "users_table_state",

        columns: ["usernamet"],
        defaultSort: "username",

        entityName: "User"
    });

});