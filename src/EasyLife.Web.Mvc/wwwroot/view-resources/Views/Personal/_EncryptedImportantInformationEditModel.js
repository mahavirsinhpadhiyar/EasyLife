(function ($) {

    var _encryptedImportantInformationService = abp.services.app.encryptedImportantInformation,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#EncryptedImportantInformationEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var encryptedImportantInformation = _$form.serializeFormToObject();

        abp.ui.setBusy(_$form);
        _encryptedImportantInformationService.update(encryptedImportantInformation).done(function () {
            _$modal.modal('hide');
            abp.notify.success(l('SavedSuccessfully'));
            abp.event.trigger('encryptedImportantInformation.edited', encryptedImportantInformation);
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