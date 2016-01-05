(function () {
    'use strict';

    angular
        .module('app')
        .factory('driveClientSvc', function ($resource) {
            return $resource("api/drives/:name/",
                { name: "@name" },
                {
                    'query': {
                        method: 'GET',
                        url: '/api/drives/:name/:pageSize/:pageNumber/:folderPath/:orderBy',
                        params: { name: '@name', pageSize: '@pageSize', pageNumber: '@pageNumber',folderPath:'@folderPath', orderBy: '@orderBy' }
                     
                    }
                });
        });
})();
