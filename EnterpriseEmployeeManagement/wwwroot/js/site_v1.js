function showToast(message, type = "success") {
    const toast = document.getElementById("globalToast");
    const text = document.getElementById("toastMessage");

    toast.className = "toast align-items-center text-bg-" + type + " border-0";

    text.innerText = message;

    new bootstrap.Toast(toast).show();
}

function showLoader() {
    document
        .getElementById("globalLoader")
        .classList.remove("d-none");
}

function hideLoader() {
    document
        .getElementById("globalLoader")
        .classList.add("d-none");
}

function disableButton(btn) {
    if (!btn) return;

    btn.dataset.originalText = btn.innerHTML;

    btn.disabled = true;

    btn.innerHTML =
        '<span class="spinner-border spinner-border-sm"></span> Processing';
}

function enableButton(btn) {
    if (!btn) return;

    btn.disabled = false;

    if (btn.dataset.originalText)
        btn.innerHTML = btn.dataset.originalText;
}
