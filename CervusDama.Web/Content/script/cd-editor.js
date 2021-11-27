var selectedText = '';
var selection;
var image = '';
var galleryContent;
var idealColumnWidth;

document.addEventListener('DOMContentLoaded', function () {
	document.execCommand("defaultParagraphSeparator", false, 'p');

	var editorSwap = document.getElementById('cd-editor-swap');

	//editor buttons
	document.querySelectorAll('a[data-proc]').forEach(function (item) {
		item.addEventListener('click', function (e) {
			var action = '';
			var active = '';
			var element;
			if (e.target.tagName === 'I') {
				element = e.target.parentElement;
				action = element.getAttribute('data-proc');
				active = element.getAttribute('data-active');
			} else {
				element = e.target;
				action = element.getAttribute('data-proc');
				active = element.getAttribute('data-active');
			}

			if (active === 'true' && document.queryCommandState('bold')) {
				element.classList.toggle('active');
			}

			switch (action) {
				case 'heading':
					document.execCommand('formatBlock', false, '<h2>');
					break;

				case 'bold':
					document.execCommand('bold', false, null);
					break;

				case 'italic':
					document.execCommand('italic', false, null);
					break;

				case 'underline':
					document.execCommand('underline', false, null);
					break;

				case 'ul':
					document.execCommand('insertHTML', false, '<ul><li></li></ul>');
					break;

				case 'ol':
					document.execCommand('insertHTML', false, '<ol><li></li></ol>');
					break;

				case 'quote':
					document.execCommand('formatBlock', false, 'blockquote');
					break;

				default:
					return false;
					break;
			}
		});
	});

	document.querySelectorAll('a[data-action]').forEach(function (item) {
		item.addEventListener('click', function (e) {
			var action = '';
			if (e.target.tagName === 'I') {
				action = e.target.parentElement.getAttribute('data-modal');
			} else {
				action = e.target.getAttribute('data-modal');
			}

			var modal = document.getElementById(action);
			modal.style.display = 'block';
		});
	});

	//editor modals
	document.getElementById('insert-code-btn').addEventListener('click', function () {

		var html = document.querySelector('textarea[name="code"]').value;
		var lang = document.querySelector('select[name="language"]').value;

		html = html.replace(/</gi, '&lt;');
		html = html.replace(/>/gi, '&gt;');

		html = '<pre class="' + lang.trim() + '"><code class="' + lang.trim() + '">' + html + '</code></pre>';

		restoreSelection(selection);

		document.execCommand('insertHTML', null, html);

		Prism.highlightAll();

		closeModal();
	});

	document.getElementById('insert-link-btn').addEventListener('click', function () {
		var url = document.querySelector('input[name="link-url"]').value
		var text = document.querySelector('input[name="link-text"]').value;

		if (text == '') {
			text = selectedText;
		}

		html = '<a href="' + url + '">' + text + '</a>';

		restoreSelection(selection);

		document.execCommand('insertHTML', null, html);

		closeModal();
	});

	document.querySelector('textarea[name="code"]').addEventListener('keydown', function (e) {
		if (e.keyCode == 9) {
			e.preventDefault();
			var start = this.selectionStart;
			var end = this.selectionEnd;
			this.value = this.value.substring(0, start) + "\t" + this.value.substring(end);
			this.selectionStart = this.selectionEnd = start + 1;
		}
	});

	var editorContent = document.getElementById('editor-content');
	addEvents(['mousedown', 'mouseup', 'keydown', 'keyup'], editorContent, function (e) {
		selection = saveSelection();
		selectedText = selection.toString();
	});

	/*if (typeof editorContent.addEventListener != "undefined") {
		editorContent.addEventListener("keypress", enterKeyPressHandler, false);
	} else if (typeof el.attachEvent != "undefined") {
		editorContent.attachEvent("onkeypress", enterKeyPressHandler);
	}*/

	editorContent.addEventListener('keypress', function (e) {
		if (e.keyCode == '13') {
			document.execCommand('insertHTML', false, '<p><br/></p>');
			e.preventDefault();
			return false;
		}
	});

	document.addEventListener('selectionchange', function (e) {
		document.querySelector('a[data-proc="bold"]').classList.toggle('active', document.queryCommandState('bold'));
		document.querySelector('a[data-proc="italic"]').classList.toggle('active', document.queryCommandState('italic'));
		document.querySelector('a[data-proc="underline"]').classList.toggle('active', document.queryCommandState('underline'));
	});

	//editor image insert
	galleryContent = document.getElementById('cde-gallery-content');
	idealColumnWidth = galleryContent.clientWidth < 640 ? 145 : 150;
	
	window.addEventListener('resize', function () {
		var column = galleryContent.getAttribute('data-columns');
		var wd = Math.round(galleryContent.clientWidth / idealColumnWidth);
		galleryContent.setAttribute('data-columns', wd);
	});

	getEditorImages(galleryContent);
	getEditorImages(document.getElementById('article-image-content'));

	document.getElementById('insert-image-btn').addEventListener('click', function () {
		var html = '<img src="/uploads/medium/' + image + '">';
		restoreSelection(selection);
		document.execCommand('insertHTML', null, html);
		closeModal();
	});
});

function addEvents(events, element, callBack) {
	events.forEach(function (item) {
		element.addEventListener(item, callBack);
	});
}

function closeModal() {
	document.querySelectorAll('.modal').forEach(function (item) {
		item.style.display = 'none';
	});
	
}

function saveSelection() {
	if (window.getSelection) {
		sel = window.getSelection();
		if (sel.getRangeAt && sel.rangeCount) {
			return sel.getRangeAt(0);
		}
	} else if (document.selection && document.selection.createRange) {
		return document.selection.createRange();
	}
	return null;
}

function restoreSelection(range) {
	if (range) {
		if (window.getSelection) {
			sel = window.getSelection();
			sel.removeAllRanges();
			sel.addRange(range);
		} else if (document.selection && range.select) {
			range.select();
		}
	}
}

function getEditorImages(galleryContent) {
	var req = new XMLHttpRequest();
	req.onreadystatechange = function () {
		if (this.readyState == 4 && this.status == 200) {
			JSON.parse(this.responseText).forEach(function (item) {
				var thumbnail = document.createElement('div');
				thumbnail.setAttribute('data-image', item.Title);
				thumbnail.setAttribute('data-image-no', item.ID);
				thumbnail.className = 'thumbnail';

				var imageTag = document.createElement('img');
				imageTag.setAttribute('src', '/uploads/thumbnail/' + item.Title);

				thumbnail.appendChild(imageTag);
				thumbnail.addEventListener('click', function (e) {
					document.querySelectorAll('.thumbnail').forEach(function (thmb) {
						thmb.classList.remove('selected');
					});

					if (e.target.tagName === 'IMG') {
						e.target.parentElement.classList.add('selected');
						image = e.target.parentElement.getAttribute('data-image');
					} else {
						e.target.classList.add('selected');
						image = e.target.getAttribute('data-image');
					}
				});

				galleryContent.appendChild(thumbnail);
			});
		}
	};

	req.open('POST', '/Media/EditorImageList', true);
	req.send();
}

$(document).ready(function () {
	$('#editor-content').click(function (e) {

		var isCode = false, node = null;
		for (var i = 0; i < e.originalEvent.path.length; i++) {
			if (e.originalEvent.path[i].nodeName === 'PRE') {
				isCode = true;
				node = e.originalEvent.path[i];
				break;
			}
		}
		
		if (isCode) {
			$('textarea[name="code"]').val(node.innerText);
			$('select[name="language"] option[value="' + node.className.trim() + '"]').prop('selected', true);
			console.log(node.className);
			$('a[data-action="code"]').addClass('active');
		} else {
			$('a[data-action="code"]').removeClass('active');
		}
	});
});
