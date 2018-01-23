
function MulUploader(config) {
    var $list = $(".uploader-list");
    var maxSize = 40;//M
    var uploader = this.uploader = WebUploader.create({
        // swf文件路径
        swf: '/Scripts/upload/Uploader.swf',
        // 文件接收服务端。
        server: config.url,
        disableGlobalDnd:true,
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: {
            id: config.el,
            multiple:true
        },
        auto: true,
        threads: 1,
        fileSingleSizeLimit: maxSize * 1024 * 1024,
        duplicate: true,
        formData: config.formData
    });
    this.upload = function () {
        uploader.upload();
    };
    
    // 当有文件被添加进队列的时候
    this.uploader.on('fileQueued', function (file) {
        var tr = $("<tr id='" + file.id + "'>");
        tr.append($("<td>").text(file.name));
        tr.append($("<td>").html("<p class='state'></p>"));
        $list.show();
        $list.find("table tbody").append(tr);
    });
    // 文件上传过程中创建进度条实时显示。
    this.uploader.on('uploadProgress', function (file, percentage) {
        //var $li = $('#' + file.id),
        //    $percent = $li.find('.progress .progress-bar');

        //// 避免重复创建
        //if (!$percent.length) {
        //    $percent = $('<div class="progress progress-striped active">' +
        //      '<div class="progress-bar" role="progressbar" style="width: 0%">' +
        //      '</div>' +
        //    '</div>').appendTo($li).find('.progress-bar');
        //}
        
        //$percent.css('width', percentage * 100 + '%');
    });
    this.uploader.on("error", function (type) {
        if (type == "F_EXCEED_SIZE") {
            alert("文件大小不能超过"+maxSize+"M");
        }
    });
    this.uploader.on('uploadError', function (file, reason) {
        $('#' + file.id, $list).remove();
        uploader.cancelFile(file);
    });

    this.uploader.on('uploadComplete', function (file) {
        uploader.cancelFile(file);      
    });
    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    this.uploader.on('uploadFinished', function () {
        if (config && config.uploadCallBack)
            config.uploadCallBack();
    });
    
    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    this.uploader.on('uploadSuccess', function (file, retData) {
        if (config && config.uploadSuccess) {
            config.uploadSuccess(retData);
        }
        var del = $("<a>删除</a>").css("cursor", "pointer");
        del.click(function () {
            $('#' + file.id, $list).remove();
            if(config && config.fileDelete){
                config.fileDelete(retData);
            }
        });
        $('#' + file.id, $list).find('td:last-child').append(del);
    });
}