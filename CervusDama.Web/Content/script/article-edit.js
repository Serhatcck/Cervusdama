$(document).ready(function () {
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

	$('#article-edit-form').submit(function (e) {
		e.preventDefault();
		$.ajax({
			type: 'POST',
			url: urlGenerator('makale-duzenle'),
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