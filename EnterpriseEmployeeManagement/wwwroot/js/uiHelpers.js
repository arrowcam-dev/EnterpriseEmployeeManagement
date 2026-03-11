const UI = {

    showLoader() {
        document.getElementById("globalLoader")?.classList.remove("d-none");
    },

    hideLoader() {
        document.getElementById("globalLoader")?.classList.add("d-none");
    },

    disableButton(btn) {
        if (!btn) return;

        btn.dataset.originalText = btn.innerHTML;
        btn.disabled = true;

        btn.innerHTML =
            '<span class="spinner-border spinner-border-sm"></span> Processing';
    },

    enableButton(btn) {

        if (!btn) return;

        btn.disabled = false;

        if (btn.dataset.originalText)
            btn.innerHTML = btn.dataset.originalText;
    },

    toast(message, type = "success") {

        const toast = document.getElementById("globalToast");
        const text = document.getElementById("toastMessage");

        toast.className = "toast align-items-center text-bg-" + type + " border-0";

        text.innerText = message;

        new bootstrap.Toast(toast).show();
    },
    enableFormValidation(form) {

        if (!form) return;

        $(form).removeData("validator");
        $(form).removeData("unobtrusiveValidation");

        $.validator.unobtrusive.parse(form);
    },
    displayValidationErrors(errors) {

        document
            .querySelectorAll("[data-valmsg-for]")
            .forEach(x => x.innerText = "");

        let firstField = null;

        for (const key in errors) {

            const field = document.querySelector(`[data-valmsg-for="${key}"]`);

            if (field) {

                field.innerText = errors[key][0];

                if (!firstField) {
                    firstField = document.querySelector(`[name="${key}"]`);
                }
            }
        }

        firstField?.focus();
    }

};