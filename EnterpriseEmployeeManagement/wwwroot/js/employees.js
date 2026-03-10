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
    showLoader();

    const search = document.getElementById("searchBox").value;
    const pageSize = document.getElementById("pageSize").value;

    fetch(`/Employee/LoadEmployees?search=${search}&page=${page}&pageSize=${pageSize}&sortColumn=${currentSortColumn}&sortDirection=${currentSortDirection}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById("employeeTable").innerHTML = html;

            updateSortIcons();
        })
        .finally(() => {
            hideLoader();
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
            const modalBody = document.getElementById('employeeModalBody');

            modalBody.innerHTML = html;

            // Re-enable validation
            const form = modalBody.querySelector("form");

            enableFormValidation(form);

            new bootstrap.Modal('#employeeModal').show();
        });
}

function openEditModal(id) {
    fetch(`/Employee/EditModal/${id}`)
        .then(res => res.text())
        .then(html => {
            const modalBody = document.getElementById('employeeModalBody');

            modalBody.innerHTML = html;

            // Re-enable validation
            const form = modalBody.querySelector("form");

            enableFormValidation(form);
            new bootstrap.Modal('#employeeModal').show();
        });
}

function confirmDelete(id) {
    deleteEmployeeId = id;

    new bootstrap.Modal('#deleteModal').show();
}

document.getElementById('confirmDeleteBtn').addEventListener('click', () => {
    fetch(`/Employee/Delete/${deleteEmployeeId}`)
        .then(() => {
            loadEmployees();

            bootstrap.Modal.getInstance(
                document.getElementById('deleteModal')
            ).hide();
        });
});

document.addEventListener("submit", function (e) {
    if (e.target.id === "employeeForm" ||
        e.target.id === "editEmployeeForm") {
        e.preventDefault();

        const form = e.target;
        const submitBtn = form.querySelector("button[type=submit]");
        const formData = new FormData(form);

        const url = form.id === "employeeForm"
            ? "/Employee/CreateModal"
            : "/Employee/EditModal";

        disableButton(submitBtn);
        showLoader();

        fetch(url, {
            method: "POST",
            body: formData
        })
            .then(async response => {
                //console.log(response);

                if (response.ok) {
                    loadEmployees();

                    bootstrap.Modal
                        .getInstance(document.getElementById('employeeModal'))
                        .hide();

                    showToast("Saved successfully");
                }
                else {
                    //var errText = await response.text();
                    //console.log(errText);

                    const errors = await response.json();
                    console.log(errors);

                    displayValidationErrors(errors);
                }
            })
            .catch(() => {
                showToast("Unexpected server error", "danger");
            })
            .finally(() => {
                enableButton(submitBtn);
                hideLoader();
            });
    }
});

function displayValidationErrors(errors) {
    document
        .querySelectorAll("[data-valmsg-for]")
        .forEach(x => x.innerText = "");

    for (const key in errors) {
        const field = document.querySelector(
            `[data-valmsg-for="${key}"]`
        );

        if (field) {
            field.innerText = errors[key][0];
        }
    }
}

function enableFormValidation(form) {
    $(form).removeData("validator");
    $(form).removeData("unobtrusiveValidation");

    $.validator.unobtrusive.parse(form);
}


