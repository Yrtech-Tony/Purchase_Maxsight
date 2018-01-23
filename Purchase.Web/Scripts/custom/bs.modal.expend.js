function createBSWindow(config) {
    
	var window = config.window || "windowModal";
	var modalType = config.modalType || "";
	var windowEl = $('#' + window);
	
	if (windowEl.length == 0) {
		$('body').append('<div id="' + window + '" class="modal fade"><div class="modal-dialog '+modalType+'"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button><h4 class="modal-title"></h4></div><div class="modal-body"></div></div></div></div>');
		windowEl = $('#' + window);
		windowEl.modal({ backdrop: 'static', keyboard: false });
	}
	if (config) {
		if (config.title) {
		    $('.modal-title', windowEl).html(config.title);
		}
		if (config.innerHtml) {
		    $('.modal-body', windowEl).html(config.innerHtml);
		}
		if (config.create) {
		    $('.modal-body', windowEl).html("");
		}
	}
	
	windowEl.modal('show');
	
	return windowEl;
}