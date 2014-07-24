angular.module('SampleApp', ['ngRoute'])

.config(function ($routeProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: '/home/dashboard',
            controller: 'HomeCtrl'
        })
        .when('/person/:name', {
            templateUrl: '/home/person',
            controller: 'PersonCtrl'
        })
        .otherwise('/');
})

.service('SalesService', function ($http) {
    this.fetchSales = function () {
        return $http.get('/home/sales');
    }

    this.fetchMonthReport = function () {
        return $http.get('/home/monthreport');
    }

    this.fetchPersonReport = function (salesPerson) {
        return $http.get('/home/personreport?id=' + salesPerson);
    }
})

.controller('HomeCtrl', function ($scope, SalesService) {
    function init() {
        $scope.refreshSales();
    }

    $scope.refreshSales = function () {
        $scope.monthReportLoading = true;
        $scope.salesLoading = true;

        SalesService.fetchSales().then(function (result) {
            $scope.sales = result.data;
            $scope.salesLoading = false;
        }).catch(function (error) {
            alert('Sorry, there was an error loading sales.');
        });

        SalesService.fetchMonthReport().then(function (result) {
            $scope.monthReport = result.data;
            $scope.monthReportLoading = false;

            var ctx = document.getElementById("sales-by-month").getContext("2d");
            var myLineChart = new Chart(ctx).Line($scope.monthReport);
        
        }).catch(function (error) {
            alert('Sorry, there was an error loading the month report.');
        });
    }

    init();
})

.controller('PersonCtrl', function ($scope, $routeParams, SalesService) {
    function init() {
        $scope.salesperson = $routeParams.name;
        $scope.refreshSales();
    }

    $scope.refreshSales = function () {
        $scope.loading = true;

        SalesService.fetchPersonReport($scope.salesperson).then(function (result) {
            $scope.sales = result.data;
            $scope.loading = false;
        }).catch(function (error) {
            alert('Sorry, there was an error loading the month report.');
        });
    }

    init();
});