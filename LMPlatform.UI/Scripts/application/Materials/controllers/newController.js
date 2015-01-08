
angular
    .module('materialsApp.ctrl.new', ['ngResource'])
    .controller('newCtrl', [
        '$scope',
        '$location',
        '$resource',
        "materialsService",
        function ($scope, $location, $resource, materialsService) {


            function saveText(obj) {
                var text = obj.getContent();
                data = {
                    idf: "1049",
                    name: "slaq",
                    text: text
                }
                materialsService.saveText(datah).success(function (data) {
                    //$scope.folders = data.Folders;
                });
            }

            tinymce.init({
                save_enablewhendirty: true,
                save_onsavecallback: function (dr) {
                    saveText(dr);
                },
                selector: "textarea",
                autoresize_min_height: "450px",
                autoresize_max_height: "500px",
                language: 'ru',
                skin: 'light',
                plugins: [
                    "pagebreak save advlist autolink lists link image charmap print preview anchor",
                    "searchreplace visualblocks code fullscreen",
                    "insertdatetime media table contextmenu paste autoresize"
                ],
                pagebreak_separator: "<!-- my page break -->",
                toolbar: "save | insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image"
            });

        }]);
    
    
