// video ---------------------
extend(VideoLoader, BaseLoader)

function VideoLoader(dataContainer, loader, doc, dialog) {
    BaseLoader.call(this, dataContainer, loader, doc, dialog);
}

VideoLoader.prototype.loadData = function (filePath) {
    this.startSpin();
    this.clearDataContainer();
    debugger;
    var video = $('<video />', {
        id: 'video',
        src: filePath,
        controls: true
    });
    video.appendTo(this.$dataContainer);
    video.width = this.$doc.width();
    video.height = this.$doc.height();
    this.stopSpin();
}
// end video ---------------------

