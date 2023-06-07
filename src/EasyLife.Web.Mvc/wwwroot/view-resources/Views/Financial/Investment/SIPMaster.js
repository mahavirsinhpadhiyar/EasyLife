var _sipMasterService;

//function calculareTotalInvested() {
//    _sipMasterService.calculateTotalInvested().done(function (result) {
//        $(".spnTotalInvestment").html(result);
//    }).always(function () {
//        abp.ui.clearBusy(_$table);
//    });
//}

(function ($) {
    _sipMasterService = abp.services.app.sIPMaster,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#SIPMasterCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#SIPMasterTable');

    var _$SIPMasterTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        searching: false,
        ajax: function (data, callback, settings) {
            var filter = $('#SIPMasterSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            if (data.order.length == 0) {
                filter.sorting = "SIP_Name desc";
            } else {
                filter.sorting = data.columns[1].data + " desc"
            }

            abp.ui.setBusy(_$table);
            _sipMasterService.getAllFiltered(filter).done(function (result) {
                debugger;
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                });
            }).always(function () {
                /*calculareTotalInvested();*/
                abp.ui.clearBusy(_$table);
            });
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$SIPMasterTable.draw(false)
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i>',
                autoPrint: false,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7]
                },
                title: function () {
                    return 'EasyLife / SIP Master Report';
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
                data: 'siP_Name',
                sortable: true
            },
            {
                targets: 2,
                data: 'siP_Folio_No',
                className: "text-right",
                sortable: true
            },
            {
                targets: 3,
                data: 'averageNAV',
                className: "text-right",
                sortable: true
            },
            {
                targets: 4,
                data: 'currentAveragePrice',
                className: "text-right",
                sortable: true
            },
            {
                targets: 5,
                data: null,
                sortable: true,
                className: "text-center",
                render: (data, type, row, meta) => {
                    if (row.totalShareDisplay == "0") {
                        if (parseFloat(row.totalEarnedOrLoss) > 0) {
                            return '<small class="text-success mr-1"><i class="fas fa-arrow-up"></i> ' + row.totalEarnedOrLossDisplay + '</b></small><b>';
                        } else {
                            return '<small class="text-danger mr-1"><i class="fas fa-arrow-down"></i> ' + row.totalEarnedOrLossDisplay + '</b></small><b>';
                        }
                    } else {
                        return '<span>' + row.totalEarnedOrLossDisplay + '</span>';
                    }
                }
            },
            {
                targets: 6,
                data: 'totalInvestedAmount',
                className: "text-right",
                sortable: true
            },
            {
                targets: 7,
                data: 'totalPurchasedUnits',
                className: "text-right",
                sortable: true
            },
            {
                targets: 8,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <a href="/Financial/SIPEntries?sipMasterId=${row.id}" type="button" class="btn btn-sm bg-secondary orders-sip" title="Entries">`,
                        `       <i class="fas fa-list" title="Entries"></i>`,
                        '   </a>',
                        `   <button type="button" class="btn btn-sm bg-secondary edit-sip-master" data-sip-master-id="${row.id}" data-toggle="modal" data-target="#SIPMasterEditModal" title="Edit">`,
                        `       <i class="fas fa-pencil-alt" title="Edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-sip-master" data-sip-master-id="${row.id}" data-name="${row.payee}" title="Edit">`,
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

        var shareMasterObj = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _sipMasterService.create(shareMasterObj)
            .done(function (data) {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.success(l('SavedSuccessfully'));
                _$SIPMasterTable.ajax.reload();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.delete-sip-master', function () {
        var sipMasterId = $(this).attr("data-sip-master-id");
        var sipMasterName = $(this).attr('data-name');

        deleteSIPMaster(sipMasterId, sipMasterName);
    });

    function deleteSIPMaster(sipMasterId, sipMasterName) {
        abp.message.confirm(
            abp.utils.formatString("Are You Sure Want To Delete Master SIP entry and it's entries too",
                sipMasterName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _sipMasterService.delete({
                        id: sipMasterId
                    }).done(() => {
                        abp.notify.success(l('SuccessfullyDeleted'));
                        _$SIPMasterTable.ajax.reload();
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
        _$SIPMasterTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$SIPMasterTable.ajax.reload();
            return false;
        }
    });

    abp.event.on('sipmaster.edited', function () {
        _$SIPMasterTable.ajax.reload();
    });

    $(document).on('click', '.edit-sip-master', function (e) {
        var sipMasterId = $(this).attr("data-sip-master-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Financial/SIPMasterEditModal?sipMasterId=' + sipMasterId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#SIPMasterEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        })
    });

    if (userIsEasyLifeAdmin) {
        $('#FilterUserId').on("change", function () {
            _$SIPMasterTable.ajax.reload();
        });
    }

    $(".btnSIPMasterNotes").on("click", function () {
        $(".divSIPMasterNotes").toggle(1000);
    });

    //$(".spnTotalInvestment").html("Loading...");
})(jQuery);