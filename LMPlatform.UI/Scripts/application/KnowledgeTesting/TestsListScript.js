var testsList = {
    init: function() {
        $('.editButton').on('click', 'span', $.proxy(this._onEditClicked, this));
        $('.deleteButton').on('click', 'span', $.proxy(this._onDeleteClicked, this));
        $('#addNewTestButton').on('click', $.proxy(this._addNewTestButtonClicked, this));
    },

    _addNewTestButtonClicked: function() {
        testsDetails.showDialog(0);
    },

    _onEditClicked: function(eventArgs) {
        var itemId = eventArgs.target.dataset.modelId;
        testsDetails.showDialog(itemId);
    },

    _onDeleteClicked: function (eventArgs) {
        var context = {
            itemId: eventArgs.target.dataset.modelId
        };
        
        bootbox.confirm('Вы действительно хотите удалить этот тест?', $.proxy(this._onDeleteConfirmed, context));
    },
    
    _onDeleteConfirmed: function () {
        testsDetails.deleteTest(this.itemId);
    }
};

function initTestList() {
    testsList.init();
}