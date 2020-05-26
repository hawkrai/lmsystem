angular
    .module('complexMaterialsApp.service.navigation', [])
    .factory('navigationService', ['complexMaterialsDataService', '$http', function (complexMaterialsDataService, $http) {
        var defValue = '***ЭУМК не выбран***';
        var title = defValue;
        var $container = angular.element("#complex-header");
        var $subjLink = angular.element("#subjectLink");
        var breadCrumbsArray = [];
        var initedHeader = false;
        var currentSubjectId = 0;
        var tree;
        var url = '/Services/Concept/ConceptService.svc/';
        return {
            currentLecturer: {},
            updateTitle: function (id) {
                var self = this;
                currentSubjectId = id;
                $http({
                    method: 'GET',
                    url: url + 'GetConceptTitleInfo?subjectId=' + id
                }).success(function (data) {
                    title = data.Subject.Name;
                    currentLecturer = data.Lecturer;
                    self._updateTitleContainer();
                });
            },
            title: function () {
                return title;
            },
            getBreadcrumbs: function () {
                return breadCrumbsArray;
            },
            setTree: function(data){
                tree = data;
            },
            setNavigation: function (newNavObj, actionType) {
                title = newNavObj.SubjectName;
                //this._updateTitleContainer();
                if(actionType==="inc")
                    if(newNavObj.ParentId > 0 && this.getBreadcrumbs().length == 0)
                        this._buildBreadCrumbsByParent(newNavObj)
                    else
                        this._pushBreadCrumb(newNavObj)
                else
                    this._popBreadCrumb()
            },
            setHomeNavigation: function (obj) {
                if (obj && obj.SubjectName)
                    title = obj.SubjectName;
                else
                    title = defValue;
                //this._updateTitleContainer();
                this._clearBreadCrums();
                initedHeader = false;
            },
            buildBreadCrumbs: function (currentId) {
                var item = this._findBreadCrumbById(currentId);
                var index = item.index;
                for (var i = breadCrumbsArray.length-1; i >= index; i--) {
                    this._popBreadCrumb();
                }

                return item.obj;
            },
            _buildBreadCrumbsByParent: function (parent) {
                this._clearBreadCrums();
                var arr = [];
                var item = this._searchTree(tree, parent.Id);
                arr.push(item);
                while (item.ParentId>0)
                {
                    item = this._searchTree(tree, item.ParentId);
                    if (item != null)
                        arr.push(item);
                }
                for (var i = arr.length - 1; i >= 0; i--) {
                    this._pushBreadCrumb(arr[i]);
                }
                if (breadCrumbsArray.length > 0)
                {
                    title = breadCrumbsArray[0].Name;
                    //this._updateTitleContainer();
                }
                return parent;
            },

            _searchTree: function(element, id){
                if(element.Id == id){
                    return element;
                }else if (element.children != null){
                    var result = null;
                    for(var i=0; result == null && i < element.children.length; i++){
                        result = this._searchTree(element.children[i], id);
                    }
                    return result;
                }
                return null;
            },
            _findIndexBreadCrumbById : function(id){
                var result = this._findBreadCrumbById(id);
                if (result)
                    return result.index;
                return breadCrumbsArray.length;
            },
            _findBreadCrumbById: function(id){
                for (var i = 0; i < breadCrumbsArray.length; i++) {
                    if (breadCrumbsArray[i].Id == id)
                        return { index: i, obj: breadCrumbsArray[i] };
                }
                return null;
            },
            _pushBreadCrumb: function (obj) {
                if (!this._findBreadCrumbById(obj.Id))
                    breadCrumbsArray.push(obj);
            },
            _popBreadCrumb: function () {
                breadCrumbsArray.pop();
            },
            _clearBreadCrums:function(){
                breadCrumbsArray = [];
            },
            _updateTitleContainer: function () {
                if (!initedHeader) {
                    $container.html("");
                    $subjLink.html("");
                    if (currentSubjectId) {
                        var str = "<span>" + title + "</span>";
                        if (currentLecturer != undefined) {
                            str = str + "<small> Преподаватель - <a href=\"/Profile/Page/" + currentLecturer.UserName + "\">" + currentLecturer.FullName + "</a></small>";
                        }
                        var link = $(str);
                        link.appendTo($container);

                        str = "<a href='/Subject?subjectId=" + currentSubjectId + "' linkNavbar><i class='fa fa-arrow-left'></i>В предмет</a>";
                        var backLink = $(str);
                        backLink.appendTo($subjLink);
                    }
                    else {
                        $container.html(title);
                    }
                    initedHeader = true;
                }
            }
        };
    }]);
