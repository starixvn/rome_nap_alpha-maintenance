$(function () {
    $('.textbox').focus(function () { $('.lb').addClass('active'); })
           .blur(function () { $('.lb').removeClass('active'); });
});

