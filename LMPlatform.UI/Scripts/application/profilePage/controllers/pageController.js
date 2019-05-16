﻿angular.module("appProfilePage.controllers", ["ui.bootstrap", "angularSpinner"])
	.controller("PageController", function ($scope, $sce, $http, usSpinnerService) {
	    
	    $scope.login = "";
	    $scope.loadProfileData = null;
	    
	    $scope.statistic = {
	        Labs: 0,
	        Lects: 0
	    };

	    $scope.subject = null;
	    $scope.projects = null;

	    $scope.loading = false;
	    
	    $scope.init = function (userName) {
	        $scope.login = userName;

	        $scope.startSpin();

	        $scope.loadProfileData();

	        $scope.loadStatistic();

	        $scope.loadSubjects();
	        
	        $scope.loadProjects();

	        setTimeout(
	            function () {
	                $scope.stopSpin();
	            }, 100);
	    };

	    $scope.loaEvents = function () {
	        $.ajax({
	            type: 'POST',
	            url: "/Profile/GetProfileInfoCalendar",
	            dataType: "json",
	            contentType: "application/json",
	            async: false,
	            data: JSON.stringify({ userLogin: $scope.login }),
	        }).success(function (data, status) {
	            $scope.renderCalendar(data.Labs, data.Lect);
	        });
	    },

        $scope.loadStatistic = function () {
            $.ajax({
                type: 'POST',
                url: "/Profile/GetProfileStatistic",
                dataType: "json",
                contentType: "application/json",
                async: false,
                data: JSON.stringify({ userLogin: $scope.login }),
            }).success(function (data, status) {
                $scope.statistic = data;
            });
        },

         $scope.loadSubjects = function () {
             $.ajax({
                 type: 'POST',
                 url: "/Profile/GetProfileInfoSubjects",
                 dataType: "json",
                 contentType: "application/json",
                 async: false,
                 data: JSON.stringify({ userLogin: $scope.login }),
             }).success(function (data, status) {
                 $scope.subject = data;
             });
         },

        $scope.loadProjects = function () {
            $.ajax({
                type: 'POST',
                url: "/Profile/GetUserProject",
                dataType: "json",
                contentType: "application/json",
                async: false,
                data: JSON.stringify({ userLogin: $scope.login }),
            }).success(function (data, status) {
                $scope.projects = data;
            });
        },

	    $scope.loadProfileData = function () {
	        $.ajax({
	            type: 'POST',
	            url: "/Profile/GetProfileInfo",
	            dataType: "json",
	            contentType: "application/json",
	            async: false,
	            data: JSON.stringify({ userLogin: $scope.login }),
	        }).success(function (data, status) {
	            $scope.loadProfileData = data;
	            if ($scope.loadProfileData.Avatar == null || $scope.loadProfileData.Avatar.length == 0) {
	                $scope.loadProfileData.Avatar = "data:image/jpg;base64,/9j/4AAQSkZJRgABAgAAZABkAAD/7AARRHVja3kAAQAEAAAAUAAA/+4ADkFkb2JlAGTAAAAAAf/bAIQAAgICAgICAgICAgMCAgIDBAMCAgMEBQQEBAQEBQYFBQUFBQUGBgcHCAcHBgkJCgoJCQwMDAwMDAwMDAwMDAwMDAEDAwMFBAUJBgYJDQsJCw0PDg4ODg8PDAwMDAwPDwwMDAwMDA8MDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwM/8AAEQgBDgEOAwERAAIRAQMRAf/EAIAAAQEAAwEBAQEAAAAAAAAAAAACAQMGBwUECQEBAAAAAAAAAAAAAAAAAAAAABAAAgEDAgQCBQgHBgcAAAAAAAECEQMEMQUhQRIGUWFxgZEiE6GxwTJCcjMU0VKistIjU2KCksKjFvBDsyQ0VFURAQAAAAAAAAAAAAAAAAAAAAD/2gAMAwEAAhEDEQA/AP66AAAAAAAAAAAAAAAAM0fgBnpAdPmBnpQGOkB0+YGOlgKMDAAAAAAAAAAAAAAAAAAAAAAAAAAAZSbAz0gUAAAAAAAAAAYogMdPgBIAAAAAAAAAAAAAAAAAAAAAGUqgUkBkABmjAz0gZ6UAovADNF4AKLwAxRAY6QMdLAwAAAS4+AEgAAAAAAAAAAAAAAAAADKVQLAAZSqBVEBkBQDPSwM9PmA6fMB0+YDpAxRgYAAS4gTSgADDVQJaoBgAAAAAAAAAAAAAACkubAoABaVAMgZSYG21ZndnG1atyu3JukIRVW35JAdlgdnZN5RuZ95YsXx+DBdU6eb0XygdPY7X2axSuM78l9q7Jv5FRfIB9CO0bVHTbcb12oP50BM9m2mda7djqv6tuMfmSA+ZkdpbReT+HC5iyejtzbVfRKoHJ7j2nn4ilcxms60tVBUuJfd419TA5ZpxbTTTTo09UwMAT0gYaoBgCWvACQGoENUAwAAAAAAAAAAAAFJc2BQGUqgWlQDKVQKSoB+rExL+dkW8bGh13bjolyS5tvkkB61tGy4u02l0JXMmapeyWuL8l4ID7IAAAAAAOb3zt+zuUJX7EY2s6KqpaK5TlLz8wPLblqdqc7VyLt3LbcZweqa1TA1NNAYAlx8AJAw0BADUCGqAYAAAAAAAAAAMpVAsDKVQL0AylUCwMpVA9V7Z2mOBhrIuwpl5cVKTesYPjGP0sDpgAAAAAAAAHDd27UpRjudmNJRpDLS5rSMvVp7AOA0Aw1UCNAMNVAgDDVQIAw1UCAAAAAAAAAADYuAAC0qAUlUCwKS5sD7Ox4Kz9zx7Mo9VqL+JeXLphxo/S6ID2EAAAAAAAAAA1X7MMmxdx7qrbvQcJryaoB4rfsyx717HufXszlCXpi6AaGvACaVAhqgEtVAgCWuYEgS1zAkAAAAAAACormBQFRXMCgNi4AUkBWoHfdmYySzcprj7tqD/AGpfQB3QAAAAAAAAAAA8s7msK1u99pUV6Mbi9ao/lQHPAS1zQE6ga3wAlrmBIENUAwBDVAMAAAAAAA2AZSqBYFJcwLSqBYFpUA9O7Sh0bT1U/FvTl7Eo/QB04AAAAAAAAAAA8+7wtNZmLep7s7PQvTCTb/eA49qoEaAS1zAhqoEAQ1QCWqoCAJkBIAAAAAZjqBYFR8QLXFgWBaVEBcVzAtKoHq3bcenZsPz+I/8AUkB90AAAAAAAAAAAcf3fFOxhT5xuSivWl+gDgWuaAhqoEAQ1QDW0BLXACAIfBgYA1gAAAABa0AyBsQFRAtcWBYGwC0qID1btx12bC9Fz/qSA+2AAAAAAAAAAAOQ7vf8A2+HDm7kmvUl+kDggIaoBDQEPQDW+KAgCHqBEgJAh6gYAAAAGwDK1QFgWtEBcQNkQLXFgWgPT+2J9e024/wBK5OPy9X0gdCAAAAAAAAAAAOE7tu9WTiWf6dtzf990/wAoHISXMCHoBrfFAQBrAh6gRICHowIAmQEgAAADYBmOoFgbALWgGxaAXEDZEDu+0L9beZjN8YyjdivSul/MgOzAAAAAAAAAAAHl295H5rccq4nWMJfDh6IcPlaqB8d6AQBrA1sCHqBEtQIloBAGsDD0AgAAAytUBYFR5gWtUBYFrRAbFogNkdANkdAPtbDl/lNysTk6W7r+Fc9E9PY6MD1MAAAAAAAAAA/DuWV+TwcjIrSUY0t/elwj8oHlL41rxrqBrA1gQ9QNb1AiXIDXLkBD0AgDW9WBh6MCAAADK1AsCo8wLWqAsDYBa0QGxaAbFoBceYHqey56z8G3OUq37X8u+udVo/WuIH1wAAAAAAAAHFd05nVO1hQfC3/MvL+0/qr1L5wOQA1vVgQ9WBrlqBEtQNcuQGuQEPQCAIerAw9GBrAAAMrVAWBUeYFrVAWBa0QGxaIDZHQC46AbIgfa2TcXt2YpSf8AIvUhfXlyl6gPT01JJp1TVU1zQGQAAAAAAar96GPZu37jpC1Fyl6gPKcq/PJvXb9x+/ek5Py8vUB+YDWBrAh6ga3qBEgNcuQEPQCANbAw9AIAAAAGwDMdQLA2AWtANkdALiBsiBa1AsDuO3N16lHb8iXFf+LN81+p+gDsAAAAAAAcj3PnUjbwYS4y/mX/AEfZX0gcU3VgS9ANb0AgDWBrAh6ga5agTLQCANYGHoBAAAAA2LQDK1AsC1oBcQNkQLXBgWBsAtOoG+wpSvWoQbU5TioNapt8APYgAAAAAAeZ77buW90yVNtq41OEnzi1w9mgHxwIbqBrbAl6Aa3oBAGsDW9QJkBD0AgCZASAAAALjoBkC1oBcfAC1wYFgbALTAuL5AWnQDpe28B5WbHJlH+RiPqr4z+yvVqB6MAAAAAADmu5NveTjrLtRrexU+tLV29X7NQPPW6gS2BAEN1A1t1AiT5AQ9AIA1sCZASBD1AwAAAAMrUCwKj4AVoBsAtMC0+QFLgBsA+3tezZe5yUoxdrGT9/IkuH91c2B6ZiYlnCsQx8ePTbhz5t8234sD9IAAAAAAAHBb5sM7Dnl4MOqw/eu2VrDxaXh8wHIAQ3UCW6AQBrbqBDdQJb4AQBD4sDAGsAAAAAAGzUBoBsAqL5AUnQDYB9HB27N3GXTi2JXEnSVzSC9MnwA7rbe1MbH6bmdJZV3X4S4W0/nl/xwA6yMYxioxSjGKpGK4JLyAyAAAAAAAAAAc5ufbmJndV2w1iZD4uUV7kn5x+lAcHn7Vnbc3+Ysv4daK/D3oP18vWB8oCGwJboBAEN1AlsCAJl4ASAAAAAACovkBQFRfICgPqbftWducqYthyinSV58IR9MmB3u29o4mN03M6f5y7r8NcLafo1fr9gHWwhC3GMLcI24RVIwikkl5JAUAAAAAAAAAAAAADDSkmmk0+DT0YHN7h2vt+Z1TsJ4V5/atr3H6YaeygHDbjsG47d1Tna+PYX/Pte8kvNar1gfBAlvkBIEN1AwBr1AAAAAAAAAWnUDfYsXsm7Cxj2pXrs3SFuKq2B6LtHZ9q0oX90fxruqxYv3I/ea19XD0gdtC3C1CNu3CNu3BUjCKSSXkkBYAAAAAAAAAAAAAAAAAAAc3unbGBuHVcsr8nlPj8SC92T/tR09aA803Ha8za73wsq3RP8O7HjCa8mB8xsCQJk+QEgAAAAAAAAP1YeLfzcm1i40Hcu3XSK5Lxb8EgPZdn2XF2iwoW0rmRNfz8lrjJ+C8F5AfZAAAAAAAAAAAAAAAAAAAAAAAfmy8PHzrE8bJtq5amuKeqfJp8mgPHN62i9s+W7M6zsXKyxr/60fPzXMD4zdAIAAAAAAAAAZSbaSVW+CSA9h7a2SO1Yiu3or89kpO8+cI6qC+nzA6YAAAAAAAAAAAAAAAAAAAAAAAAAfL3jbLe7YNzGnSNz62Pcf2ZrR+h6MDw+7buWbty1di4XbUnC5B6pp0aA1gAAAAAAAAOw7P2tZudLMvRrYwaSino7j+r7NfYB6wAAAAAAAAAAAAAAAAAAAAAAAAAAADyvvXAWPn2s2EaQzY+/T+pCiftTQHFgAAAAAAAAPaO2MNYezYipSeSvj3H4ufFfs0A6AAAAAAAAAAAAAAAAAAAAAAAAAAAAHKd5Y6vbLO7T3sW7C4n5N9D/AHgPIgAAAAAAAAHq+D3fsqsWLNyV7G+FbjB9duq91JfYcgPtWt+2a/To3Kwq6Kcuj96gH07d6zeVbN2F1eMJKS+QDYAAAAAAAAAAAAAAAAAAAAAAAw2kqt0S1bA/Fd3TbbH42fj234O5GvsrUDlu4O4NoydrzMOxlq9fvRShGMZUqpJ/WpTl4geXAAAAAAAAAAADKbi002mtGgP3Wt13OxT4W4ZEEvsq5KnsrQD6VvunfbWmc5rwnCEvlcagfvt97bxD68Me8ufVCSf7MkB+2HfmSvxdvtT+7OUfnUgP2Q78sP8AF22cPu3FL54xA/VHvna39fGyo+iMH/nQH6Yd57LLV34fet/obA/RHu3YnSuXKNfG1Ph7IsDbHujYZVpnrh427i+eIFf7m2L/AOhH/BP+EDH+5ti/+hH/AAT/AIQI/wB1bD/7/wDpXf4AIl3bsSrTKlOnhbnx9qQGiXeWzRrR35+i3+loDRPvfa1Xpx8qT84wS/fA/JPvzGX4e33J/emo/MmB+Offl5/h7bCH3rjl80UB+K53xusqq3YxrS5Ppk38sqfIB+K53bvs9MuNteELcPpTYHz7u+bxd+vuWRTmo3HFfs0A+fcvXrzrduzuvxnJy+cDUAAAAAAAAAAAAAAAAAAAAAAAAZqwM9TAdXkA6gMdTAVfiBgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//Z";
	            }
	        });
	    },

		$scope.startSpin = function () {
		    $(".loading").toggleClass('ng-hide', false);
		    //usSpinnerService.spin('spinner-1');
		};

	    $scope.stopSpin = function () {
	        $(".loading").toggleClass('ng-hide', true);
	        //usSpinnerService.stop('spinner-1');
	    };
	});