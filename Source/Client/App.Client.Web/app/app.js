﻿/*********************
 * Application Module
 *********************/
(function () {
    'use strict';

    angular.module('app', ['ngRoute', 'ngCookies', 'app.controllers', 'app.directives'])
        .config(config)
        .run(run)
        .value('jQuery', jQuery)
        .value('toastr', toastr)
        .constant('appSettings', settings());

    config.$inject = ['$locationProvider', '$routeProvider'];
    run.$inject = ['$rootScope', '$location'];

    angular.module('templates', []);
    angular.module('app.data', []);
    angular.module('app.services', []);
    angular.module('app.controllers', ['app.data']);
    angular.module('app.directives', []);

    function config($locationProvider, $routeProvider) {
        var CONTROLLER_VIEW_MODEL = 'vm';

        $locationProvider.html5Mode(true).hashPrefix('!');

        $routeProvider
            .when('/', {
                templateUrl: '/app/home/home.html',
                controller: 'HomeCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Home'
            })
            .when('/entity/all', {
                templateUrl: '/app/entity/entities.html',
                controller: 'EntityCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'All Entities'
            })
            .when('/entity/:id', {
                templateUrl: '/app/entity/entity.html',
                controller: 'EntityCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Entity'
            })
            .when('/entity/add', {
                templateUrl: '/app/entity/add-entity.html',
                controller: 'EntityCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Add Entity'
            })
            .when('/entity/update', {
                templateUrl: '/app/entity/update-entity.html',
                controller: 'EntityCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Update Entity'
            })
            .otherwise({
                redirectTo: '404-not-found',
                templateUrl: '/app/common/views/not-found.html',
                title: 'Page Not Found'
            });
    }

    function run($rootScope, $location) {
        $rootScope.$on('$routeChangeSuccess', function routeChangeSuccess(event, current, previous) {
            if (current.hasOwnProperty('$$route')) {
                $rootScope.title = current.$$route.title;
            }
        });

        $rootScope.$on('$routeChangeStart', function routeChangeSuccess(event, current, previous) {
            // TODO: Run on route start
        });
    }

    function settings() {
        return {
            serverPath: 'https://dev.app.com/api/',
            appName: 'StartApp',
        };
    }

}());