
function WatchingTimer(id) {
    var interval_delay = 1000;
    var secs = 0;
    var _conceptId = id;
    var isCreated = false;

    this.setId = function (id) {
        _conceptId = id;
    }

    this.resumeTimer = function () {
        if (isCreated) {
            clearInterval(WatchingTimer.myInterval);
            WatchingTimer.myInterval = setInterval(interval_function, interval_delay);
        }
    }

    this.pauseTimer = function () {
        clearInterval(WatchingTimer.myInterval);
    }

    this.startTimer = function () {
        isCreated = true;
        this.resumeTimer();
    }

    this.stopTimer = function () {
        isCreated = false;
        this.pauseTimer();
    }

    this.clearTimer = function () {
        secs = 0;
    }

    function interval_function() {
        console.log(secs);
        if (++secs == 10) {
            sendQuery();
            secs = 0;
        }
    }

    function sendQuery() {
        $.ajax({
            dataType: 'json',
            type: 'PUT',
            url: 'api/watchingtime/' + _conceptId
        });
    }
}