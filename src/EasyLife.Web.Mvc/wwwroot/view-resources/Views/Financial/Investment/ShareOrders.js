var Share_Average_Price, Share_Quantity;

var _shareOrdersService = abp.services.app.shareOrders,
    l = abp.localization.getSource('EasyLife'),
    _$modal = $('#ShareOrdersCreateModal'),
    _$form = _$modal.find('form'),
    _$table = $('#ShareOrdersTable');

function calculateShareAmount() {
    Share_Average_Price = $(".Share_Average_Price").val().trim();
    Share_Quantity = $(".Share_Quantity").val().trim();
    if (Share_Average_Price != "" && Share_Quantity != "") {
        $(".Share_Amount").val((parseFloat(Share_Average_Price) * parseFloat(Share_Quantity)).toFixed(2));
    }
}

function calculateAveragePrice() {
    _shareOrdersService.getAverageSharePrice($("#ShareMasterId").val()).done(function (result) {
        $(".spnAveragePrice").html(result.totalAveragePriceDisplay);
        $(".spnTotalShare").html(result.totalShareDisplay);
        $(".spnTotalInvetsmentAmount").html(result.totalInvestedDisplay);
        if (result.totalAveragePriceDisplay == "N/A") {
            $(".spnOutputOutSpan").css("display", "block");
            if (Math.sign(result.totalEarnedOrLoss) == 1) {
                $(".spnOutput").html(result.totalEarnedOrLossDisplay);
                $(".spnOutput").css("color", "green");
            }
            else {
                $(".spnOutput").html(result.totalEarnedOrLossDisplay);
                $(".spnOutput").css("color", "red");
            }
        }
    }).always(function () {
        abp.ui.clearBusy(_$table);
    });
}

(function ($) {

    $(".spnAveragePrice").html("Loading...");
    $(".spnTotalShare").html("Loading...");
    $(".spnTotalInvetsmentAmount").html("Loading...");
    $(".spnOutput").html("Loading...");

    $(".dateTimePicker-share-order-filter-startDate").datetimepicker({
        step: 1,
        onShow: function (ct) {
            this.setOptions({
                maxDate: $('.dateTimePicker-share-order-filter-endDate').val() ? $('.dateTimePicker-share-order-filter-endDate').val() : false
            })
        }
    });

    $(".dateTimePicker-share-order-filter-endDate").datetimepicker({
        step: 5,
        onShow: function (ct) {
            this.setOptions({
                minDate: $('.dateTimePicker-share-order-filter-startDate').val() ? $('.dateTimePicker-share-order-filter-startDate').val() : false
            })
        }
    });

    $(".datetimepicker-input").datetimepicker({
        step: 5,
        defaultDate: new Date(),
        maxDate: new Date()
    });

    var _$ShareOrdersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        searching: false,
        ajax: function (data, callback, settings) {
            var filter = $('#ShareOrdersSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            if (data.order.length == 0) {
                filter.sorting = "Share_Order_Date desc";
            } else {
                filter.sorting = data.columns[1].data + " desc"
            }
            filter.shareMasterId = $("#ShareMasterId").val();
            filter.filterStartDate = filter['dateTimePicker-share-order-filter-startDate'];
            filter.filterEndDate = filter['dateTimePicker-share-order-filter-endDate'];

            abp.ui.setBusy(_$table);
            _shareOrdersService.getAllFiltered(filter).done(function (result) {
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                });
            }).always(function () {
                calculateAveragePrice();
                abp.ui.clearBusy(_$table);
            });
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$ShareOrdersTable.draw(false)
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i>',
                autoPrint: false,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7]
                },
                title: function () {
                    return 'EasyLife / Share Master Report';
                }
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
                sortable: false
            },
            {
                targets: 1,
                data: 'share_Order_Date_Display',
                sortable: true
            },
            {
                targets: 2,
                data: 'share_Order_Type_Display',
                sortable: true
            },
            {
                targets: 3,
                data: 'share_Qty_Exchange_Type_Display',
                sortable: true
            },
            {
                targets: 4,
                data: 'share_Price_Type_Display',
                sortable: true
            },
            {
                targets: 5,
                data: 'share_Order_Id',
                sortable: true
            },
            {
                targets: 6,
                data: 'share_Transaction_Type_Display',
                sortable: true
            },
            //{
            //    targets: 10,
            //    data: 'share_App_Order_Id',
            //    sortable: true
            //},
            {
                targets: 7,
                data: 'share_Average_Price',
                sortable: true,
                className: "text-right"
            },
            {
                targets: 8,
                data: 'share_Quantity',
                sortable: true,
                className: "text-right"
            },
            {
                targets: 9,
                data: 'share_Amount',
                sortable: true,
                className: "text-right"
            },
            {
                targets: 10,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary duplicate-share-order" data-share-order-id="${row.id}" data-toggle="modal" data-target="#ShareOrdersEditModal" title="Duplicate">`,
                        `       <i class="fas fa-clone" title="Duplicate"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-secondary share-orders-edit" data-share-order-id="${row.id}" data-toggle="modal" data-target="#ShareOrdersEditModal" title="Edit">`,
                        `       <i class="fas fa-pencil-alt" title="Edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-share-orders" data-share-order-id="${row.id}" title="Edit">`,
                        `       <i class="fas fa-trash" title="Delete"></i>`,
                        '   </button>',
                    ].join('');
                }
            }
        ],
        dom: [
            "<'row'<'col-md-12'f>>",
            "<'row'<'col-md-12't>>",
            "<'row mt-2'",
            "<'col-lg-1 col-xs-12'<'float-left text-center data-tables-refresh'B>>",
            "<'col-lg-3 col-xs-12'<'float-left text-center'i>>",
            "<'col-lg-3 col-xs-12'<'text-center'l>>",
            "<'col-lg-5 col-xs-12'<'float-right'p>>",
            ">"
        ].join('')
    });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var shareOrderObj = _$form.serializeFormToObject();
        shareOrderObj.Share_Order_Type = $(".Share_Order_Type").val();
        shareOrderObj.Share_Qty_Exchange_Type = $(".Share_Qty_Exchange_Type").val();
        shareOrderObj.Share_Price_Type = $(".Share_Price_Type").val();
        shareOrderObj.Share_Transaction_Type = $(".Share_Transaction_Type").val();

        abp.ui.setBusy(_$modal);
        _shareOrdersService.create(shareOrderObj)
            .done(function (data) {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.success(l('SavedSuccessfully'));
                _$ShareOrdersTable.ajax.reload();
                calculateAveragePrice();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.delete-share-orders', function () {
        var shareOrderId = $(this).attr("data-share-order-id");

        deleteShareOrder(shareOrderId);
    });

    function deleteShareOrder(shareOrderId) {
        abp.message.confirm(
            abp.utils.formatString("Are You Sure Want To Delete Share Order entry"),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _shareOrdersService.delete({
                        id: shareOrderId
                    }).done(() => {
                        abp.notify.success(l('SuccessfullyDeleted'));
                        _$ShareOrdersTable.ajax.reload();
                    });
                }
            }
        );
    }

    _$modal.on('shown.bs.modal', () => {
        _$modal.find('input:not([type=hidden]):first').focus();
    }).on('hidden.bs.modal', () => {
        _$form.clearForm();
    });

    $('.btn-search').on('click', (e) => {
        _$ShareOrdersTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$ShareOrdersTable.ajax.reload();
            return false;
        }
    });

    abp.event.on('shareorders.edited', function () {
        _$ShareOrdersTable.ajax.reload();
    });

    $(document).on('click', '.share-orders-edit', function (e) {
        var shareOrderId = $(this).attr("data-share-order-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Financial/ShareOrdersEditModal?shareOrderId=' + shareOrderId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ShareOrdersEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        })
    });

    $(document).on('click', '.duplicate-share-order', function (e) {
        var shareOrderId = $(this).attr("data-share-order-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Financial/ShareOrderDuplicateModal?shareOrderId=' + shareOrderId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ShareOrdersEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        })
    });

    $(document).on('blur', '.Share_Average_Price', function (e) {
        calculateShareAmount();
    });

    $(document).on('blur', '.Share_Quantity', function (e) {
        calculateShareAmount();
    });

    $(".btnShareOrders").on("click", function () {
        $(".divShareOrders").toggle(1000);
    });
})(jQuery)