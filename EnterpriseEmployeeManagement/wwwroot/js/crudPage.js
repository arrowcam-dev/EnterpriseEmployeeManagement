const CrudPage = (function () {

    let table;

    function init(config) {

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

        ModalForms.bindActions(config.pageContainer, {

            createUrl: config.createUrl,
            editUrl: config.editUrl,
            deleteUrl: config.deleteUrl,

            modalId: config.modalId,
            bodyId: config.modalBodyId,

            deleteModalId: config.deleteModalId,
            deleteConfirmBtn: config.deleteConfirmBtn,

            afterDelete: () => table.load()
        });
    }
    function sort(column) {

        if (table)
            table.sort(column);
    }

    function reload() {

        if (table)
            table.load();
    }

    function reset() {
        if (table)
            table.clearState();
    }
    function loadPage(page) {

        if (table)
            table.load(page);
    }

    return { init, reset, reload, sort, loadPage };

})();