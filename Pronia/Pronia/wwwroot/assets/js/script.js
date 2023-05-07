$(document).ready(function () {
    //get products by category on click category
    $(document).on("click", ".category", function (e) {
        e.preventDefault();
        let categoryId = $(this).attr("data-id");
        let parent = $(".product-grid-view")
        
        $.ajax({

            url: `shop/GetProductsByCategory?id=${categoryId}`,
            type: "Get",

            success: function (res) {
                
                $(parent).html(res)
            }
        })



    })


     //get all products 

    $(document).on("click", ".all-products", function (e) {
        e.preventDefault();
        let parent = $(".product-grid-view")

        $.ajax({

            url: "shop/GetAllProduct",
            type: "Get",

            success: function (res) {

                $(parent).html(res)
            }
        })



    })

 

    //get product by color
    $(document).on("click", ".color", function (e) {
        e.preventDefault();
        let colorId = $(this).attr("data-id");
        let parent = $(".product-grid-view")

        $.ajax({

            url: `shop/GetProductByColor?id=${colorId}`,
            type: "Get",

            success: function (res) {

                $(parent).html(res)
            }
        })



    })




    $(document).on("click", ".all-color", function (e) {
        e.preventDefault();
        let parent = $(".product-grid-view")

        $.ajax({

            url: "shop/GetAllProduct",
            type: "Get",

            success: function (res) {

                $(parent).html(res)
            }
        })



    }) 



    //filtered

    $(document).on("click", ".select", function (e) {
        e.preventDefault();
        console.log(this);
        let colorId = $(this).attr("data-id");
        let parent = $(".product-grid-view")

        $.ajax({

            url: `shop/GetProductByColor?id=${colorId}`,
            type: "Get",

            success: function (res) {

                $(parent).html(res)
            }
        })



    })
})