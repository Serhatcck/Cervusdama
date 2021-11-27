$(document).ready(function () {

	$('#PersonalInfoUpdate').submit(function (e) {
		e.preventDefault();

		$.ajax({
			type: 'POST',
			url: $(this).attr('action'),
			data: $(this).serialize(),
			success: function (data) {
				showAlert('#PersonalInfoUpdate-alert', data);
			}
		});
	});

	$('#ConnectInfoUpdate').submit(function (e) {
		e.preventDefault();

		$.ajax({
			type: 'POST',
			url: $(this).attr('action'),
			data: $(this).serialize(),
			success: function (data) {
				showAlert('#ConnectInfoUpdate-alert', data);
			}
		});
	});

	$('#PasswordUpdate').submit(function (e) {
		e.preventDefault();

		$.ajax({
			type: 'POST',
			url: $(this).attr('action'),
			data: $(this).serialize(),
			success: function (data) {
				showAlert('#PasswordUpdate-alert', data);
			}
		});
	});

	$('#profile-delete-form').submit(function (e) {
		e.preventDefault();

		if ($('input[name="IsAccept"]').is(':checked')) {
			$.ajax({
				type: 'POST',
				url: $(this).attr('action'),
				data: $(this).serialize(),
				success: function (data) {
					if (data.status) {
						$('#profile-delete-accept .modal-body').html('<p class="modal-content-text">' + data.message + '</p>');
						$('#profile-delete-accept .modal-footer').html('<button class="btn btn-primary cancel-btn">Tamam</button>');

						setTimeout(function () {
							window.location = '/';
						}, 2000);
					} else {

					}
				}
			});
		} else {
			$('#profile-delete-accept .modal-body label').css({ 'color': '#d95b44' });
			$('#profile-delete-accept .modal-footer span').html('Silme işlemini onaylayınız.');
		}
	});

	$('#profile-image-change').click(function () {
		$('input[name="profileImage"]').trigger('click');
	});

	$('input[name="profileImage"]').change(function () {
		var file = this.files[0];

		var toolTip = $('.profile-img .tooltip');

		if ((file.size / 1024) > 150) {
			toolTip.html('Seçilen dosya çok büyük, en fazla 150 KB olabilir.').fadeIn();
		} else {

			console.log(file.type);

			if (file.type !== 'image/png' && file.type !== 'image/jpg' && file.type !== 'image/jpeg') {
				toolTip.html('Sadece PNG ve JPG formatında resim seçebilirsiniz.').fadeIn();
			} else {
				//Hata yok resmi yükle.
				var formData = new FormData();
				formData.append('profileImage', file);

				$.ajax({
					type: 'POST',
					url: '/Profile/ChangeProfileImage',
					data: formData,
					contentType: false,
					cache: false,
					processData: false,
					async: false,
					success: function (data) {
						if (data.status) {
							var timestamp = new Date().getTime();
							$('#profile-image').attr('src', $('#profile-image').attr('src') + '?t=' + timestamp);
						} else {
							toolTip.html(data.message).fadeIn();
						}
					}
				});
			}
		}
	});

	$('input[name="articleSearch"]').keyup(function () {
		var searchQuery = $(this).val();
		var userID = $('input[name="profileUserID"]').val();

		if (searchQuery.length > 2) {
			$.ajax({
				type: 'POST',
				url: '/Profile/ProfileArticleSearch',
				data: { 'searchQuery': searchQuery, 'userID': userID },
				success: function (data) {
					if (data.status) {
						$('#article-base').hide();
						$('#article-search').html('');

						$.each(data.data, function (index, item) {

							var html = '<div class="article-item"><div class="data-column"><div class="article-icon"><i class="cdi ' + item.BaseCategory.Icon + '" style="color: ' + item.BaseCategory.Color + ';"></i></div></div><div class="data-column"><h5><a href="' + item.Slug + '">' + item.Title + '</a></h5>';

							$.each(item.Tickets, function (i, ticket) {
								html += '<a href="'+ticket.Slug+ '" class="ticket" >' + ticket.Title + '</a>, '
							});

							html += '</div><div class="data-column date">' + item.CreateAtString + '</div><div class="data-column">';

							if (item.Status == 1) {
								html += '<span class="label info">Yayında</span>';
							}else if (item.Status == 2) {
								html += '<span class="label warning">Onaylanmadı</span>';
							}else if (item.Status == 3) {
								html += '<span class="label danger">Düzeltme</span>';
							}

							html += '</div>';
							// TO:DO burada oturum kontrolü gerekli.
							if (item.AuthID == userID) {
								html += '<div class="data-column tool"><a href="#" class="edit"><i class="cdi cdi-edit"></i></a></div><div class="data-column tool"><a href="#" class="block"><i class="cdi cdi-remove"></i></a></div><div class="data-column tool"><a href="#" class="delete"><i class="cdi cdi-trash"></i></a></div>';
							} else {
								html += '<div class="data-column" style="text-align:center;width:110px;">' + item.Hit + ' Görüntüleme</div>';
							}

							html += '</div>';

							$('#article-search').append(html);
						});

						$('#article-search').show();
					} else {

					}
				}
			});
		} else {
			$('#article-search').hide();
			$('#article-base').show();
		}
	});

	//Makale silme
	var proc = '';
	$('a[data-action="article-delete"]').click(function () {
		var id = $(this).data('id');
		$('#check-alert .message').html('Seçtiğiniz makaleyi silmek istediğinizden emin misiniz? Bu işlemi geri alamazsınız.');
		$('#check-alert').slideDown();
		proc = 'article-delete';
		$('#article-id').val(id);
	});

	//Soru Silme
	$('a[data-action="question-delete"]').click(function () {
		var id = $(this).data('id');
		$('#check-alert .message').html('Seçtiğiniz soruyu silmek istediğinizden emin misiniz? Bu işlemi geri alamazsınız.');
		$('#check-alert').slideDown();
		proc = 'question-delete';
		$('#question-id').val(id);
	});

	$('#alert-ok-btn').click(function () {
		if (proc == 'article-delete') {
			sendRequest({ 'id': $('#article-id').val() }, 'Article/Delete');
		} else if (proc == 'question-delete') {
			sendRequest({ 'id': $('#question-id').val() }, 'Question/Delete');
		}
	});

	//makale yayından kaldırma
	$('a[data-action="article-block"]').click(function () {
		var id = $(this).data('id');

		sendRequest({ 'id': id }, 'Article/Block');
	});

	//soru yayından kaldırma
	$('a[data-action="question-block"]').click(function () {
		var id = $(this).data('id');

		sendRequest({ 'id': id }, 'Question/Block');
	});

	
});

$(document).click(function () {
	$('.tooltip').fadeOut();
});

function showAlert(selector, data) {

	if (Object.prototype.toString.call(data) === '[object String]') {
		data = JSON.parse(data);
	}

	if (data.status) {
		$(selector).removeClass('danger').addClass('success');
	} else {
		$(selector).removeClass('success').addClass('danger');
	}
	$(selector + ' .message').html(data.message);
	$(selector).fadeIn();
}