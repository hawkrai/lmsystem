/// base loader ---------------------------
function BaseLoader(dataContainer, loader, doc, dialog) {
    this.$dataContainer = dataContainer;
    this.$loader = loader;
    this.$doc = doc;
    this.$dialog = dialog;
    this.uiHelper = new DilogUIHelper(doc, dataContainer);
}

BaseLoader.prototype.clearDataContainer = function () {
    this.$dataContainer.empty();
}

BaseLoader.prototype.startSpin = function () {
    this.$loader.toggleClass('ng-hide', false);
};

BaseLoader.prototype.stopSpin = function () {
    this.$loader.toggleClass('ng-hide', true);
};

BaseLoader.prototype.showEmptyContainer = function (message) {
    this.uiHelper.showEmptyContainer(message);
}

BaseLoader.prototype.updateHeader = function (name) {
    var prefix = "Просмотр файла";
    var title = prefix;
    if (name && name.length > 0)
        title = prefix + ' "' + name + '"';

    this.$dialog.html(title);
}

//end base loader -------------------------