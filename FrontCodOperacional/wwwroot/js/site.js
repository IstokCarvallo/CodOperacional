window.layoutUtils = {
    isMobile: function () {
        return window.innerWidth <= 768;
    }
};

window.theme = {
    toggle: function () {
        const current = document.documentElement.getAttribute("data-theme");
        const next = current === "dark" ? "light" : "dark";

        document.documentElement.setAttribute("data-theme", next);
        localStorage.setItem("theme", next);
    },

    load: function () {
        const saved = localStorage.getItem("theme") || "light";
        document.documentElement.setAttribute("data-theme", saved);
    }
};

window.dropdown = {
    init: function (selector, dotnetHelper) {

        document.addEventListener("click", function (e) {

            const menu = document.querySelector(selector);

            if (!menu) return;

            if (!menu.contains(e.target)) {
                dotnetHelper.invokeMethodAsync("CloseDropdown");
            }

        });
    }
};