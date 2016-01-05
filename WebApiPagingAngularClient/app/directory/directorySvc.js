(function () {
    'use strict';

    angular
        .module('app')
        .factory('directorySvc', directorySvc);

    directorySvc.$inject = ['$q','$routeParams', 'directoryClientSvc'];

    function directorySvc($q,$routeParams, directoryClientSvc) {
        var initialOptions = {
            size: 20,
            name: $routeParams.dName,
            orderBy: "type",
            folderPath: $routeParams.dFolder,
            folderType: $routeParams.fType
          
        },
        service = {
            initialize: initialize,
            navigate: navigate,
            clear: clear,
            pages: [],
            paging: {
                options: angular.copy(initialOptions),
                info: {
                    name: $routeParams.dName,
                    totalItems: 0,
                    totalPages: 1,
                    currentPage: 0,
                    sortableProperties: [
                    "type",
                    "name"
                    ],
                    folderPath: $routeParams.dFolder
                }
            }                       
        };

        return service;
        
        function initialize() {
            var queryArgs = {
                name: $routeParams.dName,
                pageSize: service.paging.options.size,
                pageNumber: service.paging.info.currentPage,
                orderBy: service.paging.options.orderBy,
                folderPath: $routeParams.dFolder,
                folderType: $routeParams.fType
            };
            
            return directoryClientSvc.query(queryArgs).$promise.then(
                function (result) {
                    var newPage = {
                        number: pageNumber,
                        elements: []
                    };
                    result.elements.forEach(function (result) {
                        newPage.elements.push(result);
                    });

                    service.pages.push(newPage);
                    service.paging.info.name = $routeParams.dName;
                    service.paging.info.folderPath = $routeParams.dFolder;
                    service.paging.info.currentPage = 1;
                    service.paging.info.totalPages = result.totalPages;
                    service.paging.info.path = result.path;
                    service.paging.info.countFiles = result.countFiles;

                    return result.$promise;
                }, function (result) {
                    return $q.reject(result);
                });
        }
       
        function navigate(pageNumber) {
            var dfd = $q.defer();

            if (pageNumber > service.paging.info.totalPages) {               
                return dfd.reject({ error: "page number out of range" });
            }

            if (service.pages[pageNumber]) {
                service.paging.info.currentPage = pageNumber;
                service.paging.info.name = $routeParams.dName;
                service.paging.info.folderPath = $routeParams.dFolder;
                dfd.resolve();
            } else {
                return load(pageNumber);
            }

            return dfd.promise;
        }

        function load(pageNumber) {
            var queryArgs = {
                name: $routeParams.dName,
                pageSize: service.paging.options.size,
                pageNumber: pageNumber,
                orderBy: service.paging.options.orderBy,
                folderPath: $routeParams.dFolder,
                folderType: $routeParams.fType
            };
          
            return directoryClientSvc.query(queryArgs).$promise.then(
                function (result) {
                    var newPage = {

                        number: service.paging.info.pageNumber,
                        elements: []
                    };
                    result.elements.forEach(function (result) {
                        newPage.elements.push(result);
                    });
                
                    service.pages[pageNumber] = newPage;
                    service.paging.info.name = $routeParams.dName;
                    service.paging.info.folderPath = $routeParams.dFolder;
                    service.paging.info.currentPage = pageNumber;
                    service.paging.info.totalPages = result.totalPages;
                    service.paging.info.totalItems = result.totalItems;
                    service.paging.info.path = result.path;
                    service.paging.info.countFiles = result.countFiles;


                    return result.$promise;
                }, function (result) {
                    return $q.reject(result);
                });
        }

        function clear() {
            service.pages.length = 0;
            service.paging.info.name = $routeParams.dName;
            service.paging.info.totalItems = 0;
            service.paging.info.currentPage = 0;
            service.paging.info.totalPages = 1;
            service.paging.info.folderPath = $routeParams.dFolder;
          
        }
    }
})();