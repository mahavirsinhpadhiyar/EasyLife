(function ($) {

    $('.datetimepicker-input-edit').datepicker({
        format: 'L',
        maxDate: new Date(),
        changeMonth: true,
        changeYear: true
        //dateFormat: 'dd/mm/yy'
        //onSelect: function () {
        //    alert("");
        //}
    });

    var _expenseService = abp.services.app.expenses,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#ExpensesEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var student = _$form.serializeFormToObject();

        abp.ui.setBusy(_$form);
        _expenseService.update(student).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            abp.event.trigger('expenses.edited', student);
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