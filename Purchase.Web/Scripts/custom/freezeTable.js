/*
 * 锁定表头和列
 * 
 * 参数定义
 *   table - 要锁定的表格元素或者表格ID
 *   freezeRowNum - 要锁定的前几行行数，如果行不锁定，则设置为0
 *   freezeColumnNum - 要锁定的前几列列数，如果列不锁定，则设置为0
 *   width - 表格的滚动区域宽度
 *   height - 表格的滚动区域高度
 */
function freezeTable(table, freezeRowNum, freezeColumnNum, width, height) {

    //获取冻结行数或者列数
    if (typeof (freezeRowNum) == 'string')
        freezeRowNum = parseInt(freezeRowNum)

    if (typeof (freezeColumnNum) == 'string')
        freezeColumnNum = parseInt(freezeColumnNum)
    //获取table
    var tableId;
    if (typeof (table) == 'string') {
        tableId = table;
        table = $('#' + tableId);
    } else
        tableId = table.attr('id');

    /**
     * 生成最外层的DIV用来承载内部的四个DIV
     */
    var divTableLayout = $("#" + tableId + "_tableLayout");

    if (divTableLayout.length != 0) {
        divTableLayout.before(table);
        divTableLayout.empty();
    } else {
        table.after("<div id='" + tableId + "_tableLayout' style='overflow:hidden;height:" + height + "px; width:" + width + "px;'></div>");

        divTableLayout = $("#" + tableId + "_tableLayout");
    }

    /**
     * 根据需要页面table定义的属性 需要冻结的行或者列 ，对应的拼接字符串用于生成不同的DIV  【如果行列同时冻结：生成总共四个DIV】【单独冻结行或列，仅需要生成两个DIV】
     * 这个四个DIV都是包括数据在内，完全克隆了原本的table，
     */
    var html = '';
    if (freezeRowNum > 0 && freezeColumnNum > 0)
        html += '<div id="' + tableId + '_tableFix" style="padding: 0px;"></div>';

    if (freezeRowNum > 0)
        html += '<div id="' + tableId + '_tableHead" style="padding: 0px;"></div>';

    if (freezeColumnNum > 0)
        html += '<div id="' + tableId + '_tableColumn" style="padding: 0px;"></div>';

    html += '<div id="' + tableId + '_tableData" style="padding: 0px;"></div>';

    //将div追加到页面
    $(html).appendTo("#" + tableId + "_tableLayout");

    var divTableFix = freezeRowNum > 0 && freezeColumnNum > 0 ? $("#" + tableId + "_tableFix") : null;
    var divTableHead = freezeRowNum > 0 ? $("#" + tableId + "_tableHead") : null;
    var divTableColumn = freezeColumnNum > 0 ? $("#" + tableId + "_tableColumn") : null;
    var divTableData = $("#" + tableId + "_tableData"); //位于最底层的【数据DIV】【第一个DIV，也就是原本的那个真身】

    divTableData.append(table);
    //行列同时冻结生成的【行列DIV】【第二个DIV】，一般位于左上角重叠部分
    if (divTableFix != null) {
        var tableFixClone = table.clone(true);    //克隆1
        tableFixClone.attr("id", tableId + "_tableFixClone");
        divTableFix.append(tableFixClone);
    }
    //行冻结生成的【冻结行DIV】【第三个DIV】
    if (divTableHead != null) {
        var tableHeadClone = table.clone(true);//克隆2
        tableHeadClone.attr("id", tableId + "_tableHeadClone");
        divTableHead.append(tableHeadClone);
    }
    //列冻结生成的【冻结列DIV】【第四个DIV】
    if (divTableColumn != null) {
        var tableColumnClone = table.clone(true);//克隆3
        tableColumnClone.attr("id", tableId + "_tableColumnClone");
        divTableColumn.append(tableColumnClone);
    }

    $("#" + tableId + "_tableLayout table").css("margin", "0");

    /**
     * 根据冻结行数，设置行冻结的div的高度【如果行列同时冻结，则行列div也设置对应高度】
     */
    if (freezeRowNum > 0) {
        var HeadHeight = 0;
        var ignoreRowNum = 0;
        $("#" + tableId + "_tableHead tr:lt(" + freezeRowNum + ")").each(function () {
            if (ignoreRowNum > 0)
                ignoreRowNum--;
            else {
                var td = $(this).find('td:first, th:first');
                HeadHeight += td.outerHeight(true);

                ignoreRowNum = td.attr('rowSpan');
                if (typeof (ignoreRowNum) == 'undefined')
                    ignoreRowNum = 0;
                else
                    ignoreRowNum = parseInt(ignoreRowNum) - 1;
            }
        });
        HeadHeight += 2;

        divTableHead.css("height", HeadHeight);
        divTableFix != null && divTableFix.css("height", HeadHeight);
    }

    /**
     * 根据冻结列数，对冻结列DIV设置宽度【如果行列同时冻结，则行列div也设置对应宽度】
     */
    if (freezeColumnNum > 0) {
        var ColumnsWidth = 0;
        var ColumnsNumber = 0;
        $("#" + tableId + "_tableColumn tr:eq(" + freezeRowNum + ")").find("td:lt(" + freezeColumnNum + "), th:lt(" + freezeColumnNum + ")").each(function () {
            if (ColumnsNumber >= freezeColumnNum)
                return;

            ColumnsWidth += $(this).outerWidth(true);

            ColumnsNumber += $(this).attr('colSpan') ? parseInt($(this).attr('colSpan')) : 1;
        });
        ColumnsWidth += 2;

        divTableColumn.css("width", ColumnsWidth);
        divTableFix != null && divTableFix.css("width", ColumnsWidth);
    }

    //滚动
    divTableData.scroll(function () {
        divTableHead != null && divTableHead.scrollLeft(divTableData.scrollLeft());

        divTableColumn != null && divTableColumn.scrollTop(divTableData.scrollTop());
    });

    divTableFix != null && divTableFix.css({ "overflow": "hidden", "position": "absolute", "z-index": "50" });
    divTableHead != null && divTableHead.css({ "overflow": "hidden", "width": width - 17, "position": "absolute", "z-index": "45" });
    divTableColumn != null && divTableColumn.css({ "overflow": "hidden", "height": height - 17, "position": "absolute", "z-index": "40" });
    divTableData.css({ "overflow": "scroll", "width": width, "height": height, "position": "absolute" });

    divTableFix != null && divTableFix.offset(divTableLayout.offset());
    divTableHead != null && divTableHead.offset(divTableLayout.offset());
    divTableColumn != null && divTableColumn.offset(divTableLayout.offset());
    divTableData.offset(divTableLayout.offset());
}

/*
 * 调整锁定表的宽度和高度，这个函数在resize事件中调用
 * 
 * 参数定义
 *   table - 要锁定的表格元素或者表格ID
 *   width - 表格的滚动区域宽度
 *   height - 表格的滚动区域高度
 */
function adjustTableSize(table, width, height) {
    var tableId;
    if (typeof (table) == 'string')
        tableId = table;
    else
        tableId = table.attr('id');

    $("#" + tableId + "_tableLayout").width(width).height(height);
    $("#" + tableId + "_tableHead").width(width - 17);
    $("#" + tableId + "_tableColumn").height(height - 17);
    $("#" + tableId + "_tableData").width(width).height(height);
}

//返回页面的高度
function pageHeight() {
    if (/msie/.test(navigator.userAgent.toLowerCase())) {
        return document.compatMode == "CSS1Compat" ? document.documentElement.clientHeight : document.body.clientHeight;
    } else {
        return self.innerHeight;
    }
};

//返回当前页面宽度
function pageWidth() {
    if (/msie/.test(navigator.userAgent.toLowerCase())) {
        return document.compatMode == "CSS1Compat" ? document.documentElement.clientWidth : document.body.clientWidth;
    } else {
        return self.innerWidth;
    }
};