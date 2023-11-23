// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.close-alert').click(function () {
    $('.alert').hide('hide');;
})

function calcularMedia() {
    var np1 = parseFloat(document.getElementById("np1").value) || 0;
    var np2 = parseFloat(document.getElementById("np2").value) || 0;
    var np3 = parseFloat(document.getElementById("np3").value) || 0;
    var np4 = parseFloat(document.getElementById("np4").value) || 0;

    var mediaFinal = (np1 + np2 + np3 + np4) / 4;
    document.getElementById("mediaFInal").innerText = mediaFinal.toFixed(2);
}
