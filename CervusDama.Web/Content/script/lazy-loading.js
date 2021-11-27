(function ($) {
	$.fn.lazyLoading = function (settings) {
		var setting = $.extend({}, settings);

		return this.each(function () {
			var element = $(this);

			var page = 2;
			var isEnd = false;

			$(window).scroll(function () {
				var scroll = $(this).scrollTop();
				var uzunluk = ($(document).height() - $(this).height());

				if (uzunluk == scroll) {

					$.ajax({
						type: 'POST',
						url: setting.url,
						data: { 'page': page },
						success: function (res) {
							if (res.status && !isEnd) {
								page++;
								if (res.data.Articles.length > 0) {

									if (res.data.Articles.length < 20) {
										isEnd = true;
									}

									if (setting.tip == 'article') {

										$.each(res.data.Articles, function (i, item) {
											element.append('<div class="form-post-card"><div class="image"><img src="/uploads/medium/' + item.Image + '" /></div><div class="title"><a href="/makale-detay/@' + item.UserInfo.Slug + '/' + item.Slug + '">' + item.Title + '</a></div><div class="info-text">' + item.Description + '</div><div class="user-info"><div class="profile"><img src="/Uploads/profile/' + item.UserInfo.Slug + '.png" alt="profile"><div class="author-content"><a href="/profil/' + item.UserInfo.Slug + '">' + item.UserInfo.FirstName + item.UserInfo.LastName + '</a><small>' + dateParser(item.CreateAt) + '</small></div></div><div class="share"><a href="javascript:void(0)"><i class="cdi cdi-visibility"></i>&nbsp;&nbsp;' + item.Hit + '</a></div></div></div>');
										});

									} else if (setting.tip == 'question') {
										$.each(res.data, function (i, item) {
											var ticketLi = "";
											for (var i = 0; i < item.Tickets.length; i++) {
												ticketLi += '<li><a href="/etiket/' + item.Tickets[i].Slug + '"><span>#</span>' + item.Tickets[i].Title + '</a></li>';
											}
											element.append('<div class="question"><div class="q-count"><div class="q-view">' + item.Hit + '<br />Görüntüleme</div><div class="q-answer">' + item.AnswerCount + '<br />Yanıt</div></div><div class="q-content"><div class="q-head"><h1><a href="' + item.Slug + '">' + item.Title + '</a></h1>' + ((item.IsSolved) ? '<div class="q-status resolve"><i class="cdi cdi-yes-alt"></i>&nbsp;&nbsp;Çözüldü</div>' : '') + '</div><div class="q-body">' + tem.Content + '</div><div class="q-footer"><ul class="ticket">' + ticketLi + '</ul><div class="q-user"><img src="~/Uploads/profile/' + item.UserSlug + '.png" alt="profile"><a href="' + item.UserSlug + '">' + item.FirstName + ' ' + item.LastName + '</a><div class="q-date">' + dateParser(item.CreateAt) + '</div></div></div></div></div>');
										});
									}
								} else {
									isEnd = true;
									element.append('<div class="no-content">Daha fazla içerik yok!</div > ');
								}
							}
						}
					});
				}
			});
		});
	}
}(jQuery));
