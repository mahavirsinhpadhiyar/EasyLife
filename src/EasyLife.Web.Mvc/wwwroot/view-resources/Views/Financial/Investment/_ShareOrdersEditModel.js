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

    var _shareOrderService = abp.services.app.shareOrders,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#ShareOrdersEditModal'),
        _$form = _$modal.find('form');

    function save() {
        debugger;
        if (!_$form.valid()) {
            return;
        }

        var shareOrder = _$form.serializeFormToObject();
        shareOrder.Share_Order_Type = $(".Share_Order_Type_Edit").val();
        shareOrder.Share_Qty_Exchange_Type = $(".Share_Qty_Exchange_Type_Edit").val();
        shareOrder.Share_Price_Type = $(".Share_Price_Type_Edit").val();
        shareOrder.Share_Transaction_Type = $(".Share_Transaction_Type_Edit").val();

        abp.ui.setBusy(_$form);
        _shareOrderService.update(shareOrder).done(function () {
            _$modal.modal('hide');
            abp.notify.success(l('SavedSuccessfully'));
            abp.event.trigger('shareorders.edited');
            calculateAveragePrice();
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

    $(document).on('blur', '.Share_Average_Price_Edit', function (e) {
        calculateShareAmountEdit();
    });

    $(document).on('blur', '.Share_Quantity_Edit', function (e) {
        calculateShareAmountEdit();
    });
})(jQuery);