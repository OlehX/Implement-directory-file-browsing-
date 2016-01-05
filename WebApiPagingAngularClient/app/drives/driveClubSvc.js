(function () {
    'use strict';

    angular
        .module('app')
        .factory('driveClubSvc', driveClubSvc);

    driveClubSvc.$inject = ['$q', 'driveClientSvc'];

    function driveClubSvc($q, driveClientSvc) {
        var initialOptions = {
        }, service = {
            load: load,
            drives: [],
            paging: {
                options: angular.copy(initialOptions),
                info: {
                    currentPage: 0    
                }
            }
        };

        return service;

        function load() {
            service.paging.info.currentPage += 1;

            var queryArgs = {
               
            };

            return driveClientSvc.query(queryArgs).$promise.then(
                function (result) {
                    result.drives.forEach(function (result) {
                        service.drives.push(result);
                    });
                    return result.$promise;
                });
        }
    }
})();