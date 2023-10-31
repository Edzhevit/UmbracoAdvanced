function contactRequestsResource($http, umbRequestHelper) {
    return {
        getTotalNumber: function () {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/api/ContactRequestsApi/GetTotalNumber"),
                "Failed to retrieve the total number"
            );
        },
        getAll: function () {
            return umbRequestHelper.resourcePromise(
                $http.get("/umbraco/backoffice/api/ContactRequestsApi/GetAll"),
                "Failed to retrieve the contacts requests"
            );
        }
    }
}

angular.module("umbraco").factory("contactRequestsResource", contactRequestsResource);