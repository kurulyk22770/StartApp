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
    run.$inject = ['$rootScope', '$location', 'identity'];

    angular.module('templates', []);
    angular.module('app.data', []);
    angular.module('app.services', []);
    angular.module('app.controllers', ['app.data', 'app.services']);
    angular.module('app.directives', []);

    function config($locationProvider, $routeProvider) {
        var CONTROLLER_VIEW_MODEL = 'vm';

        $locationProvider.html5Mode(true).hashPrefix('!');

        $routeProvider
            // HOME
            .when('/', {
                templateUrl: '/app/home/home.html',
                controller: 'HomeCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Home'
            })

            // ENTITY
            .when('/entity/get/:id', {
                templateUrl: '/app/entity/entity.html',
                controller: 'EntityCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Entity'
            })
            .when('/entity/add', {
                templateUrl: '/app/entity/add-entity.html',
                controller: 'AddEntityCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Add Entity'
            })
            .when('/entity/edit/:id', {
                templateUrl: '/app/entity/edit-entity.html',
                controller: 'EditEntityCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Edit Entity'
            })

            // USER
            .when('/user/login', {
                templateUrl: '/app/user/login.html',
                controller: 'LoginCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Login'
            })
            .when('/user/register', {
                templateUrl: '/app/user/register.html',
                controller: 'RegisterCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'Register'
            })
            .when('/user/profile/:id', {
                templateUrl: '/app/user/profile.html',
                controller: 'ProfileCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'User Profile',
                secure: true
            })
            .when('/user/list', {
                templateUrl: '/app/user/user-list.html',
                controller: 'UserListCtrl',
                controllerAs: CONTROLLER_VIEW_MODEL,
                title: 'All Users'
            })

            // 404
            .otherwise({
                redirectTo: '404-not-found',
                templateUrl: '/app/common/views/not-found.html',
                title: 'Page Not Found'
            });
    }

    function run($rootScope, $location, identity) {
        $rootScope.$on('$routeChangeStart', function routeChangeSuccess(event, next, previous) {
            if (next && next.$$route && next.$$route.secure) {
                if (!identity.isAuthenticated()) {
                    $location.path('/user/login');
                }
            }
        });

        $rootScope.$on('$routeChangeSuccess', function routeChangeSuccess(event, current, previous) {
            if (current.hasOwnProperty('$$route')) {
                $rootScope.title = current.$$route.title;
            }
        });

        $rootScope.$on('$routeChangeError', function routeChangeError(event, current, previous) {
            if (current.hasOwnProperty('$$route')) {
                // TODO: Implement
            }
        });
    }

    function settings() {
        return {
            serverPath: 'http://localhost:3000/api/',
            appName: 'StartApp'
        };
    }

}());
