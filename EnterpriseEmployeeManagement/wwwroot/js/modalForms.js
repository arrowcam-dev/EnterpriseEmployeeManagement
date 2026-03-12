const ModalForms = (function () {

    function open(url, title) {

        Api.get(url, { loader: false })
            .then(html => {

                const body = document.getElementById("crudModalBody");
                const titleElement = document.getElementById("crudModalTitle");

                body.innerHTML = html;

                // Default title
                if (title) titleElement.innerText = title;

                // If the partial view provides a dynamic title
                const dynamicTitle = body.querySelector("[data-modal-title]");

                if (dynamicTitle)
                    titleElement.innerText = dynamicTitle.dataset.modalTitle;

                const form = body.querySelector("form");

                if (form) UI.enableFormValidation(form);

                new bootstrap.Modal("#crudModal").show();
            });
    }

    function confirm(options) {

        const modal = new bootstrap.Modal("#confirmModal");

        document.getElementById("confirmModalTitle").innerText = options.title || "Confirm";

        document.getElementById("confirmModalMessage").innerHTML = options.message;

        let btn = document.getElementById("confirmModalBtn");

        const newBtn = btn.cloneNode(true);

        btn.replaceWith(newBtn);

        btn = newBtn;

        btn.className = options.btnClass || "btn btn-danger";

        btn.innerText = options.confirmText || "Confirm";

        btn.addEventListener("click", function () {

            modal.hide();

            if (options.onConfirm) options.onConfirm();
        });

        modal.show();
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

    return { open, confirm };

})();

document.addEventListener("submit", function (e) {

    const form = e.target;

    if (!form.matches("[data-ajax-form]")) return;

    e.preventDefault();

    const submitBtn = form.querySelector("button[type=submit]");
    const url = form.action;

    UI.disableButton(submitBtn);

    Api.post(url, new FormData(form))
        .then(() => {
            const modalElement = form.closest(".modal");

            bootstrap.Modal
                .getInstance(modalElement)
                .hide();

            UI.toast("Saved successfully");

            CrudPage.reload();

        }).catch(errors => {
            UI.displayValidationErrors(errors);
        }).finally(() => {
            UI.enableButton(submitBtn);
        });
});