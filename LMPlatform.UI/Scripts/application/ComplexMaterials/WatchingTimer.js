
function WatchingTimer(id) {
    var interval_delay = 1000;
    var secs = 0;
    var _conceptId = id;

    this.setId = function (id) {
        _conceptId = id;
    }

    this.startTimer = function () {
        clearInterval(WatchingTimer.myInterval);
        WatchingTimer.myInterval = setInterval(interval_function, interval_delay);
    }

    this.stopTimer = function () {
        clearInterval(WatchingTimer.myInterval);
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