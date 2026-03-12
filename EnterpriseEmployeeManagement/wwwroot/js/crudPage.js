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

        //ModalForms.bindActions(config.pageContainer, {

        //    createUrl: config.createUrl,
        //    editUrl: config.editUrl,
        //    deleteUrl: config.deleteUrl,

        //    modalId: config.modalId,
        //    bodyId: config.modalBodyId,

        //    deleteModalId: config.deleteModalId,
        //    deleteConfirmBtn: config.deleteConfirmBtn,

        //    afterDelete: () => table.load()
        //});
        bindEvents(config.pageContainer);

    }
    //function sort(column) {
    //    if (table) table.sort(column);
    //}
    //function reset() {
    //    if (table) table.clearState();
    //}
    //function loadPage(page) {

    //    if (table) table.load(page);
    //}

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

                    handleAction(action, id);
                }
            });
    }

    function handleAction(action, id) {

        if (action === "reset") table.clearState();

        if (action === "create") {
            ModalForms.openModal(config.createUrl,
                config.modalId,
                config.modalBodyId);
        }

        if (action === "edit") {
            ModalForms.openModal(
                config.editUrl.replace("{id}", id),
                config.modalId,
                config.modalBodyId
            );
        }

        if (action === "delete") {
            ModalForms.confirmDelete(id, config.deleteUrl, () => table.load());
        }
    }
    function reload() {

        if (table) table.load();
    }

    return { init, reload };

})();