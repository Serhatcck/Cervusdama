$(document).ready(function () {
	// -- Tab Control --
	var link = window.location.href;
	if (link.indexOf('#') > 0) {
		var part = link.split('#');
		var tab_id = part[part.length - 1];

		if ($('.tab-content div[data-tab="' + tab_id + '"]').length > 0) {
			$('.tab-header ul li').removeClass('active');
			$('a[data-index="' + tab_id + '"]').parent().addClass('active');
			$('div[data-tab="' + tab_id + '"]').fadeIn(500);
		} else {
			$('.tab-header ul li:eq(0)').addClass('active');
			$('.tab-item:eq(0)').fadeIn(500);
		}
	} else {
		$('.tab-header ul li:eq(0)').addClass('active');
		$('.tab-item:eq(0)').fadeIn(500);
	}
	$('a[data-index]').click(function () {
		var id = $(this).data('index');
		$('.tab-item').hide();

		$('.tab-header ul li').removeClass('active');
		$(this).parent().addClass('active');

		$('div[data-tab="' + id + '"]').fadeIn(500);
	});

	//Makale görseli seç.
	$('#insert-article-image-btn').click(function () {
		//TO:DO = seçilen görsel hem önizleme yapılacak, hem  de formla gönderilecek şekilde bir form elemanına seçilen resim id si eklenecek.
		var selectedImage = $('#article-image-content').find('.selected');
		alert('makale görseli seçildi');
	});

	$('#form-send-btn').click(function () {
		var container = $('#editor-content');
		var html = container.html();

		container.find('pre').each(function (el) {
			var item = $(this).find('code');
			html = html.replace(item.html(), item.text());
			html = html.replace('style="text-align: start;"', '');
		});

		$('#cd-editor-swap').val(html);

	});

	$('#article-insert-form').submit(function (e) {
		e.preventDefault();
		$.ajax({
			type: 'POST',
			url: urlGenerator('makale-ekle'),
			data: $(this).serialize(),
			success: function (data) {
				if (data.status) {
					$('#alert').addClass('success').removeClass('danger');
				} else {
					$('#alert').addClass('danger').removeClass('success');
				}

				$('#alert .message').html(data.message);

				$('#alert').show();

				if (result.status) {
					setTimeout(function () {
						window.location.href = urlGenerator('makale-detay/' + result.slug);
					}, 1500);
				}
			}
		});
	});

});