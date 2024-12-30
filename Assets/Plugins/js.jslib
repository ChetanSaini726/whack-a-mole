mergeInto(LibraryManager.library, {
    CallExternalJS: function (message) {
        getWAMScore(UTF8ToString(message));
    },
});