var Projects = {
    clearFields: function() {
        $('#btn_addProject').click(function() {
            $('#tb_projectName').test = "";
            $('#cb_isChosen').checked = false;
        });
    },

    showProjectInfo: function() {
        $('#projectNameList').change(function() {
            var chosenProjectId = $(this).val();

            alert("Выбран новый проект!");
        });
    }
    

};

$(document).ready(function () {
    clearFields();
    showProjectInfo();
});