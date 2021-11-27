$(document).ready(function () {
    $('#question-form').submit(function (e) {
        e.preventDefault();
        /*var container = $('#editor-content');
        var html = container.html();

        container.find('pre').each(function (el) {
            var item = $(this).find('code');
            html = html.replace(item.html(), item.text());
        });

        $('#cd-editor-swap').val(html);*/
        var container = $('#editor-content');
        var html = container.html();

        container.find('pre').each(function (el) {
            var item = $(this).find('code');
            html = html.replace(item.html(), item.text());
            html = html.replace('style="text-align: start;"', '');
        });

        $('#cd-editor-swap').val(html);
        $.ajax({
            url: urlGenerator('/Question/Edit'),
            type: 'POST',
            data: $(this).serialize(),
            success: function (data) {
                if (data.status) {
                    $('#general-alert').addClass('alert-bg-info').removeClass('alert-bg-error');
                    $('#general-alert .message').html(data.message);
                    $('#general-alert').slideDown();
                    
                     setTimeout(function () {
                         window.location.href = urlGenerator('soru-duzenle/' + data.slug);
                    }, 1500);
                     

                } else {
                    $('#general-alert').addClass('alert-bg-error');
                    $('#general-alert .message').html(data.message);
                    $('#general-alert').slideDown();
                }

            }
        })
    })
});