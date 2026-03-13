// sidebar toggle

const toggleSidebarBtn = document.getElementById("sidebarToggle")

toggleSidebarBtn.addEventListener("click", () => {

    const sidebar = document.getElementById("sidebar")

    sidebar.classList.toggle("collapsed")

    localStorage.setItem(
        "sidebarCollapsed",
        sidebar.classList.contains("collapsed")
    )

})

// dark mode

const darkToggleBtn = document.getElementById("darkModeToggle")

darkToggleBtn.addEventListener("click", () => {

    const html = document.documentElement

    const theme = html.getAttribute("data-bs-theme")

    if (theme === "dark") {

        html.setAttribute("data-bs-theme", "light")
        localStorage.setItem("theme", "light")

    } else {

        html.setAttribute("data-bs-theme", "dark")
        localStorage.setItem("theme", "dark")

    }

})

window.addEventListener("DOMContentLoaded", () => {
    const saved = localStorage.getItem("theme")

    if (saved) {
        document.documentElement.setAttribute("data-bs-theme", saved)
    }

    const collapsed = localStorage.getItem("sidebarCollapsed")

    if (collapsed === "true") {

        document
            .getElementById("sidebar")
            .classList.add("collapsed")

    }
})