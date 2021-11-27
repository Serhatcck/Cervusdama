$(document).ready(function () {

	//okuma süresi hesaplama
	var text = $('article').text();
	text = text.replace(/(^\s*) | (\s*$)/gi, "");
	text = text.replace(/[ ]{2,}/gi, " ");
	text = text.replace(/\n/, /"\n"/);

	var word = text.split(' ');
	var wordSize = word.length;

	$('#okuma-suresi').html('Tahmini okuma süresi : ' + Math.ceil(((wordSize / 2) / 60)) + ' dakika.');

	//makale beğenileri
	$('#like-btn').click(function () {
		$.ajax({
			type: 'POST',
			url: urlGenerator('/Article/Like'),
			data: { 'ArticleID': $('#article-id').val(), 'VoteType': true },
			success: function (data) {
				if (data.status) {
					$('#like-counter').html(data.likeCount);
					$('#dislike-counter').html(data.disLikeCount);
				} else {
					$('#general-alert').addClass('alert-bg-error');
					$('#general-alert .message').html(data.message);
					$('#general-alert').slideDown();
				}
			}
		});
	});

	$('#dislike-btn').click(function () {
		$.ajax({
			type: 'POST',
			url: urlGenerator('/Article/Like'),
			data: { 'ArticleID': $('#article-id').val(), 'VoteType': false },
			success: function (data) {
				if (data.status) {
					$('#like-counter').html(data.likeCount);
					$('#dislike-counter').html(data.disLikeCount);
				} else {
					$('#general-alert').addClass('alert-bg-error');
					$('#general-alert .message').html(data.message);
					$('#general-alert').slideDown();
				}
			}
		});
	});

	//yorum ekleme
	$('#comment-form').submit(function (e) {
		e.preventDefault();

		$.ajax({
			type: 'POST',
			url: urlGenerator('/Comment/InsertComment'),
			data: $(this).serialize(),
			success: function (data) {

				if (data.status) {
					$('#general-alert').addClass('alert-bg-info');
				} else {
					$('#general-alert').addClass('alert-bg-error');
				}

				$('#general-alert .message').html(data.message);
				$('#general-alert').slideDown();

				if (data.status) {
					setTimeout(function () {
						window.location.reload();
					}, 2000);
				}
			}
		});
	});

	$('#comment-content').keyup(function () {
		$('#letter-counter').html($(this).val().length + " / 5000");
	});

	//Yorum düzenleme
	$('.comment-menu a.edit').click(function () {
		var commentID = $(this).data('id');
		$('#comment-edit-form input[name="CommentID"]').val(commentID);
		var commentText = $(this).parents(4).children('.comment-text').html().trim();
		$('#comment-edit-content').val(commentText);
		$('#comment-form').hide();
		$('#comment-edit-form').show();
		$('#comment-title').html('Yorum Düzenle');
	});

	$('#form-btn-cancel').click(function (e) {
		e.preventDefault();
		$('#comment-title').html('Yorum Gönder');
		$('#comment-edit-form').hide();
		$('#comment-form').show();
		return false;
	});

	$('#comment-edit-form').submit(function (e) {
		e.preventDefault();

		$.ajax({
			type: 'POST',
			url: '/Comment/EditArticleComment',
			data: $(this).serialize(),
			success: function (data) {
				if (data.status) {
					$('#general-alert').addClass('alert-bg-info');
				} else {
					$('#general-alert').addClass('alert-bg-error');
				}

				$('#general-alert .message').html(data.message);
				$('#general-alert').slideDown();

				if (data.status) {
					setTimeout(function () {
						window.location.reload();
					}, 2000);
				}
			}
		});
	});

	//Yorum silme
	var proc = '';
	$('.comment-menu a.delete').click(function () {
		var id = $(this).data('id');
		$('#check-alert .message').html('Yorumu silmek istediğinizden emin misiniz? Bu işlemi geri alamazsınız!');
		$('#check-alert').slideDown();
		proc = 'comment-delete';
		$('#comment-id').val(id);
	});

	$('#alert-ok-btn').click(function () {
		if (proc == 'comment-delete') {
			sendRequest({ 'id': $('#comment-id').val() }, '/Comment/Delete');
		}
	});

	//Yoruma beğeni gönderme
	var element;
	$('a[data-action="like"]').click(function () {
		element = $(this);
		console.log(element.parents());
		var id = element.data('id');
		$.ajax({
			type: 'POST',
			url: urlGenerator('Comment/Like'),
			data: { "CommentID": id, "VoteType": true },
			success: function (data) {
				if (data.status) {
					element.parents(1).children('.vote-item').find('.like-val').html(data.likeCount);
					element.parents(1).children('.vote-item').find('.dlike-val').html(data.disLikeCount);
				} else {
					$('#general-alert').addClass('alert-bg-error');
					$('#general-alert .message').html(data.message);
					$('#general-alert').slideDown();
				}
			}
		});
	});

	$('a[data-action="dislike"]').click(function () {
		element = $(this);
		var id = element.data('id');
		$.ajax({
			type: 'POST',
			url: urlGenerator('Comment/Like'),
			data: { "CommentID": id, "VoteType": false },
			success: function (data) {
				if (data.status) {
					element.parents(1).children('.vote-item').find('.like-val').html(data.likeCount);
					element.parents(1).children('.vote-item').find('.dlike-val').html(data.disLikeCount);
				} else {
					$('#general-alert').addClass('alert-bg-error');
					$('#general-alert .message').html(data.message);
					$('#general-alert').slideDown();
				}
			}
		});
	});
});

// -- Başlık Geçişleri --
const browserHeight = document.documentElement.clientHeight * (1 / 2);
let baslik = [];
let topicClick = false;
window.addEventListener('load', () => {

	baslik = Array.from(document.querySelectorAll('article h2')).filter(b => b.offsetParent !== null);
	let contentList = document.getElementById('content-table');

	baslik.forEach(function (element) {
		let text = element.innerHTML;
		let id = text.toLowerCase().replace(/[^a-z0-9 -]/g, '').replace(/\s+/g, '-').replace(/-+/g, '-');
		element.setAttribute('id', id);

		let li = document.createElement('li');
		let a = document.createElement('a');
		a.href = 'javascript:void(0)';
		a.dataset.ref = '#' + id;
		a.text = text;
		a.addEventListener('click', (e) => {
			scrollToHeading(e.target.getAttribute('data-ref'));
		});
		li.appendChild(a);

		contentList.appendChild(li);
	});

	document.querySelectorAll('#content-table li')[1].classList.add('active');
});

function scrollToHeading(id) {
	console.log(id);
	let element = document.querySelector("[id='" + id.replace('#', '') + "']");
	let offset = element.offsetTop;
	topicClick = true;
	document.querySelectorAll('#content-table li').forEach(function (el) {
		el.classList.remove('active');
	});

	id = 'a[data-ref="' + id + '"]';
	let activeLink = document.querySelector(id);
	activeLink.parentNode.classList.add('active');

	let body = $('html');
	body.stop().animate({ scrollTop: (offset - 80) }, 600, 'swing', function () {
		topicClick = false;
	});
}

window.addEventListener('scroll', () => {
	if (!topicClick) {
		let id = '';
		for (let i = baslik.length - 1; i >= 0; i--) {
			const rect = baslik[i].getBoundingClientRect();
			if (rect.top + rect.height / 3 <= browserHeight) {
				id = baslik[i].getAttribute('id');
				break;
			}
		}

		if (id !== '') {
			document.querySelectorAll('#content-table li').forEach(function (el) {
				el.classList.remove('active');
			});
			id = 'a[data-ref="#' + id + '"]';
			let activeLink = document.querySelector(id);
			activeLink.parentNode.classList.add('active');
		}
	}
});