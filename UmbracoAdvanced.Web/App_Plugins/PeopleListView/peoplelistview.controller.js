(function () {
    "use strict";

    function peoplelistviewController($scope, $location, listViewHelper) {
        $scope.toggleItem = function (item) {
            if (item.selected) {
                listViewHelper.deselectItem(item, $scope.selection);
            } else {
                listViewHelper.selectItem(item, $scope.selection);
            }
        }
        $scope.gotoItem = function(item) {
            $location.url(item.editPath);
        }
    }

    angular.module("umbraco").controller("peoplelistviewController", peoplelistviewController);
})();