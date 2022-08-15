(function ($) {

    var _expenseService = abp.services.app.shareMaster,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#ShareMasterEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var expense = _$form.serializeFormToObject();

        abp.ui.setBusy(_$form);
        _expenseService.update(expense).done(function () {
            _$modal.modal('hide');
            abp.notify.success(l('SavedSuccessfully'));
            abp.event.trigger('sharemaster.edited');
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