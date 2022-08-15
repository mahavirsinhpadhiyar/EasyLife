var _shareMasterService;

//function calculareTotalInvested() {
//    _shareMasterService.calculateTotalInvested().done(function (result) {
//        $(".spnTotalInvestment").html(result);
//    }).always(function () {
//        abp.ui.clearBusy(_$table);
//    });
//}

(function ($) {
    _shareMasterService = abp.services.app.shareMaster,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#ShareMasterCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ShareMasterTable');

    var _$ShareMasterTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        searching: false,
        ajax: function (data, callback, settings) {
            var filter = $('#ShareMasterSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            if (data.order.length == 0) {
                filter.sorting = "Share_Name desc";
            } else {
                filter.sorting = data.columns[1].data + " desc"
            }

            abp.ui.setBusy(_$table);
            _shareMasterService.getAllFiltered(filter).done(function (result) {
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
                action: () => _$ShareMasterTable.draw(false)
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
                data: 'share_Name',
                sortable: true
            },
            {
                targets: 2,
                data: 'totalShareDisplay',
                className: "text-right",
                sortable: true
            },
            {
                targets: 3,
                data: 'totalAveragePriceDisplay',
                className: "text-right",
                sortable: true
            },
            {
                targets: 4,
                data: 'totalInvestedDisplay',
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
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <a href="/Financial/ShareOrders?shareMasterId=${row.id}" type="button" class="btn btn-sm bg-secondary orders-share-master" title="Orders">`,
                        `       <i class="fas fa-list" title="Orders"></i>`,
                        '   </a>',
                        `   <button type="button" class="btn btn-sm bg-secondary share-master-expenses" data-share-master-id="${row.id}" data-toggle="modal" data-target="#ShareMasterEditModal" title="Edit">`,
                        `       <i class="fas fa-pencil-alt" title="Edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-share-master" data-share-master-id="${row.id}" data-name="${row.payee}" title="Edit">`,
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
        _shareMasterService.create(shareMasterObj)
            .done(function (data) {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.success(l('SavedSuccessfully'));
                _$ShareMasterTable.ajax.reload();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.delete-share-master', function () {
        var shareMasterId = $(this).attr("data-share-master-id");
        var shareMasterName = $(this).attr('data-name');

        deleteShareMaster(shareMasterId, shareMasterName);
    });

    function deleteShareMaster(shareMasterId, shareMasterName) {
        abp.message.confirm(
            abp.utils.formatString("Are You Sure Want To Delete Master Share entry and it's orders too",
                shareMasterName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _shareMasterService.delete({
                        id: shareMasterId
                    }).done(() => {
                        abp.notify.success(l('SuccessfullyDeleted'));
                        _$ShareMasterTable.ajax.reload();
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
        _$ShareMasterTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$ShareMasterTable.ajax.reload();
            return false;
        }
    });

    abp.event.on('sharemaster.edited', function () {
        _$ShareMasterTable.ajax.reload();
    });

    $(document).on('click', '.share-master-expenses', function (e) {
        var shareMasterId = $(this).attr("data-share-master-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Financial/ShareMasterEditModal?shareMasterId=' + shareMasterId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ShareMasterEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        })
    });

    if (userIsEasyLifeAdmin) {
        $('#FilterUserId').on("change", function () {
            _$ShareMasterTable.ajax.reload();
        });
    }

    $(".btnShareMasterNotes").on("click", function () {
        $(".divShareMasterNotes").toggle(1000);
    });

    //$(".spnTotalInvestment").html("Loading...");
})(jQuery);