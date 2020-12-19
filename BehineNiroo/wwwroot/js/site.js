// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
$(document).ready(function () {
    $("#EventTable #checkall").click(function () {
        if ($("#EventTable #checkall").is(':checked')) {
            $("#EventTable input[type=checkbox]").each(function () {
                $(this).prop("checked", true);
            });

        } else {
            $("#EventTable input[type=checkbox]").each(function () {
                $(this).prop("checked", false);
            });
        }
    });

    $("[data-toggle=tooltip]").tooltip();
});

$(document).ready(function () {
    $("#EnergyTable #checkall").click(function () {
        if ($("#EnergyTable #checkall").is(':checked')) {
            $("#EnergyTable input[type=checkbox]").each(function () {
                $(this).prop("checked", true);
            });

        } else {
            $("#EnergyTable input[type=checkbox]").each(function () {
                $(this).prop("checked", false);
            });
        }
    });

    $("[data-toggle=tooltip]").tooltip();

});

