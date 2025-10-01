// wwwroot/js/site.js (or create a new JS file and link it in _Layout.cshtml)

window.localStorageFunctions = {
    setItem: function (key, value) {
        localStorage.setItem(key, value);
    },
    getItem: function (key) {
        return localStorage.getItem(key);
    },
    removeItem: function (key) {
        localStorage.removeItem(key);
    }
};

// Your existing updateBodyAttributes, loadThemeConfig, loadApps functions
window.updateBodyAttributes = (attributeName, value) => {
    document.body.setAttribute(attributeName, value);
};

window.loadThemeConfig = () => {
    // Implement your theme loading logic here
    console.log("Theme config loaded.");
};

window.loadApps = () => {
    // Implement your app loading logic here
    console.log("Apps loaded.");
};