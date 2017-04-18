// video ---------------------
extend(VideoLoader, BaseLoader)

function VideoLoader(dataContainer, loader, doc, dialog, timer) {
    BaseLoader.call(this, dataContainer, loader, doc, dialog);
    VideoLoader.prototype.timer = timer;
}

VideoLoader.prototype.loadData = function (filePath) {
    this.startSpin();
    this.clearDataContainer();
    var video = $('<video />', {
        id: 'video',
        src: filePath,
        controls: true
    });
    video.on("pause", function (e) {
        VideoLoader.prototype.timer.stopTimer();
        console.log("video paused");
    });
    video.on("play", function (e) {
        VideoLoader.prototype.timer.startTimer();
        console.log("video resume");
    });
    video.appendTo(this.$dataContainer);
    video.width = this.$doc.width();
    video.height = this.$doc.height();
    this.stopSpin();
}
// end video ---------------------

