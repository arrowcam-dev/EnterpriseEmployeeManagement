const ModalForms = (function () {

    function openModal(url, modalId, bodyId) {

        Api.get(url, { loader: false })
            .then(html => {
                const body = document.getElementById(bodyId);

                body.innerHTML = html;

                const form = body.querySelector("form");

                UI.enableFormValidation(form);

                new bootstrap.Modal(modalId).show();
            });
    }

    function confirmDelete(id, deleteUrl, afterDelete) {

        const modalElement = document.getElementById("deleteModal");

        const modal = new bootstrap.Modal(modalElement);

        modal.show();

        let confirmBtn = document.getElementById("confirmDeleteBtn");

        const newBtn = confirmBtn.cloneNode(true);

        confirmBtn.replaceWith(newBtn);

        confirmBtn = newBtn;

        confirmBtn.addEventListener("click", function () {

            Api.delete(deleteUrl.replace("{id}", id))
                .then(() => {

                    modal.hide();

                    UI.toast("Deleted successfully");

                    if (afterDelete) afterDelete();
                })
                .catch(() => {

                    UI.toast("Delete failed", "danger");
                });
        });
    }

    return { openModal, confirmDelete };

})();

document.addEventListener("submit", function (e) {

    const form = e.target;

    if (!form.matches("[data-ajax-form]")) return;

    e.preventDefault();

    const submitBtn = form.querySelector("button[type=submit]");
    const url = form.action;

    UI.disableButton(submitBtn);
    UI.showLoader();

    Api.post(url, new FormData(form))
        .then(() => {
            bootstrap.Modal
                .getInstance(document.getElementById('employeeModal'))
                .hide();
            UI.toast("Saved successfully");

            CrudPage.reload();

        }).catch(errors => {
            UI.displayValidationErrors(errors);
        });
});