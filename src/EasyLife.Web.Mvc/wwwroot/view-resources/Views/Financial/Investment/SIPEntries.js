var Share_Average_Price, Share_Quantity;

var _sipEntriesService = abp.services.app.sIPEntries,
    l = abp.localization.getSource('EasyLife'),
    _$modal = $('#SIPEntriesCreateModal'),
    _$form = _$modal.find('form'),
    _$table = $('#SIPEntriesTable');

function calculateShareAmount() {
    Share_Average_Price = $(".Share_Average_Price").val().trim();
    Share_Quantity = $(".Share_Quantity").val().trim();
    if (Share_Average_Price != "" && Share_Quantity != "") {
        $(".Share_Amount").val((parseFloat(Share_Average_Price) * parseFloat(Share_Quantity)).toFixed(2));
    }
}

function calculateAveragePrice() {
    _sipEntriesService.getAverageSharePrice($("#ShareMasterId").val()).done(function (result) {
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

    $(".dateTimePicker-sip-entry-filter-startDate").datetimepicker({
        step: 1,
        onShow: function (ct) {
            this.setOptions({
                maxDate: $('.dateTimePicker-sip-entry-filter-endDate').val() ? $('.dateTimePicker-sip-entry-filter-endDate').val() : false
            })
        }
    });

    $(".dateTimePicker-sip-entry-filter-endDate").datetimepicker({
        step: 5,
        onShow: function (ct) {
            this.setOptions({
                minDate: $('.dateTimePicker-sip-entry-filter-startDate').val() ? $('.dateTimePicker-sip-entry-filter-startDate').val() : false
            })
        }
    });

    $(".datetimepicker-input").datetimepicker({
        step: 5,
        defaultDate: new Date(),
        maxDate: new Date()
    });

    var _$SIPEntriesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        searching: false,
        ajax: function (data, callback, settings) {
            debugger;
            var filter = $('#SIPEntriesSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            if (data.order.length == 0) {
                filter.sorting = "SIP_Order_Date desc";
            } else {
                filter.sorting = data.columns[1].data + " desc"
            }
            filter.sIPMasterId = $("#SIPMasterId").val();
            filter.filterStartDate = filter['dateTimePicker-sip-entry-filter-startDate'];
            filter.filterEndDate = filter['dateTimePicker-sip-entry-filter-endDate'];

            abp.ui.setBusy(_$table);
            _sipEntriesService.getAllFiltered(filter).done(function (result) {
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                });
            }).always(function () {
                //calculateAveragePrice();
                abp.ui.clearBusy(_$table);
            });
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$SIPEntriesTable.draw(false)
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
                data: 'siP_Entry_Date_Display',
                sortable: true
            },
            {
                targets: 2,
                data: 'siP_Order_Id',
                sortable: true
            },
            {
                targets: 3,
                data: 'siP_Average_Price',
                sortable: true,
                className: "text-right"
            },
            {
                targets: 4,
                data: 'siP_Units',
                sortable: true,
                className: "text-right"
            },
            {
                targets: 5,
                data: 'sipTypeDisplay',
                sortable: true
            },
            {
                targets: 6,
                data: 'siP_Amount',
                sortable: true,
                className: "text-right"
            },
            {
                targets: 7,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary duplicate-sip-entry" data-sip-entry-id="${row.id}" data-toggle="modal" data-target="#SIPEntriesEditModal" title="Duplicate">`,
                        `       <i class="fas fa-clone" title="Duplicate"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-secondary sip-entry-edit" data-sip-entry-id="${row.id}" data-toggle="modal" data-target="#SIPEntriesEditModal" title="Edit">`,
                        `       <i class="fas fa-pencil-alt" title="Edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-sip-entry" data-sip-entry-id="${row.id}" title="Edit">`,
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

        var sipEntryObj = _$form.serializeFormToObject();
        sipEntryObj.SIPType = $(".SIPType").val();

        abp.ui.setBusy(_$modal);
        _sipEntriesService.create(sipEntryObj)
            .done(function (data) {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.success(l('SavedSuccessfully'));
                _$SIPEntriesTable.ajax.reload();
                //calculateAveragePrice();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.delete-sip-entry', function () {
        var sipEntryId = $(this).attr("data-sip-entry-id");

        deleteSIPEntry(sipEntryId);
    });

    function deleteSIPEntry(sipEntryId) {
        abp.message.confirm(
            abp.utils.formatString("Are You Sure Want To Delete SIP entry"),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _sipEntriesService.delete({
                        id: sipEntryId
                    }).done(() => {
                        abp.notify.success(l('SuccessfullyDeleted'));
                        _$SIPEntriesTable.ajax.reload();
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
        _$SIPEntriesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$SIPEntriesTable.ajax.reload();
            return false;
        }
    });

    abp.event.on('sipentries.edited', function () {
        _$SIPEntriesTable.ajax.reload();
    });

    $(document).on('click', '.sip-entry-edit', function (e) {
        var sipEntryId = $(this).attr("data-sip-entry-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Financial/SIPEntriesEditModal?sipEntryId=' + sipEntryId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#SIPEntriesEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        })
    });

    $(document).on('click', '.duplicate-sip-entry', function (e) {
        var sipEntryId = $(this).attr("data-sip-entry-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Financial/SIPEntriesDuplicateModal?sipEntryId=' + sipEntryId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#SIPEntriesEditModal div.modal-content').html(content);
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

    $(".btnSIPEntries").on("click", function () {
        $(".divSIPEntries").toggle(1000);
    });
})(jQuery)