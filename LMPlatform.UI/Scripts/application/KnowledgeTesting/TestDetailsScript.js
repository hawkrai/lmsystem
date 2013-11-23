var testsDetails = {
    showDialog: function (id) {
        var viewModel = {
            Title: 'KnockoutTest'
        };

        koWrapper.createOrUpdateViewModel(viewModel);
        $('#testDetails').modal();
    }
};