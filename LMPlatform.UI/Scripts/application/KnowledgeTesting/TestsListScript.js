var testsList = {
    init: function () {
        $('#cardsList').on("dblclick", "tr", function () {
            var aData = datatable.fnGetData(datatable.fnGetPosition(this));
            var name = aData[1];
            testsDetails.showDialog(name);
        });
    }
};

$(document).ready(function () {
    testsList.init();
});