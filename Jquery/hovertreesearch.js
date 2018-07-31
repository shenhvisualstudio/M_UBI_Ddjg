function searchToggle(obj, evt) {
    var container = $(obj).closest('.search-wrapper');

    if (!container.hasClass('active')) {
        container.addClass('active');
        evt.preventDefault();
    }
    else if (container.hasClass('active') && $(obj).closest('.input-holder').length == 0) {
        container.removeClass('active');
        // clear input
        container.find('.search-input').val('');
        // clear and hide result container when we press close
        container.find('.result-container').fadeOut(100, function () { $(this).empty(); });
    }
}

function submitFn(obj, evt) {
    var value = $(obj).find('.search-input').val().trim();

    var _html = "您搜索的关键词： ";
    if (!value.length) {
        _html = "关键词不能为空。";
    }
    else {
        _html += "<div><iframe src=\"WeightNote.html\" frameborder=\"0\" style=\"width: 100% ;height:500px; margin-top: 40px;\" scrolling=\"yes\"></iframe></div>";
    }

    $(obj).find('.result-container').html(_html);
    $(obj).find('.result-container').fadeIn(100);

    evt.preventDefault();
}