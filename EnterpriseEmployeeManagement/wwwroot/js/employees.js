let currentSortColumn = "name";
let currentSortDirection = "asc";

function sortTable(column) {
    if (currentSortColumn === column) {
        currentSortDirection = currentSortDirection === "asc" ? "desc" : "asc";
    }
    else {
        currentSortColumn = column;
        currentSortDirection = "asc";
    }

    updateSortIcons();

    loadEmployees(1);
}


function loadEmployees(page = 1) {
    const search = document.getElementById("searchBox").value;
    const pageSize = document.getElementById("pageSize").value;

    fetch(`/Employee/LoadEmployees?search=${search}&page=${page}&pageSize=${pageSize}&sortColumn=${currentSortColumn}&sortDirection=${currentSortDirection}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById("employeeTable").innerHTML = html;

            updateSortIcons();
        });
}

function updateSortIcons() {
    const columns = ["name", "email", "department"];

    columns.forEach(col => {
        const icon = document.getElementById("sortIcon-" + col);

        if (!icon) return;

        icon.className = "";
    });

    const currentIcon =
        document.getElementById("sortIcon-" + currentSortColumn);

    if (currentIcon) {
        if (currentSortDirection === "asc")
            currentIcon.className = "bi bi-arrow-up";
        else
            currentIcon.className = "bi bi-arrow-down";
    }
}


document.getElementById("searchBox").addEventListener("keyup", () => {
    loadEmployees(1);
});

document.getElementById("pageSize").addEventListener("change", () => {
    loadEmployees(1);
});

document.addEventListener("DOMContentLoaded", () => {
    loadEmployees();
});

