// Filter table
$(document).ready(function(){
    $("#searchBtn").on("click", function() {
    var value = $("#inputData").val().toLowerCase();
        $("#tbodyData tr").filter(function() {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});

$('tr[data-href]').on("click", function() {
    document.location = $(this).data('href');
});