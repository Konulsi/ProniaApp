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



    //search

    $(document).on("submit", ".hm-searchbox", function (e) {
        e.preventDefault();
        let value = $(".input-search").val();
        let url = `/shop/mainsearch?searchText=${value}`;

        window.location.assign(url);
 
    })




    //SEARCH WITH li

    $(document).on("keyup", ".input-field", function () {
        debugger
        $("#search-list li").slice(1).remove();
        let value = $(".input-field").val();

        $.ajax({

            url: `shop/search?searchText=${value}`,

            type: "Get",

            success: function (res) {
                $("#search-list").append(res);
            }

        })

    })



    $(document).on("click", ".product-tag", function (e) {
        e.preventDefault();
     
        let tagId = $(this).attr("data-id");
        let parent = $(".product-grid-view")

        $.ajax({

            url: `shop/GetProductsByTag?id=${tagId}`,
            type: "Get",

            success: function (res) {

                $(parent).html(res)
            }
        })



    })



    ////get products by category on click category (About page)


    //$(document).on("click", ".category", function (e) {
    //    e.preventDefault();
    //    let categoryId = $(this).attr("data-id");
    //    let parent = $(".product-grid-view")

    //    $.ajax({

    //        url: `blog/GetProductsByCategory?id=${categoryId}`,
    //        type: "Get",

    //        success: function (res) {

    //            $(parent).html(res)
    //        }
    //    })



    //})

    ////get All products by category  (About page)


    //$(document).on("click", ".all-products", function (e) {
    //    e.preventDefault();
    //    let parent = $(".product-grid-view")

    //    $.ajax({

    //        url: "blog/GetAllProduct",
    //        type: "Get",

    //        success: function (res) {

    //            $(parent).html(res)
    //        }
    //    })



    //})



    //$(document).on("click", ".product-tag", function (e) {
    //    e.preventDefault();

    //    let tagId = $(this).attr("data-id");
    //    let parent = $(".product-grid-view")

    //    $.ajax({

    //        url: `blog/GetProductsByTag?id=${tagId}`,
    //        type: "Get",

    //        success: function (res) {

    //            $(parent).html(res)
    //        }
    //    })



    //})


})