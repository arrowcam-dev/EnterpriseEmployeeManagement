const Api = (function () {

    async function request(url, options = {}, config = {}) {
        const useLoader = config.loader !== false;

        try {

            if (useLoader) UI.showLoader();

            const response = await fetch(url, options);

            if (!response.ok) {

                let error;

                try {
                    error = await response.json();
                }
                catch {
                    error = { message: "Server error" };
                }

                throw error;
            }

            const contentType = response.headers.get("content-type");

            if (contentType && contentType.includes("application/json"))
                return await response.json();

            return await response.text();
        }
        finally {
            if (useLoader) UI.hideLoader();
        }
    }

    function get(url, config = {}) {

        return request(url, {
            method: "GET"
        }, config);
    }

    function post(url, data, config = {}) {

        return request(url, {
            method: "POST",
            body: data
        }, config);
    }

    function del(url, config = {}) {

        return request(url, {
            method: "DELETE"
        }, config);
    }

    return {
        get,
        post,
        delete: del
    };

})();