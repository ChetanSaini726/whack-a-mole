mergeInto(LibraryManager.library, {
    CallExternalJs: function (message) {
        getWAMScore(UTF8ToString(message));
    },
});