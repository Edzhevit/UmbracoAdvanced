function contactRequestsService(contactRequestsResource) {
    return {
        getTotalNumber: function () {
            return contactRequestsResource.getTotalNumber().then(function (data) {
                return data;
            }, function () { });
        },
        getAll: function () {
            return contactRequestsResource.getAll().then(function (data) {
                return data;
            }, function () { });
        }
    }
}

angular.module("umbraco").factory("contactRequestsService", contactRequestsService);