(function() {
    'use strict';

    angular
        .module('app')
        .directive('pager', pager);

    pager.$inject = ['$window','directorySvc'];
    
    function pager($window, directorySvc) {
        var directive = {
            link: link,
            restrict: 'E',
            replace: true,
            templateUrl: "app/directory/pager.html",
            scope: {                
                totalPages: "=",
                currentPage: "=",
                pageAction: "&"
            }
        };

        return directive;

        function link(scope, element, attrs) {
            scope.pages = [];
            scope.$watch('totalPages', function () {
                createPageArray(scope.pages, scope.totalPages);
            });
            scope.gotoPage = function (p) {
                scope.pageAction({pageNumber: p});
            };
        }

        function createPageArray(pages, totalPages) {
            var i;
            pages.length = 0;

            for (i = 1; i <= totalPages; i++) {
                pages.push(i);
            }
        }
    }

})();