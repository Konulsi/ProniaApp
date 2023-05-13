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




    //basket



    $(document).on("click", ".add-basket", function () {  

        debugger
        let productId = $(this).parent().attr("data-id")  

        let data = { id: productId };


        $.ajax({
            url: `home/AddBasket`,     
            type: "post",
            data: data,                 
            success: function (res) {

                $(".cart-count").text(res);

                swal("Product added to basket", "", "success");  

            }
        })
        return false;
    })





       //delete buton

    $(document).on("click", ".delete-btn", function () {   


        let deletProduct = $(this).parent().parent();                 

        let productId = $(this).attr("data-id")                        

        let sum = 0;

        let grandTotal = $(".total-product").children().eq(0);         

        $.ajax({
            url: `card/DeleteProductFromBasket?id=${productId}`,      
            type: "Post",
            success: function (res) {
                res--
                $(".cart-count").text(res);


                $(deletProduct).remove();                            
                for (const product of $(".table-product").children()) {     
                    let total = parseFloat($(product).children().eq(6).text())   
                    sum += total    
                }


                $(grandTotal).text(sum);   


                swal("Product deleted to basket", "", "success");   


                if ($(".table-product").children().length == 0) {     
                    $("table").addClass("d-none");                      
                    $(".total-product").addClass("d-none");            
                    $(".alert-product").removeClass("d-none");            
                }


            }
        })
        return false;
    })





    $(document).on("click", ".minus", function () {

        let productId = $(this).parent().parent().attr("data-id");    

        let input = $(this).next()                                   

        let count = parseInt($(input).val()) - 1;                   

        let nativePrice = parseFloat($(this).parent().prev().text())   

        let total = $(this).parent().next().children().eq(0);          

        let sum = 0;

        let grandTotal = $(".total-product").children().eq(0);          


        if (count > 0) {                                               
            $(input).val(count);                                      
            $.ajax({
                url: `card/DecreasetProductCount?id=${productId}`,      
                type: "Post",
                success: function (res) {
                    let productCount = res;                               
                    let subtotal = nativePrice * productCount             
                    total.text(subtotal + ".00")                        
                    for (const product of $(".table-product").children()) {     
                        let total = parseFloat($(product).children().eq(6).text())   
                        sum += total                                                
                    }
                    $(grandTotal).text(sum + ".00");                           

                }
            })
        }

    })




    $(document).on("click", ".plus", function () {

        let productId = $(this).parent().parent().attr("data-id");            

        let input = $(this).prev()                                           

        let count = parseInt($(input).val()) + 1;                     

        $(input).val(count);                                              

        let nativePrice = parseFloat($(this).parent().prev().text())            

        let total = $(this).parent().next().children().eq(0);                   

        let sum = 0;

        let grandTotal = $(".total-product").children().eq(0);                  


        $.ajax({
            url: `card/IncreaseProductCount?id=${productId}`,                   
            type: "Post",
            success: function (res) {
                let countProduct = res;                                          
                let subtotal = nativePrice * countProduct                        
                total.text(subtotal + ".00")                                       
                for (const product of $(".table-product").children()) {           
                    let total = parseFloat($(product).children().eq(6).text())   
                    sum += total                                                
                }
                $(grandTotal).text(sum + ".00");                                
            }
        })
        return false;
    })

})