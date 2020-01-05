var app = angular.module('TestApp', ['LogicServices', 'ngRoute']);

app.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
   
    $routeProvider.when('/', {
        templateUrl: 'SPAViews/main.html',
        controller: 'MainController',
    });
    $routeProvider.when('/checkout', {
        templateUrl: 'SPAViews/checkout.html',
        controller: 'CheckoutController',
    });  
    $routeProvider.otherwise({
        redirectTo: '/'
    });

    $locationProvider.html5Mode(false).hashPrefix('!');
}]);


app.controller('MainController', function ($scope, $http, ProductsService, $location, productsExchangeService) {
    
    $scope.products;
   
    ProductsService.GetProducts().then(function (data) {    
        $scope.products = data.data;       
    })
    .catch(function (data) {
        alert(data);
    });

    $scope.CheckOut = function (event) {       
        productsExchangeService.addProduct($scope.products);
        $location.path('/checkout');        
    }

    $scope.AddItem = function (event) {
      
        ProductsService.AddToCart(event.target.id, $scope.products.Cart.CartItems).then(function (data) {
            $scope.products = data.data;            
        })
        .catch(function (data) {
            alert(data);
        });
    }
   
});



app.controller('CheckoutController', function ($scope, $http, ProductsService, productsExchangeService) {    
   
    $scope.products = productsExchangeService.getProducts();

    $scope.ProceedCheckout = function (event) {
        // Proceeding to check out logic
    }
});


app.service('productsExchangeService', function () {
    var products;

    var addProduct = function (newObj) {
        products = newObj;
    };

    var getProducts = function () {
        return products;
    };

    return {
        addProduct: addProduct,
        getProducts: getProducts
    };

});
