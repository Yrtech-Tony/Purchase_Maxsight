var provinces, citys;
$(function () {    
    $("#Province").change(function () {
        loadCityByProvince($("#Province option:selected").attr("id"));
    })

    $.ajax({
        async: false,
        url: "/Base/GetProvinces",
        success: function (data) {
            provinces = data;
        }
    });
    $.ajax({
        async: false,
        url: "/Base/GetCitys",
        success: function (data) {
            citys = data;
        }
    });
    loadProvince();
})

function loadCityByProvince(pid) {
    $("#City").empty();
    $("#City").append($("<option>"));
    $.each(citys, function () {
        if (this.ProID == pid) {
            var option = $("<option>").val(this.CName).html(this.CName);
            $("#City").append(option);
        }
    })
    if ($("#City option").length == 2) {
        $("#City").val($("#City option:nth-child(2)").val());
    }
    $("#City").data("live-search", true).prop("title","").selectpicker("refresh");
}
function loadProvince() {
    $("#Province").empty();
    $("#Province").append($("<option>"));
    $.each(provinces, function () {
        var option = $("<option>").val(this.PName).html(this.PName).attr("id", this.ProID);
        $("#Province").append(option);
    })
}
