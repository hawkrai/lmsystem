'use strict';
knowledgeTestingApp.controller('createNeuralNetworkCtrl', function ($scope, $http, data, $modalInstance) {
	//$scope.SelectedGroup = 'A';
	var subjectId = getUrlValue('subjectId');

	$scope.questions = data;
	$scope.context = $scope;

	$scope.showQuestions = function () {
		var container = $.find(".part1 .questions .row");

		$.each($scope.questions, function (index, value) {
			var div = '<div class="col-md-4">' + value.Title + '</div> <div class="col-md-4"> Сложность ' + value.ComlexityLevel + '</div> <div> Тема ' + value.ConceptId + ' </div>';
			$(container).append(div);
		});

		$(".generateData").show();
		$(".showQuestions").hide();
	};

	$scope.generateData = function () {
		var argumentsD = [];

		var queestions = [];

		var topics = [];
		$.each($scope.questions, function (index, value) {
			argumentsD.push([0, 1]);
			queestions.push(value.Title);
			
			if ($.inArray(value.ConceptId, topics) === -1) {
				topics.push(value.ConceptId);
			}
		});

		var cartesian = $scope.cartesian.apply(this, argumentsD);
		cartesian.unshift(queestions);
		$scope.drawTable(cartesian, "testData");

		var dataForTopic = $scope.cartesian.apply(this, argumentsD);
		var topicsValue = [];

		$.each(dataForTopic, function (index, value) {
			topicsValue.push([0, 0]);
		});

		topicsValue.unshift(topics);
		$scope.drawTable(topicsValue, "topicData");

		$(".part2").show();
	};

	$scope.cartesian = function() {
		var r = [], arg = arguments, max = arg.length - 1;

		function helper(arr, i) {
			for (var j = 0, l = arg[i].length; j < l; j++) {
				var a = arr.slice(0); // clone arr
				a.push(arg[i][j]);
				if (i == max)
					r.push(a);
				else
					helper(a, i + 1);
			}
		}

		helper([], 0);
		return r;
	};

	$scope.drawTable = function (testData, testDataH) {
		var tbody = document.getElementById(testDataH);
		var headerNames = testData[0];
		var columnCount = headerNames.length;
		////Add the header row.
		//var row = tbody.insertRow(-1);
		//for (var i = 0; i < columnCount; i++) {
		//	var headerCell = document.createElement("TH");
		//	headerCell.innerHTML = headerNames[i];
		//	row.appendChild(headerCell);
		//}
		var tr, td;

		// loop through data source
		for (var i = 0; i < testData.length; i++) {
			tr = tbody.insertRow(tbody.rows.length);
			td = tr.insertCell(tr.cells.length);
			td.setAttribute("align", "center");

			for (var key in testData[i]) {
				td.innerHTML = testData[i][key];
				td = tr.insertCell(tr.cells.length);
			}

		}
	}

	$scope.closeDialog = function () {
		$modalInstance.close();
	};
});