(function () {
    'use strict';

    angular
        .module('app')
        .controller('directoryCtrl', directoryCtrl);

    directoryCtrl.$inject = ['$scope', '$sce', 'directorySvc'];

    function directoryCtrl($scope, $sce, directorySvc) {
        $scope.title = 'directory';
        $scope.description = 'Select to see count of files  in current directory and subdirectories';
        $scope.sce = $sce;
        $scope.pages = directorySvc.pages;
        $scope.info = directorySvc.paging.info;
        $scope.elements = directorySvc.pages.elements;
        $scope.options = directorySvc.paging.options;
        $scope.navigate = navigate;
        $scope.clear = optionsChanged;

        $scope.getTypes = getType;

        $scope.status = {
            type: "info",
            message: "ready",
            busy: false
        };
        optionsChanged();
        //activate();

        function activate() {
            //if this is the first activation of the controller load the first page
            if (directorySvc.paging.info.currentPage === 0) {
                navigate(1);
            }

        }

        function navigate(pageNumber) {
            $scope.status.busy = true;
            $scope.status.message = "loading records";

            directorySvc.navigate(pageNumber)
                            .then(function () {
                                $scope.status.message = "ready";
                            }, function (result) {
                                $scope.status.message = "something went wrong: " + (result.error || result.statusText);
                            })
                            ['finally'](function () {
                                $scope.status.busy = false;
                            });
        }

        function optionsChanged() {
            directorySvc.clear();
            activate();
        }

        function getType(type, name, drive, folder) {
            var str='';
            if (folder) {
                str = folder + '-!';
            }
            if (type == "File")
                return name;
            else
                return '<a href="#/driveManager/' + drive + '/' + str + name + '">' + name + '\\</a>';
        }
    }

})();