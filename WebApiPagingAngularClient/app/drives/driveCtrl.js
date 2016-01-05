(function () {
    'use strict';

    angular
        .module('app')
        .controller('driveCtrl', driveCtrl);

    driveCtrl.$inject = ['$scope', 'driveClubSvc'];

    function driveCtrl($scope ,driveClubSvc) {
        $scope.title = 'Select Drives';
        $scope.description = 'Select drives from a list.';

        $scope.drives = driveClubSvc.drives;
        $scope.loadDrives = loadDrives;
      //  $scope.info = driveClubSvc.paging.info;
 
        $scope.status = {
            type: "info",
            message: "ready",
            busy: false
        };

        activate();

        function activate() {
            //if this is the first activation of the controller load the first page
            if (driveClubSvc.paging.info.currentPage === 0) {
                driveClubSvc.load();
            }
         
        }

        function loadDrives() {
            $scope.status.busy = true;
            $scope.status.message = "loading records";

            driveClubSvc.load()
                            .then(function (result) {
                                $scope.status.message = "ready";
                            }, function (result) {
                                $scope.status.message = "something went wrong: " + (result.error || result.statusText);
                            })
                            ['finally'](function () {
                                $scope.status.busy = false;
                            });
        }

       
    }
})();
