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

let deleteEmployeeId = 0;

function openCreateModal() {
    fetch('/Employee/CreateModal')
        .then(res => res.text())
        .then(html => {
            document.getElementById('employeeModalBody').innerHTML = html;

            new bootstrap.Modal('#employeeModal').show();
        });
}

function openEditModal(id) {
    fetch(`/Employee/EditModal/${id}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById('employeeModalBody').innerHTML = html;

            new bootstrap.Modal('#employeeModal').show();
        });
}

function confirmDelete(id) {
    deleteEmployeeId = id;

    new bootstrap.Modal('#deleteModal').show();
}

document.getElementById('confirmDeleteBtn')
    .addEventListener('click', () => {
        fetch(`/Employee/Delete/${deleteEmployeeId}`)
            .then(() => {
                loadEmployees();

                bootstrap.Modal.getInstance(
                    document.getElementById('deleteModal')
                ).hide();
            });
    });

document.addEventListener("submit", function (e) {
    if (e.target.id === "employeeForm") {
        e.preventDefault();

        const formData = new FormData(e.target);

        fetch('/Employee/CreateModal',
            {
                method: 'POST',
                body: formData
            })
            .then(() => {
                loadEmployees();

                bootstrap.Modal.getInstance(
                    document.getElementById('employeeModal')
                ).hide();
            });
    } else if (e.target.id === "editEmployeeForm") {
        e.preventDefault();

        const formData = new FormData(e.target);

        fetch('/Employee/EditModal',
            {
                method: 'POST',
                body: formData
            })
            .then(res => {
                if (res.ok) {
                    loadEmployees();

                    bootstrap.Modal.getInstance(
                        document.getElementById('employeeModal')
                    ).hide();
                }
            });
    }
});


