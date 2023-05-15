$(document).ready(function () {


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


    //basket


    //addbasket from home page
    $(document).on("click", ".add-basket", function () {
        let productId = $(this).parent().attr("data-id")
        let data = { id: productId };
      
        $.ajax({
            url: `home/AddBasket`,
            type: "post",
            data: data,
            success: function (res) {

                $(".quantity").text(res);
             
            }
        })
        return false;
    })



    $(document).on("click", ".add-basket", function () {
        let productId = $(this).parent().attr("data-id")
        let data = { id: productId };

        $.ajax({
            url: `shop/AddBasket`,
            type: "post",
            data: data,
            success: function (res) {

                $(".quantity").text(res);
                
            }
        })
        return false;
    })



    //delete buton

    $(document).on("click", ".product_remove", function () {

        let deletProduct = $(this).parent();
        let productId = $(this).parent().attr("data-id")


        $.ajax({
            url: `shop/DeleteProductFromBasket?id=${productId}`,
            type: "Post",
            success: function (res) {
                res--
                $(".cart-count").text(res);


                $(deletProduct).remove();
              


            }
        })
        return false;
    })





})



