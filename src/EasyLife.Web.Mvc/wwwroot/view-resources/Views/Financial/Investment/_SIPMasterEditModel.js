(function ($) {

    var _sipService = abp.services.app.sIPMaster,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#SIPMasterEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var sipInfo = _$form.serializeFormToObject();

        abp.ui.setBusy(_$form);
        _sipService.update(sipInfo).done(function () {
            _$modal.modal('hide');
            abp.notify.success(l('SavedSuccessfully'));
            abp.event.trigger('sipmaster.edited');
        }).always(function () {
            abp.ui.clearBusy(_$form);
        });
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

    _$form.find('input').on('keypress', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            save();
        }
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);