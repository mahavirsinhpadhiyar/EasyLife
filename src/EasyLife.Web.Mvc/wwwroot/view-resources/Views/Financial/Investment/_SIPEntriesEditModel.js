var Share_Average_Price, Share_Quantity;

function calculateShareAmountEdit() {
    Share_Average_Price = $(".Share_Average_Price_Edit").val().trim();
    Share_Quantity = $(".Share_Quantity_Edit").val().trim();
    if (Share_Average_Price != "" && Share_Quantity != "") {
        $(".Share_Amount_Edit").val((parseFloat(Share_Average_Price) * parseFloat(Share_Quantity)).toFixed(2));
    }
}

(function ($) {

    $(".datetimepicker-input").datetimepicker({
        step: 5,
        defaultDate: new Date(),
        maxDate: new Date()
    });

    var _sipEntryService = abp.services.app.sIPEntries,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#SIPEntriesEditModal'),
        _$form = _$modal.find('form');

    function save() {
        if (!_$form.valid()) {
            return;
        }

        var sipInfo = _$form.serializeFormToObject();
        sipInfo.SIPType = $(".SIPType_Edit").val();

        abp.ui.setBusy(_$form);
        _sipEntryService.update(sipInfo).done(function () {
            _$modal.modal('hide');
            abp.notify.success(l('SavedSuccessfully'));
            abp.event.trigger('sipentries.edited');
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

    $(document).on('blur', '.SIP_Average_Price_Edit', function (e) {
        calculateShareAmountEdit();
    });

    $(document).on('blur', '.SIP_Amount_Edit', function (e) {
        calculateShareAmountEdit();
    });

    $(document).on('blur', '.SIP_Units_Edit', function (e) {
        calculateShareAmountEdit();
    });
})(jQuery);