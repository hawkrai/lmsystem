'use strict';

$('#fileupload').fileupload();

$('#fileupload').fileupload('option', {
	maxFileSize: 100000000,
	resizeMaxWidth: 1920,
	resizeMaxHeight: 1200,
});
