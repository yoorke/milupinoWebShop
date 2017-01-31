$(document).ready(function ($) {
    var offset = 300;
    var offsetOpacity = 1200;
    var scrollTopDuration = 700;
    $backToTop = $('.cd-top');

    $(window).scroll(function () {
        ($(this).scrollTop() > offset) ? $backToTop.addClass('cd-is-visible') : $backToTop.removeClass('cd-is-visible cd-fade-out');
        if ($(this).scrollTop() > offsetOpacity) {
            $backToTop.addClass('cd-fade-out');
        }
    })

    $backToTop.on('click', function (event) {
        event.preventDefault();
        $('body,html').animate({
            scrollTop: 0,
        }, scrollTopDuration);
    })
})