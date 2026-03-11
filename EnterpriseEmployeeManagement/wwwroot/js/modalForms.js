const ModalForms = (function () {

    function openModal(url, modalId, bodyId) {

        Api.get(url)
            .then(html => {
                const body = document.getElementById(bodyId);
                body.innerHTML = html;
                const form = body.querySelector("form");
                UI.enableFormValidation(form);
                new bootstrap.Modal(modalId).show();
            });

        //fetch(url)
        //    .then(res => res.text())
        //    .then(html => {

        //        const body = document.getElementById(bodyId);

        //        body.innerHTML = html;

        //        const form = body.querySelector("form");

        //        UI.enableFormValidation(form);

        //        new bootstrap.Modal(modalId).show();
        //    });
    }

    function bindActions(containerSelector, config) {

        document.querySelector(containerSelector)
            .addEventListener("click", function (e) {

                const createBtn = e.target.closest("[data-create]");
                const editBtn = e.target.closest("[data-edit]");
                const deleteBtn = e.target.closest("[data-delete]");

                if (createBtn) {

                    openModal(
                        config.createUrl,
                        config.modalId,
                        config.bodyId
                    );

                    return;
                }

                if (editBtn) {

                    const id = editBtn.dataset.edit;

                    openModal(
                        config.editUrl.replace("{id}", id),
                        config.modalId,
                        config.bodyId
                    );

                    return;
                }

                if (deleteBtn) {

                    const id = deleteBtn.dataset.delete;

                    confirmDelete(id, config);
                }
            });
    }

    function confirmDelete(id, config) {

        const modal = new bootstrap.Modal(config.deleteModalId);

        modal.show();

        const confirmBtn = document.querySelector(config.deleteConfirmBtn);

        confirmBtn.onclick = function () {

            Api.delete(config.deleteUrl.replace("{id}", id))
                .then(() => {
                    modal.hide();

                    if (config.afterDelete) {
                        config.afterDelete();
                    }

                    UI.toast("Deleted successfully");
                });
        };
    }

    return { openModal, bindActions };

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

            employeeTable.load();

        }).catch(errors => {
            displayValidationErrors(errors);
        });


    //fetch(url, {
    //    method: form.method || "POST",
    //    body: new FormData(form)
    //})
    //    .then(async res => {

    //        if (res.ok) {

    //            bootstrap.Modal
    //                .getInstance(document.getElementById('employeeModal'))
    //                .hide();

    //            UI.toast("Saved successfully");

    //            if (window.employeeTable) {
    //                employeeTable.load();
    //            }

    //            return;
    //        }

    //        const errors = await res.json();

    //        displayValidationErrors(errors);
    //    })
    //    .catch(() => {
    //        UI.toast("Unexpected server error", "danger");
    //    })
    //    .finally(() => {

    //        UI.enableButton(submitBtn);
    //        UI.hideLoader();
    //    });
});