/*jslint nomen: true */
/*global $ */
var url = "/Controllers/FileUpload/AdvancedFileUploadHandler.ashx?FolderPath=xxx";

$(document).ready(function () {
	'use strict';

	// Initialize the jQuery File Upload widget:
	$('#fileupload').fileupload({
		url: url,
		//formData: [{ name: "Folder", value: "AfterActionReview/Gallery/5"}],
		//singleFileUploads : true,
		//limitMultiFileUploads:2,
		//maxNumberOfFiles:10,
		//maxFileSize: 5000000,
		acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
		previewAsCanvas: false
		//start: function (e) {alert('Uploads started');}
	});


	LoadFiles();
	

	// Open download dialogs via iframes, to prevent aborting current uploads:
	$('#fileupload .files a:not([target^=_blank])').live('click', function (e) {
		e.preventDefault();
		$('<iframe style="display:none;"></iframe>')
			.prop('src', this.href)
			.appendTo('body');
	});


	$('#fileupload').bind('fileuploaddone', function (e, data) {
		if (data.jqXHR.responseText || data.result) {
			var fu = $('#fileupload').data('fileupload');
			var JSONjQueryObject = (data.jqXHR.responseText) ? jQuery.parseJSON(data.jqXHR.responseText) : data.result;
			fu._adjustMaxNumberOfFiles(JSONjQueryObject.files.length);
			//debugger;
			fu._renderDownload(JSONjQueryObject.files)
                .appendTo($('#fileupload .files'))
                .fadeIn(function () {
                	$(this).show(); // Fix for IE7 and lower:
                });
		}
	});
});


var LoadFiles = function () {
	$.getJSON(url, function (files) {
		var fu = $('#fileupload').data('fileupload');
		fu._adjustMaxNumberOfFiles(-files.length);
		fu._renderDownload(files)
			.appendTo($('#fileupload .files'))
			.fadeIn(function () {
				$(this).show(); // Fix for IE7 and lower
			});
	});
};