// Write your Javascript code.
$(function () {
    $(".badge.more").hover(function (e) {
        toggleTable(this);
    }, function (e) {
        toggleTable(this);
    });
});

function toggleTable(el) {
    var $container = $(el).closest(".appartment-container");
    var $table = $("table", $container);
    $table.toggle();
}