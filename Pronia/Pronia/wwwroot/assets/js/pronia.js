$(document).ready(function () {

    $(document).on("click", ".category", function (e) {

        e.preventDefault();
        let categoryId = $(this).attr("data-id");
        let parent = $(".product-grid-view")
        $.ajax({

            url: `shop/GetProductsByCategory?id=${categoryId}`,
            type: "Get",

            success: function (res) {
                console.log(parent)
                $(parent).append(res)
            }
        })



    })
})