var testsList = {
    init: function () {
        $('#testsList').on('dblclick', "tr", this._onRowDoubleClick);
        $('#addNewTestButton').on('click', $.proxy(this._addNewTestButtonClicked, this));
    },
    
    _addNewTestButtonClicked: function() {
        testsDetails.showDialog(0);
    },
    
    _onRowDoubleClick: function() {
        var aData = datatable.fnGetData(datatable.fnGetPosition(this));
        var itemId = aData[0];
        testsDetails.showDialog(itemId);
    }
};

$(document).ready(function () {
    testsList.init();
});