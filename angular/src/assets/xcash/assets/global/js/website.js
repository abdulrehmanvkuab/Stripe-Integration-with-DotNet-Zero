(function($) {
    "use strict";

    // Constants
    const colors = {
        success: '#28c76f',
        error: '#eb2222',
        warning: '#ff9f43',
        info: '#1e9ff2',
    };

    const icons = {
        success: 'fas fa-check-circle',
        error: 'fas fa-times-circle',
        warning: 'fas fa-exclamation-triangle',
        info: 'fas fa-exclamation-circle',
    };

    const notifications = [];
    const errors = [];

    // Toast Notification
    const triggerToaster = (status, message) => {
        iziToast[status]({
            title: status.charAt(0).toUpperCase() + status.slice(1),
            message: message,
            position: "topRight",
            backgroundColor: '#fff',
            icon: icons[status],
            iconColor: colors[status],
            progressBarColor: colors[status],
            titleSize: '1rem',
            messageSize: '1rem',
            titleColor: '#474747',
            messageColor: '#a2a2a2',
            transitionIn: 'bounceInLeft'
        });
    };

    if (notifications.length) {
        notifications.forEach(element => {
            triggerToaster(element[0], element[1]);
        });
    }

    if (errors.length) {
        errors.forEach(error => {
            triggerToaster('error', error);
        });
    }

    const notify = (status, message) => {
        if (typeof message == 'string') {
            triggerToaster(status, message);
        } else {
            $.each(message, (i, val) => triggerToaster(status, val));
        }
    };

    // Subscription Form Submission
    const formEl = $(".subscription-form");
    formEl.on('submit', function(e) {
        e.preventDefault();
        const data = formEl.serialize();

        if (!formEl.find('input[name=email]').val()) {
            return notify('error', 'Email field is required');
        }

        $.ajax({
            url: "http://localhost:4050/xcash-real/subscribe",
            method: 'post',
            data: data,
            success: function(response) {
                if (response.success) {
                    formEl.find('input[name=email]').val('');
                    notify('success', response.message);
                } else {
                    $.each(response.error, function(key, value) {
                        notify('error', value);
                    });
                }
            },
            error: function(error) {
                console.log(error);
            }
        });
    });

    // Language Selection
    const $mainlangList = $(".langList");
    const $langBtn = $(".language-content");
    const $langListItem = $mainlangList.children();

    $langListItem.each(function() {
        const $innerItem = $(this);
        const $languageText = $innerItem.find(".language_text");
        const $languageFlag = $innerItem.find(".language_flag");

        $innerItem.on("click", function() {
            $langBtn.find(".language_text_select").text($languageText.text());
            $langBtn.find(".language_flag").html($languageFlag.html());
        });
    });

    // Language Change
    $(".languageList").on("click", function() {
        window.location.href = "http://localhost:4050/xcash-real/change/" + $(this).data('code');
    });

    // Select2 Initialization
    $('.select2').select2();

    // Active Menu Item
    $('.main-menu li a[href="http://localhost:4050/xcash-real"]').closest('li').addClass('active');

    // Cookie Policy
    $('.policy').on('click', function() {
        $.get('cookie/accept.html', function() {
            $('.cookies-card').addClass('d-none');
        });
    });

    setTimeout(function() {
        $('.cookies-card').removeClass('hide');
    }, 2000);

    // Form Element Enhancements
    $.each($('input:not([type=checkbox]):not([type=hidden]), select, textarea'), function(i, element) {
        const $element = $(element);
        if ($element.attr('required')) {
            $element.closest('.form-group').find('label').addClass('required');
        }
    });

    const inputElements = $('[type=text],select,textarea,input');
    $.each(inputElements, function(index, element) {
        const $element = $(element);

        if (!$element.hasClass('exclude')) {
            $element.closest('.form-group').find('label').attr('for', $element.attr('name'));
            $element.attr('id', $element.attr('name'));
        }
    });

    // Disable Form Submission
    let disableSubmission = false;
    $('.disableSubmission').on('submit', function(e) {
        if (disableSubmission) {
            e.preventDefault();
        } else {
            disableSubmission = true;
        }
    });

})(jQuery);
