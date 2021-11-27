window.addEventListener('scroll', () => {
	let header = document.querySelector('header');
	header.classList.toggle('sticky', window.scrollY > 150);

	var path = window.location.pathname;
	var page = path.split("/").pop();

	if (page == 'anasayfa' || page.length == 0) {
		header.classList.toggle('main-page', window.scrollY <= 150);
	}

	var scrollBtn = document.querySelector('.scroll-top');

	if (window.scrollY > 180) {
		scrollBtn.classList.add('scroll-open');
	} else {
		scrollBtn.classList.remove('scroll-open');
	}
});

window.addEventListener('load', () => {
	let scrollUpBtn = document.querySelector('.scroll-top');
	scrollUpBtn.addEventListener('click', () => {
		window.scrollTo({
			top: 0,
			left: 0,
			behavior: 'smooth'
		});
	});
});

$(document).ready(function () {
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

	//Alert kapat
	$('.alert .close').click(function () {
		$(this).parent().slideUp();
	});

	//Genel Alert
	$('.general-alert a.close').click(function () {
		$('.general-alert').slideUp();
	});

	$('*[data-action="close"]').click(function () {
		$('.general-alert').slideUp();
	});

	//Akordion
	$('.option-content h3').click(function () {
		$(this).next().slideToggle(); //.toggleClass('show');
		$(this).find('span').toggleClass('cdi-arrow-down-alt2').toggleClass('cdi-arrow-up-alt2');
	});

	//okuma modu
	$('#ptc').change(function () {
		var deger = $(this).val();
		$('#pt-vw').html(deger + 'px');
		$('.read-mode-modal p').css({ "font-size": deger + 'px' });
		$('.read-mode-modal h2').css({ "font-size": 22 + (deger - 15) + 'px' });
	});

	$('#ptc').on('input', function () {
		$(this).trigger('change');
	});

	$('#night').click(function () {
		var text = $('.read-menu').hasClass('rm-night') ? 'Karanlık Mod' : 'Aydınlık Mod';
		$(this).text(text);

		var parent = $('#read-box');
		parent.toggleClass('rm-light');
		parent.find('.read-menu').toggleClass('rm-night');
		parent.find('.modal-body').toggleClass('night-mode');
		parent.find('.modal-footer').toggleClass('night-mode-ft');
		parent.find('pre').toggleClass('dark-block');
		parent.find('code').toggleClass('dark-block');
	});
});

function closeAlert() {
	document.getElementById('alert').style.display = 'none';
}

function urlGenerator(uri) {
	var url = 'http://localhost:9000/';

	return url + uri.replace(/^\/|\/$/g, '');
}

function dateParser(date) {
	var tarih = new Date(parseInt(date.substr(6)));
	var aylar = ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"];
	return ("0" + tarih.getDay()).slice(-2) + ' ' + aylar[tarih.getMonth()] + ' ' + tarih.getFullYear() + ' ' + ("0" + tarih.getHours()).slice(-2) + ':' + ("0" + tarih.getMinutes()).slice(-2);
}

function sendRequest(data, url) {
	$.ajax({
		type: 'POST',
		url: urlGenerator(url),
		data: data,
		success: function (res) {

			$('#check-alert').slideUp();

			if (res.status) {
				$('#general-alert').addClass('alert-bg-info');
			} else {
				$('#general-alert').addClass('alert-bg-error');
			}

			$('#general-alert .message').html(res.message);
			$('#general-alert').slideDown();

			if (res.status) {
				setTimeout(function () {
					window.location.reload();
				}, 2000);
			}
		}
	});
}