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
    /*$('#question-form').submit(function () {
        $('input[name="Content"]').val($('#editor-content').html());
        $.ajax({
            type: 'POST',
            url: $(this).attr('action'),
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: $('#question-form').serialize(),
            success: function (data) {

                if (data.status) {
                    $('#alert').removeClass('danger').addClass('success');
                } else {
                    $('#alert').removeClass('success').addClass('danger');
                    console.log(data.ex);
                }

                $('#alert .message').html(data.message);

                $('#alert').slideDown();
            }
        });

        return false;
    });*/
});

document.addEventListener('DOMContentLoaded', function () {

    
	$('#form-send-btn').click(function () {
		var container = $('#editor-content');
		var html = container.html();

		container.find('pre').each(function (el) {
			var item = $(this).find('code');
			html = html.replace(item.html(), item.text());
		});

		$('#cd-editor-swap').val(html);

	});


    var questionForm = document.getElementById('question-form');
    questionForm.addEventListener('submit', (e) => {
        e.preventDefault();
        new FormData(questionForm);
    });
    questionForm.addEventListener('formdata', (e) => {
        var data = e.formData;
        var req = new XMLHttpRequest();
        req.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                var result = JSON.parse(this.responseText);
                if (result.status) {
                    $('#general-alert').addClass('alert-bg-info').removeClass('alert-bg-error');
                    $('#general-alert .message').html(result.message);
                    $('#general-alert').slideDown();
                    setTimeout(function () {
						window.location.href = urlGenerator('soru-detay/' + result.slug);
					}, 1500);
                } else {
                    $('#general-alert').addClass('alert-bg-error');
                    $('#general-alert .message').html(result.message);
                    $('#general-alert').slideDown();
                }
                /*document.querySelector('#alert .message').innerHTML = result.message;
                document.getElementById('alert').style.display = 'block'*/
            }
        };
        req.open('POST', document.getElementById("question-form").action, true);
        req.send(data);
    });
});

/*
 
 $('#general-alert').addClass('alert-bg-info').removeClass('alert-bg-error');
                    $('#general-alert .message').html(data.message);
                    $('#general-alert').slideDown();
 */