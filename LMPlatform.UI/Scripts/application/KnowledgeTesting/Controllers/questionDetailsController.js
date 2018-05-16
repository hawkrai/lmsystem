'use strict';
knowledgeTestingApp.controller('questionDetailsCtrl', function ($scope, $http, id, subjectId, $modalInstance) {
    $scope.BeforeEUMK = false;
    $scope.ForEUMK = false;
    $scope.question = { QuestionType: 0 };
    $scope.types = [{ Id: 0, Name: 'С одним вариантом' }, { Id: 1, Name: 'С несколькими вариантами' }, { Id: 2, Name: 'Ввод с клавиатуры' }, { Id: 3, Name: 'Последовательность элементов' }];

    $scope.deleteAnswer = function (index) {
        if ($scope.question.Answers.length == 1) {
            alertify.error('Единственный вариант ответа не может быть удален.');
            return;
        }
        $scope.question.Answers.splice(index, 1);
    };

    $scope.questionTypeChanged = function () {
        if ($scope.question.QuestionType === 0) {
            $scope.question.Answers.forEach(function (answer) {
                answer.IsCorrect = 0;
            });
        }
    }

    var data = {
        name: "node1",
        children: [{ name: "node21", children: [] },
        {
            name: "node22", children: [
                { name: "node31", children: [] },
                { name: "node32", children: [] },
                {
                    name: "node33", children: [
                        { name: "node41", children: [] }]
                }]
        },
        { name: "node23", children: [] }]

    }

    $scope.treeNodeClicked = function (e) {
        $scope.question.ConceptId = $(this).attr('id');
        addEUMKConceptButton($(this).text());
    }

    function addEUMKConceptButton(name) {
        var removeControl = $("<i class='glyphicon glyphicon-remove' style='padding-left:5px;cursor:pointer' title='Удалить'></i>")
        var container = $(".concept-container");
        removeControl.on("click", function () {
            container.html("");
            $scope.question.ConceptId = null;
        });
        container.html(name);
        removeControl.appendTo(container);
    }

    function findConceptById(data, index) {
        for (var i = 0; i < data.length; i++) {
            if (data[i].Id == index) {
                return data[i];
            }
            else if (data[i].Children != undefined && data[i].Children.length > 0) {
                return findConceptById(data[i].Children, index);
            }
        }
        return null;
    }

    $scope.buildChildNodes = function (child, rootRef) {

        var hasChildren = child.Children.length > 0;
        var li, ul;
        if (hasChildren) {
            li = $("<li class='dropdown-submenu'>");
            var ul = $("<ul class='dropdown-menu'>");
        }
        else
            li = $("<li>");
        var a = $('<a tabindex="-1" id="' + child.Id + '">' + child.Name + '</a>');
        a.appendTo(li);
        a.on("click", $scope.treeNodeClicked)
        if (hasChildren)
            ul.appendTo(li);
        li.appendTo(rootRef);
        for (var i = 0; i < child.Children.length; i++) {
            $scope.buildChildNodes(child.Children[i], ul)
        }
    }


    $scope.initConceptTree = function (data) {
        var root = $("#conceptTree");
        for (var i = 0; i < data.length; i++) {
            var li;
            var hasChildren = data[i].Children.length > 0;
            if (hasChildren) {
                li = $("<li class='dropdown-submenu'>");
                var ul = $("<ul class='dropdown-menu'>");
            }
            else
                li = $("<li'>");
            var a = $('<a tabindex="-1">' + data[i].Name + '</a>');
            a.appendTo(li);
            li.appendTo(root);
            var ul = $("<ul class='dropdown-menu'>");
            for (var j = 0; j < data[i].Children.length; j++) {
                $scope.buildChildNodes(data[i].Children[j], ul)
            }
            if (hasChildren)
                ul.appendTo(li);
        }

    }

    $http({ method: 'GET', url: kt.actions.questions.getConcepts, dataType: 'json', params: { subjectId: subjectId } })
        .success(function (data) {
            $scope.concepts = data;
            $scope.initConceptTree(data);
            $scope.loadQuestionData();
            $scope.BeforeEUMK = getHashValue('beforeEUMK') == 'true';
            $scope.ForEUMK = getHashValue('forEUMK') == 'true';
        })
        .error(function (data, status, headers, config) {
            alertify.error('Во время получения данных произошла ошибка');
        });


    $scope.loadQuestionData = function () {
        $http({ method: 'GET', url: kt.actions.questions.getQuestion, dataType: 'json', params: { id: id } })
            .success(function (data) {
                $scope.question = data;

                var result = findConceptById($scope.concepts, $scope.question.ConceptId);
                addEUMKConceptButton(result.Name);
            })
            .error(function (data, status, headers, config) {
                alertify.error('Во время получения данных произошла ошибка');
            });
    }



    $scope.radioGroupChanged = function (answer) {
        $scope.question.Answers.forEach(function (item) {
            if (item !== answer) {
                item.IsCorrect = 0;
            }
        });
    };

    $scope.saveQuestion = function () {
        $scope.question.TestId = getHashValue('testId');

        $.ajax({
            url: kt.actions.questions.saveQuestion,
            type: "POST",
            data: JSON.stringify($scope.question),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data.ErrorMessage) {
                    alertify.error(data.ErrorMessage);
                } else {
                    $scope.loadQuestions();
                    $scope.closeDialog();
                    alertify.success('Вопрос успешно сохранен');
                }
            },
            error: function (data) {
                alertify.error("Необработанная ошибка: " + data.statusText);
            }
        });
    };

    $scope.closeDialog = function () {
        $modalInstance.close();
    };

    function aveTest(test) {

    }
});