/***
* 冻结列/冻结表头 插件；使用时传入冻结列数和表格的高度
* 例如 { fixColumnIndex: 3 ,height：400}  默认是{ fixColumnIndex: 1 ,height：$(this).height()}
* 有横向滚动条，才会出现冻结列
* 有纵向滚动条，才会出现冻结表头
* 插件适合表格设置为自动根据内容扩展宽度的设置。固定宽带或百分百宽带可能样式会出现问题
*/
$.fn.fixHeadColumn = function (option) {
    option = option || {};
    var fixColumnIndex = option.fixColumnIndex||1;//冻结列下标
    var height = option.height || $(this).height();//表格高度

    var hasHorScroll = true;
    var hasVerScroll = true;
    try
    {
        if (this.get(0).scrollWidth <= this.get(0).clientWidth) {
            hasHorScroll = false;
        }
        if (this.get(0).scrollHeight <= this.get(0).clientHeight) {
            hasVerScroll = false;
        }
    }catch(ex){};
    

    $(this).show();
    var parent = $(this).parent();
    $("#fix-layout", parent).remove();
    var layout = $("<div id='fix-layout' style='position:relative'>");

    var divBody = $(this).clone().attr("id", null).removeClass("fix-table-div").addClass("fixed-table-body");    
    var bodyTHead = divBody.find("thead");
    var bodyTHeadClone = bodyTHead.clone();
    var scrollBarWidth = 20;//divBody[0].offsetWidth - divBody[0].clientWidth;
    var lastTh = $("<th>").html("&nbsp;").css("min-width", scrollBarWidth).css("width", scrollBarWidth);
    bodyTHeadClone.find("tr").append(lastTh);
    var divHead = $("<div class='table-responsive fixed-table-header'>");
    divHead.append($("<table class='table table-bordered table-condensed table-hover'>").append(bodyTHeadClone));
    divHead.find("table").attr("id", divBody.find("table").attr("id") + "-head");
   
    layout.prependTo(parent);
    layout.append(divBody);
    
    divBody.find("table").setHeadSameWidth(divHead.find("table"));
    $(this).hide();
    $(this).find("table tbody").empty();
    
    if (hasHorScroll) {
        var fixedHeaderColumn;
        var fixedBodyColumn;
        if (fixColumnIndex > 0) {
            //增加冻结列
            //冻结列头
            fixedHeaderColumn = $("<div class='fixed-table-header-columns'>");
            fixedHeaderColumn.append(divHead.find("table").clone());
            fixedHeaderColumn.find("thead tr th").each(function (i) {
                if (i >= fixColumnIndex) {
                    $(this).remove();
                }
            })
            //冻结内容
            fixedBodyColumn = $("<div class='fixed-table-body-columns'>").height(height - 17);
            fixedBodyColumn.append(divBody.find("table").clone());
            fixedBodyColumn.find("thead").remove();
            fixedBodyColumn.find("table").prepend(fixedHeaderColumn.find("table thead").clone()).attr("id", "");
            var bodyTrs = divBody.find("tbody tr");
            fixedBodyColumn.find("tbody tr").each(function (j) {
                //set height
                $(this).height(bodyTrs.eq(j).outerHeight(true));

                var tds = $("td", $(this));
                var i = 0;
                tds.each(function () {
                    if (i >= fixColumnIndex || $(this).css('display') == "none") {
                        $(this).remove();
                    }
                    if ($(this).css('display') != "none") {
                        i++;
                    }
                });
            })
        }
    }
    layout.prepend(fixedBodyColumn);
    if (hasVerScroll) {
        layout.prepend(divHead);
    }
    layout.prepend(fixedHeaderColumn);
    layout.css("margin-bottom", -divHead.outerHeight(true));
    if (hasVerScroll) {
        layout.css("padding-right", 17);
        divBody.css("width", divBody.width()+19);
    }    
    divBody.css("top", -divHead.outerHeight(true));

    if (fixedBodyColumn)
        fixedBodyColumn.width(fixedBodyColumn.find("table").outerWidth(true));
    //同步scroll
    divBody.scroll(function () {
        divHead.scrollLeft($(this).scrollLeft());
        if (fixedBodyColumn)
            fixedBodyColumn.scrollTop($(this).scrollTop());
    })
}

$.fn.setHeadSameWidth = function (target) {
    var source = $(this);
    var thsTar = $(target).find("thead th");
    source.find("thead th").each(function (i, th) {
        var width = $(th).outerWidth(true);
        thsTar.eq(i).css("min-width", width).css("width", width);;
    });
}

$.fn.clearFixLayout = function () {
    $("#fix-layout").remove();
    $(this).show();
}