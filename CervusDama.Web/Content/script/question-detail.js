$(document).ready(function () {

    //Kullanıcı Soruyu Beğendiğinde 
    $('#question-like-btn').on('click', function () {
        updateQuestionVote(true);
    });

    //Kullanıcı Soruyu Beğenmediğinde
    $('#question-dislike-btn').on('click', function () {
        updateQuestionVote(false);
    });

    $('.answer-like-btn').on('click', function () {
        var element = $(this);
        $.ajax({
            url: urlGenerator('/Answer/Like'),
            type: "POST",
            data: { 'AnswerID': $(this).attr('data-id'), 'VoteType': true },
            success: function (data) {
                if (data.status) {
                    element.parent().parent().find('.a-like').html(data.likeCount);
                    element.parent().parent().find('.a-dlike').html(data.disLikeCount);
                } else {
                    $('#general-alert').addClass('alert-bg-error');
                    $('#general-alert .message').html(data.message);
                    $('#general-alert').slideDown();
                }
            }
        })
    });

    $('.answer-dislike-btn').on('click', function () {
        var element = $(this);
        $.ajax({
            url: urlGenerator('/Answer/Like'),
            type: "POST",
            data: { 'AnswerID': $(this).attr('data-id'), 'VoteType': false },
            success: function (data) {
                if (data.status) {
                    element.parent().parent().find('.a-like').html(data.likeCount);
                    element.parent().parent().find('.a-dlike').html(data.disLikeCount);
                } else {
                    $('#general-alert').addClass('alert-bg-error');
                    $('#general-alert .message').html(data.message);
                    $('#general-alert').slideDown();
                }
            }
        })
    });

    //Soru Çözüldü
    $('.solve').on('click', function () {
        $.ajax({
            url: urlGenerator('/Question/Solved'),
            data: { 'id': $('#question-id').val() },
            type: 'POST',
            success: function (data) {
                if (data.status) {
                    $('#general-alert').addClass('alert-bg-info').removeClass('alert-bg-error');
                    $('#general-alert .message').html("İşlem Başarılı");
                    $('#general-alert').slideDown();

                } else {
                    $('#general-alert').addClass('alert-bg-error');
                    $('#general-alert .message').html(data.message);
                    $('#general-alert').slideDown();
                }
            }
        })
    })

    //Bu cevap çözdü
    $('.solve-link').on('click', function () {
        var answerId = $(this).attr('data-id');
        $.ajax({
            url: urlGenerator('/Answer/Solved'),
            type: 'POST',
            data: { 'QuestionID': $('#question-id').val(), 'AnswerID': answerId },
            success: function (data) {
                if (data.status) {
                    $('#general-alert').addClass('alert-bg-info').removeClass('alert-bg-error');
                    $('#general-alert .message').html("İşlem Başarılı");
                    $('#general-alert').slideDown();
                    setTimeout(function () {
                        window.location.reload();
                    }, 1500);

                } else {
                    $('#general-alert').addClass('alert-bg-error');
                    $('#general-alert .message').html(data.message);
                    $('#general-alert').slideDown();
                }
            }
        })
    })

    var questionId;
    $('.delete-answer').on('click', function () {
        $('#check-alert .message').html('Cevabı silmek istediğinizden emin misiniz? Bu işlemi geri alamazsınız!');
        $('#check-alert').slideDown();
        proc = 'answer-delete';
        questionId = $(this).attr('data-id');
    });
    $('#alert-ok-btn').click(function () {
        $('#check-alert').slideUp();
        if (proc == 'answer-delete') {
            $.ajax({
                url: urlGenerator('/Answer/Delete'),
                type: 'POST',
                data: { 'id': questionId },
                success: function (data) {
                    if (data.status) {
                        $('#general-alert').addClass('alert-bg-info').removeClass('alert-bg-error');
                        $('#general-alert .message').html(data.message);
                        $('#general-alert').slideDown();
                        setTimeout(function () {
                            window.location.reload();
                        }, 1500);

                    } else {
                        $('#general-alert').addClass('alert-bg-error');
                        $('#general-alert .message').html(data.message);
                        $('#general-alert').slideDown();
                    }
                }
            })
        }
    });

    $('.edit-answer').on('click', function () {

        $('.page-title').html('Cevap Düzenle');
        var answerComment = $(this).parent().parent().parent().find('.answer-text').html().trim();
        $('#answer-edit-form textarea[name="Content"]').val(answerComment);
        $('#answer-edit-form input[name="ID"]').val($(this).attr('data-id'));
        $('.answer-box').hide();
        $('#answer-edit-form').show();
    });
    $('#form-btn-cancel').click(function (e) {
        e.preventDefault();
        $('.page-title').html('Cevap Gönder');
        $('#answer-edit-form').hide();
        $('.answer-box').show();
        return false;
    });


    $('#answer-edit-form').submit(function (e) {
        e.preventDefault();

        $.ajax({
            type: 'POST',
            url: '/Answer/Edit',
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
});

/**
 * Sorunun beğenilerini arrtırmak  veya azaltmak için
 * @param {boolean} voteType 
 */
function updateQuestionVote(voteType) {
    $.ajax({
        url: urlGenerator('/Question/Like'),
        type: "POST",
        data: { 'QuestionID': $('#question-id').val(), 'VoteType': voteType },
        success: function (data) {
            if (data.status) {
                $('#q-like').html(data.likeCount);
                $('#q-dlike').html(data.disLikeCount);
            } else {
                $('#general-alert').addClass('alert-bg-error');
                $('#general-alert .message').html(data.message);
                $('#general-alert').slideDown();
            }
        }
    })
}
