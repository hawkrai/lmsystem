'use strict';

$('#fileupload').fileupload();

$('#fileupload').fileupload('option', {
	maxFileSize: 1000000000,
	resizeMaxWidth: 1920,
	resizeMaxHeight: 1200,
	autoUpload:true
});
