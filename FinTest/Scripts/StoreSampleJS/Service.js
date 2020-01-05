var servapp = angular.module('LogicServices', [])
    .service('ProductsService', function ($http) {

        this.GetProducts = function () {
            return $http.get(window.location.protocol + "//" + window.location.host + "/" + '/api/products/');
        }

        this.AddToCart = function (model, cartItems) {           

          return $http({
                method: 'POST',
                url: window.location.protocol + "//" + window.location.host + "/" + '/api/cart/' + model,
                data: cartItems,
                headers: { 'Content-Type': 'application/json' },
            })
        }

        this.CheckOut = function (cart) {

            return $http({
                method: 'POST',
                url: window.location.protocol + "//" + window.location.host + "/" + '/home/checkout/',
                data: cart,
                headers: { 'Content-Type': 'application/json' },
            })
        }

    });