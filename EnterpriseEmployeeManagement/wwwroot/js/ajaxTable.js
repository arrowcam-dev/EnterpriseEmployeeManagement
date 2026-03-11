const AjaxTable = function (config) {

    let state = {
        sortColumn: config.defaultSort || "id",
        sortDirection: "asc"
    };

    function getSearch() {
        return document.querySelector(config.searchBox)?.value || "";
    }

    function getPageSize() {
        return document.querySelector(config.pageSize)?.value || 10;
    }

    function showLoader() {
        document.querySelector(config.loader)?.classList.remove("d-none");
        document.querySelector(config.table)?.classList.add("table-loading");
    }

    function hideLoader() {
        document.querySelector(config.loader)?.classList.add("d-none");
        document.querySelector(config.table)?.classList.remove("table-loading");
    }

    function saveState(page) {

        const data = {
            search: getSearch(),
            pageSize: getPageSize(),
            sortColumn: state.sortColumn,
            sortDirection: state.sortDirection,
            page: page
        };

        localStorage.setItem(config.stateKey, JSON.stringify(data));
    }

    function loadState() {
        const s = localStorage.getItem(config.stateKey);
        return s ? JSON.parse(s) : null;
    }

    function updateSortIcons() {

        config.columns.forEach(col => {
            const icon = document.getElementById("sortIcon-" + col);
            if (icon) icon.className = "";
        });

        const icon = document.getElementById("sortIcon-" + state.sortColumn);

        if (icon) {
            icon.className =
                state.sortDirection === "asc"
                    ? "bi bi-arrow-up"
                    : "bi bi-arrow-down";
        }
    }

    function load(page = 1) {

        showLoader();

        saveState(page);

        const url =
            `${config.url}?search=${getSearch()}&page=${page}&pageSize=${getPageSize()}&sortColumn=${state.sortColumn}&sortDirection=${state.sortDirection}`;

        fetch(url)
            .then(r => r.text())
            .then(html => {

                document.querySelector(config.table).innerHTML = html;

                updateSortIcons();

                if (config.afterLoad)
                    config.afterLoad();
            })
            .finally(hideLoader);
    }

    function sort(column) {

        if (state.sortColumn === column)
            state.sortDirection = state.sortDirection === "asc" ? "desc" : "asc";
        else {
            state.sortColumn = column;
            state.sortDirection = "asc";
        }

        load(1);
    }

    function restore() {

        const s = loadState();

        if (!s) return load();

        document.querySelector(config.searchBox).value = s.search || "";
        document.querySelector(config.pageSize).value = s.pageSize || 10;

        state.sortColumn = s.sortColumn;
        state.sortDirection = s.sortDirection;

        load(s.page || 1);
    }

    function clearState() {

        if (config.stateKey)
            localStorage.removeItem(config.stateKey);

        // reset UI controls
        if (config.searchBox)
            document.querySelector(config.searchBox).value = "";

        if (config.pageSize)
            document.querySelector(config.pageSize).value = config.defaultPageSize || 10;

        // reset sorting
        state.sortColumn = config.defaultSort || "id";
        state.sortDirection = "asc";

        updateSortIcons();

        load(1);
    }

    function bind() {

        let searchTimeout;

        document.querySelector(config.searchBox)
            ?.addEventListener("keyup", () => {

                clearTimeout(searchTimeout);

                searchTimeout = setTimeout(() => load(1), 400);
            });

        document.querySelector(config.pageSize)
            ?.addEventListener("change", () => load(1));
    }

    bind();

    return { load, sort, restore, clearState };
};