var stepSize = 0;
var activeStep = 0;

$(document).ready(function () {
	/*var idealColumnWidth = $('.wrapper').width() < 640 ? 135 : 150;

	$(window).resize(function () {
		var wd = Math.min(Math.round($('.wrapper').width() / idealColumnWidth), 9) || 1;
		$('.gallery').attr('data-columns', wd);
	});*/

	//Akordion
	$('.accordion-content').hide().eq(0).show();

	$('.accordion-header').click(function () {
		$(this).parent().find('.accordion-content').slideToggle();
		$(this).find('i').toggleClass('fa-chevron-down').toggleClass('fa-chevron-up');
	});

	//MessageBox
	$('.message a[data-role="close"]').click(function () {
		$(this).parent().slideUp('fast');
	});

	//alert
	$('.alert .close').click(function () {
		$(this).parent().slideUp();
	});

	// Modal 
	let modalID = '';
	$('*[data-trigger="modal"]').click(function () {
		modalID = $(this).data('target');

		if (!modalID.startsWith('#')) {
			modalID = '#' + modalID;
		}

		$('body').addClass('modal-fix');
		$('.modal').find('form').trigger('reset');
		$('.modal-footer span').html('');
		$(modalID).fadeIn();
	});

	$('.modal').find('.close').click(function () {
		$(modalID).fadeOut();
		$('body').removeClass('modal-fix');
	});

	$('.modal').on('click', '.cancel-btn', function () {
		$(modalID).fadeOut();
		$('body').removeClass('modal-fix');
	});

	//Tab Control
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

	//Fotogaleri
	var idealColumnWidth = $('#gallery-container').width() < 640 ? 135 : 150;

	$(window).resize(function () {
		var column = $('#gallery-container .gallery').data('columns');
		var wd = Math.min(Math.round($('#gallery-container').width() / idealColumnWidth), parseInt(column)) || 1;
		$('.gallery').attr('data-columns', wd);
	});

	//menü aktif
	var path = window.location.pathname;

	if (path === '/yazi-ekle') {
		path = '/yazilar';
	} else if (path === '/medya-yukle') {
		path = '/medya';
	}

	$('.nav-menu a[href="' + path + '"]').addClass('active');

	//form wizard
	stepSize = $('.form-step-content').length;

	$('#next-step').click(function () {
		if (stepSize > activeStep) {
			$('.form-step-content').eq(activeStep).hide();

			activeStep++;

			$('.form-step-content').eq(activeStep).show();
			$('.form-steps .step').eq(activeStep).addClass('active');

			if (activeStep > 0) {
				$('#back-step').show();
			} else {
				$('#back-step').hide();
			}

			if (activeStep == (stepSize - 1)) {
				$('#save-btn').show();
				$('#next-step').hide();
			} else {
				$('#save-btn').hide();
			}
		}
	});

	$('#back-step').click(function () {
		if (activeStep > 0) {
			$('.form-step-content').eq(activeStep).hide();
			$('.form-steps .step').eq(activeStep).removeClass('active');
			activeStep--;

			$('.form-step-content').eq(activeStep).show();


			if (activeStep < (stepSize - 1)) {
				$('#next-step').show();
				$('#save-btn').hide();
			}

			if (activeStep == 0) {
				$('#back-step').hide();
			}
		}
	});

	//DropDown Menu
	$('*[data-menu="dropdown"]').click(function () {
		$('.system-menu-wrap').slideUp('fast');
		var target = $(this).data('target');
		var menu = $(target);
		menu.slideToggle();
	});
});

window.onclick = function (event) {
	if (!event.target.matches('.dropbtn') && !event.target.matches('.search-input') && !event.target.matches('.system-menu-wrap')) {
		$('.system-menu-wrap').slideUp();
	}
}

function closeDialog(id) {
	$('#' + id).fadeOut();
}

function dateParser(date) {
	var tarih = new Date(parseInt(date.substr(6)));
	var aylar = ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"];
	return ("0" + tarih.getDay()).slice(-2) + ' ' + aylar[tarih.getMonth()] + ' ' + tarih.getFullYear() + ' ' + ("0" + tarih.getHours()).slice(-2) + ':' + ("0" + tarih.getMinutes()).slice(-2);
}