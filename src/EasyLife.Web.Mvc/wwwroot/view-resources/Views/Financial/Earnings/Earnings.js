var blankGUID = '00000000-0000-0000-0000-000000000000';

$(".dateTimePicker-earning-filter-startDate").datetimepicker({
    //startDate: '+2018/01/01',
    //format: 'd.m.Y H:i',
    //lang: 'ru',
    step: 5,
    onShow: function (ct) {
        this.setOptions({
            maxDate: $('.dateTimePicker-earning-filter-endDate').val() ? $('.dateTimePicker-earning-filter-endDate').val() : false
        })
    }
    //onSelectDate: function (selected) {
    //    debugger;
    //    var dt = new Date(selected);
    //    dt.setDate(dt.getDate() + 1);
    //    $(".dateTimePicker-earning-filter-endDate").datetimepicker("setOptions", "minDate", dt);
    //}
});

$(".dateTimePicker-earning-filter-endDate").datetimepicker({
    //startDate: '+2018/01/01',
    //format: 'd.m.Y H:i',
    //lang: 'ru',
    step: 5,
    onShow: function (ct) {
        this.setOptions({
            minDate: $('.dateTimePicker-earning-filter-startDate').val() ? $('.dateTimePicker-earning-filter-startDate').val() : false
        })
    }
    //onSelectDate: function (selected) {
    //    debugger;
    //    var dt = new Date(selected);
    //    dt.setDate(dt.getDate() + 1);
    //    $(".dateTimePicker-earning-filter-startDate").datetimepicker("setOptions", "maxDate", dt);
    //}
});

(function ($) {
    var _earningService = abp.services.app.earning,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#EarningsCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#EarningsTable');

    //$(".datetimepicker-input").datepicker({
    //    format: 'L',
    //    maxDate: new Date(),
    //    changeMonth: true,
    //    changeYear: true
    //});

    $(".datetimepicker-input").datetimepicker({
        //startDate: '+2018/01/01',
        //format: 'd.m.Y H:i',
        //lang: 'ru',
        step: 1,
        defaultDate: new Date(),
        maxDate: new Date()
    });

    var _$EearningsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        searching: false,
        ajax: function (data, callback, settings) {
            var filter = $('#EarningsSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            filter.filterStartDate = $(".dateTimePicker-earning-filter-startDate").val();
            filter.filterEndDate = $(".dateTimePicker-earning-filter-endDate").val();
            filter.sorting = data.columns[3].data + " desc"
            //data.columns[data.order[0].column].data + " " + data.order[0].dir
            if (filter.earningCategoryFilterId == "") {
                filter.categoryId = blankGUID;
            }
            else {
                filter.categoryId = filter.earningCategoryFilterId;
            }

            abp.ui.setBusy(_$table);
            _earningService.getAllFiltered(filter).done(function (result) {
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                });
            }).always(function () {
                abp.ui.clearBusy(_$table);
            });
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$EearningsTable.draw(false)
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i>',
                autoPrint: false,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7]
                },
                title: function () {
                    return 'EasyLife / Earnings Report';
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
                data: 'payee'
            },
            {
                targets: 2,
                data: 'earningCategoryName'
            },
            {
                targets: 3,
                data: 'earningDateDisplay'
            },
            {
                targets: 4,
                className: "text-right",
                data: 'money',
                render: (data, type, row, meta) => {
                    return '₹ ' + row.money;
                }
            },
            {
                targets: 5,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-earnings" data-earnings-id="${row.id}" data-toggle="modal" data-target="#EarningsEditModal" title="Edit">`,
                        `       <i class="fas fa-pencil-alt" title="Edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-earnings" data-earnings-id="${row.id}" data-name="${row.payee}" title="Edit">`,
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

        var earningsObj = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _earningService.create(earningsObj)
            .done(function (data) {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.success(l('SavedSuccessfully'));
                _$EearningsTable.ajax.reload();
                //getCalendarEvents();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.delete-earnings', function () {
        var earningsId = $(this).attr("data-earnings-id");
        var earningsName = $(this).attr('data-name');

        deleteEarnings(earningsId, earningsName);
    });

    function deleteEarnings(earningsId, earningsName) {
        abp.message.confirm(
            abp.utils.formatString("Are You Sure Want To Delete",
                earningsName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _earningService.delete({
                        id: earningsId
                    }).done(() => {
                        abp.notify.success(l('SuccessfullyDeleted'));
                        _$EearningsTable.ajax.reload();
                        //getCalendarEvents();
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
        _$EearningsTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$EearningsTable.ajax.reload();
            return false;
        }
    });

    abp.event.on('earnings.edited', (data) => {
        _$EearningsTable.ajax.reload();
        //getDashboardValues();
    });

    $(document).on('click', '.edit-earnings', function (e) {
        var earningId = $(this).attr("data-earnings-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Financial/EarningsEditModal?earningId=' + earningId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#EarningsEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        })
    });

    $('#EarningCategoryFilterId').on("change", function () {
        _$EearningsTable.ajax.reload();
    });

    if (userIsEasyLifeAdmin) {
        $('#FilterUserId').on("change", function () {
            _$EearningsTable.ajax.reload();
        });
    }

    //function getCalendarEvents() {
    //    //abp.ui.setBusy($("body"));
    //    _earningService.getCalendarEarnings().done(function (result) {
    //        new Calendar({
    //            id: '#color-calendar',
    //            eventsData: result,
    //            dateChanged: (currentDate, DateEvents) => {
    //                debugger;
    //            },
    //            monthChanged: (currentDate, DateEvents) => {
    //                debugger;
    //            }
    //        });
    //    }).always(function () {
    //        //abp.ui.clearBusy($("body"));
    //    });
    //}
    //getCalendarEvents();

    $(".btnEarningsNotes").on("click", function () {
        $(".divEarningsNotes").toggle(1000);
    });
})(jQuery);