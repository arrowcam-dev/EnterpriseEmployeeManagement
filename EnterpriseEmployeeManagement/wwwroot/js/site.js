function showToast(message, type = "success") {
    const toast = document.getElementById("globalToast");
    const text = document.getElementById("toastMessage");

    toast.className = "toast align-items-center text-bg-" + type + " border-0";

    text.innerText = message;

    new bootstrap.Toast(toast).show();
}