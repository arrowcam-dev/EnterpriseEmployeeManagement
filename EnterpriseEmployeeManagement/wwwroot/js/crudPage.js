const CrudPage = (function () {

    let table;
    let config;

    function init(cfg) {

        config = cfg;

        table = AjaxTable({
            url: config.tableUrl,
            table: config.tableSelector,
            loader: config.loaderSelector,
            searchBox: config.searchSelector,
            pageSize: config.pageSizeSelector,
            stateKey: config.stateKey,
            columns: config.columns,
            defaultSort: config.defaultSort
        });

        table.restore();

        bindEvents(config.pageContainer);

    }

    function bindEvents(container) {

        document.querySelector(container)
            .addEventListener("click", function (e) {

                const sortBtn = e.target.closest("[data-sort]");
                const pageBtn = e.target.closest("[data-page]");
                const actionBtn = e.target.closest("[data-action]");

                if (sortBtn) {

                    e.preventDefault();

                    const column = sortBtn.dataset.sort;

                    table.sort(column);
                }

                if (pageBtn) {

                    e.preventDefault();

                    const page = pageBtn.dataset.page;

                    table.load(page);
                }

                if (actionBtn) {

                    e.preventDefault();

                    const action = actionBtn.dataset.action;
                    const id = actionBtn.dataset.id;
                    const rowName = actionBtn.dataset.name;

                    handleAction(action, id, rowName);
                }
            });
    }

    function handleAction(action, id, rowName) {

        if (action === "reset") table.clearState();

        if (action === "create") {
            ModalForms.open(config.createUrl, "Create");
        }

        if (action === "edit") {
            ModalForms.open(
                config.editUrl.replace("{id}", id),
                "Edit"
            );
        }
        if (action === "details")
            ModalForms.open(
                config.detailsUrl.replace("{id}", id),
                "Details"
            );

        //if (action === "delete") {
        //    ModalForms.confirmDelete(id, config.deleteUrl, () => table.load());
        //}
        if (action === "delete") {
            
            ModalForms.confirm({

                title: "Delete Employee",
                message: `Are you sure you want to delete <strong>${rowName || "this record"}</strong>?`,
                confirmText: "Delete",
                btnClass: "btn btn-danger",
                onConfirm: () => {

                    Api.delete(config.deleteUrl.replace("{id}", id))
                        .then(() => {

                            UI.toast("Deleted successfully");

                            table.load();
                        });

                }

            });
        }
    }
    function reload() {

        if (table) table.load();
    }

    return { init, reload };

})();