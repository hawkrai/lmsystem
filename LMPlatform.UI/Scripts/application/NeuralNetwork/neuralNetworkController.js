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
		var topicsNum = [];
		var currentTopic = $scope.questions[0].ConceptId;
		var currentNumb = 0;
		$.each($scope.questions, function (index, value) {
			argumentsD.push([0, 1]);
			queestions.push(value.Title);
			
			if (currentTopic === value.ConceptId) {
				currentNumb += 1;
			} else {
				topicsNum.push(currentNumb);
				currentTopic = value.ConceptId;
				currentNumb += 1;
			}

			if ($.inArray(value.ConceptId, topics) === -1) {
				topics.push(value.ConceptId);
			}
		});

		topicsNum.push(currentNumb);

		var cartesian = $scope.cartesian.apply(this, argumentsD);
		cartesian.unshift(queestions);
		$scope.drawTable(cartesian, "testData");

		var dataForTopic = $scope.cartesian.apply(this, argumentsD);

		$scope.savedDataAnswersValue = JSON.parse(JSON.stringify(dataForTopic));

		var topicsValue = [];
		$.each(dataForTopic, function (index, value) {

			var tValue = [];
			var currentBorder = 0;
			$.each(topicsNum, function (index, valueN) {
				var sumValue = 0; 
				for (currentBorder; currentBorder < valueN; currentBorder++) {
					sumValue += value[currentBorder] * ($scope.questions[currentBorder].ComlexityLevel / 10);
				}

				tValue.push(sumValue < 0.7 ? 1 : 0);
			});

			topicsValue.push(tValue);
		});

		$scope.savedTopicsValue = JSON.parse(JSON.stringify(topicsValue));

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

	$scope.drawTable = function(testData, testDataH) {
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
	};

	$scope.opacity = 0; //opacity of image
	$scope.increase = 1; //increase opacity indicator
	$scope.decrease = 0; //decrease opacity indicator

	$scope.fade = function() {
		if ($scope.opacity < 0.6 && $scope.increase)
			$scope.opacity += 0.05;
		else {
			$scope.increase = 0;
			$scope.decrease = 1;
		}

		if ($scope.opacity > 0.3 && $scope.decrease)
			$scope.opacity -= 0.05;
		else {
			$scope.increase = 1;
			$scope.decrease = 0;
		}

		document.getElementById("iconNN").style.opacity = $scope.opacity;
	};

	$scope.train = function() {
		//$scope.savedDataAnswersValue
		//$scope.savedTopicsValue
		$(".nnContainer").show();
		$scope.refreshIntervalId = setInterval(function () {
			$scope.fade();
		}, 100);

		var data = [];

		$.each($scope.savedDataAnswersValue, function (index, value) {
			var temp = { input: value, output: $scope.savedTopicsValue[index] };
			data.push(temp);
		});

		setTimeout(function () {
			neuralNetworkV2.train(data, { log: true });
			clearInterval($scope.refreshIntervalId);
			$(".nnContainer").hide();
		}, 100);
	};

	$scope.closeDialog = function () {
		clearInterval($scope.refreshIntervalId);
		$modalInstance.close();
	};
});