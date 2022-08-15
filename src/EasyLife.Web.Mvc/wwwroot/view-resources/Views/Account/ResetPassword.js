(function () {
    $('#ReturnUrlHash').val(location.hash);

    var _$form = $('#ResetPasswordForm');

    _$form.submit(function (e) {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        abp.ui.setBusy(
            $('body'),

            abp.ajax({
                contentType: 'application/x-www-form-urlencoded',
                url: _$form.attr('action'),
                data: _$form.serialize()
            }).done(function (data) {
                if (data.message) {
                    if (data.isError) {
                        abp.notify.error(data.message);
                    }
                    else {
                        abp.notify.success(data.message);
                        window.location.href = LoginUrl;
                    }
                }
                else {
                    abp.notify.error(data.message);
                }
            }).fail(function (fatal) {
            })
        );
    });
})();