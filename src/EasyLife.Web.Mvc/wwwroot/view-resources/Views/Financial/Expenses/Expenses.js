var blankGUID = '00000000-0000-0000-0000-000000000000';
var total = 0;

//$('.dateTimePicker-expense-filter-startDate').datepicker({
//    format: 'L',
//    //minDate: new Date(),
//    onSelect: function (selected) {
//        var dt = new Date(selected);
//        dt.setDate(dt.getDate() + 1);
//        $(".dateTimePicker-expense-filter-endDate").datepicker("option", "minDate", dt);
//    }
//});

//$('.dateTimePicker-expense-filter-endDate').datepicker({
//    format: 'L',
//    //minDate: new Date(),
//    onSelect: function (selected) {
//        var dt = new Date(selected);
//        dt.setDate(dt.getDate() + 1);
//        $(".dateTimePicker-expense-filter-startDate").datepicker("option", "maxDate", dt);
//    }
//});

$(".dateTimePicker-expense-filter-startDate").datetimepicker({
    //startDate: '+2018/01/01',
    //format: 'd.m.Y H:i',
    //lang: 'ru',
    step: 1,
    onShow: function (ct) {
        this.setOptions({
            maxDate: $('.dateTimePicker-expense-filter-endDate').val() ? $('.dateTimePicker-expense-filter-endDate').val() : false
        })
    }
    //onSelectDate: function (selected) {
    //    debugger;
    //    var dt = new Date(selected);
    //    dt.setDate(dt.getDate() + 1);
    //    $(".dateTimePicker-expense-filter-endDate").datetimepicker("setOptions", "minDate", dt);
    //}
});

$(".dateTimePicker-expense-filter-endDate").datetimepicker({
    //startDate: '+2018/01/01',
    //format: 'd.m.Y H:i',
    //lang: 'ru',
    step: 5,
    onShow: function (ct) {
        this.setOptions({
            minDate: $('.dateTimePicker-expense-filter-startDate').val() ? $('.dateTimePicker-expense-filter-startDate').val() : false
        })
    }
    //onSelectDate: function (selected) {
    //    debugger;
    //    var dt = new Date(selected);
    //    dt.setDate(dt.getDate() + 1);
    //    $(".dateTimePicker-expense-filter-startDate").datetimepicker("setOptions", "maxDate", dt);
    //}
});

(function ($) {
    var _expensesService = abp.services.app.expenses,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#ExpensesCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ExpensesTable');

    //$(".datetimepicker-input").datepicker({
    //    format: 'L',
    //    maxDate: new Date(),
    //    changeMonth: true,
    //    changeYear: true
    //})
    $(".datetimepicker-input").datetimepicker({
        //startDate: '+2018/01/01',
        //format: 'd.m.Y H:i',
        //lang: 'ru',
        step: 5,
        defaultDate: new Date(),
        maxDate: new Date()
    });

    var _$ExpensesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        searching: false,
        //order: [["ExpenseDate", "asc"]],
        ajax: function (data, callback, settings) {
            var filter = $('#ExpensesSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            filter.filterStartDate = $(".dateTimePicker-expense-filter-startDate").val();
            filter.filterEndDate = $(".dateTimePicker-expense-filter-endDate").val();
            if (data.order.length == 0) {
                filter.sorting = "ExpenseDate desc";
            } else {
                filter.sorting = data.columns[1].data + " desc"
            }
            //filter.sorting = data.columns[data.order[0].column].data + " " + data.order[0].dir
            //data.columns[data.order[0].column].data + " " + data.order[0].dir
            if (filter.expenseCategoryFilterId == "") {
                filter.categoryId = blankGUID;
            }
            else {
                filter.categoryId = filter.expenseCategoryFilterId;
            }

            abp.ui.setBusy(_$table);
            _expensesService.getAllFiltered(filter).done(function (result) {
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
                action: () => _$ExpensesTable.draw(false)
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i>',
                autoPrint: false,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7]
                },
                title: function () {
                    return 'EasyLife / Expenses Report';
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
                data: 'payee',
                sortable: true
            },
            {
                targets: 2,
                data: 'expenseCategoryName',
                sortable: false
            },
            {
                targets: 3,
                data: 'expenseDateDisplay'
            },
            {
                targets: 4,
                className: "text-right",
                data: 'money',
                sortable: true,
                render: (data, type, row, meta) => {
                    total = total + row.money;
                    return format_number(row.money, 2, true);
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
                        `   <button type="button" class="btn btn-sm bg-secondary duplicate-expenses" data-expenses-id="${row.id}" data-toggle="modal" data-target="#ExpensesEditModal" title="Duplicate">`,
                        `       <i class="fas fa-clone" title="Duplicate"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-secondary edit-expenses" data-expenses-id="${row.id}" data-toggle="modal" data-target="#ExpensesEditModal" title="Edit">`,
                        `       <i class="fas fa-pencil-alt" title="Edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-expenses" data-expenses-id="${row.id}" data-name="${row.payee}" title="Edit">`,
                        `       <i class="fas fa-trash" title="Delete"></i>`,
                        '   </button>',
                    ].join('');
                }
            }
        ],
        drawCallback: function () {
            $(".expenseMoneyTotal").text(format_number(total, 2, true));
            total = 0;
        },
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

    //_$ExpensesTable.on('page.dt', function () { total = 0; });

    _$form.find('.save-button').on('click', (e) => {
        e.preventDefault();

        if (!_$form.valid()) {
            return;
        }

        var expensesObj = _$form.serializeFormToObject();

        if (userIsEasyLifeAdmin == "Y") {
            expensesObj.DoNotConsiderInTotal = $("#DoNotConsiderInTotal").prop("checked");
        } else {
            expensesObj.DoNotConsiderInTotal = false;
        }

        abp.ui.setBusy(_$modal);
        _expensesService.create(expensesObj)
            .done(function (data) {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.success(l('SavedSuccessfully'));
                _$ExpensesTable.ajax.reload();
                getDashboardTotalExpense();
                //getCalendarEvents();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.delete-expenses', function () {
        var expensesId = $(this).attr("data-expenses-id");
        var expensesName = $(this).attr('data-name');

        deleteExpenses(expensesId, expensesName);
    });

    function getDashboardTotalExpense() {
        _expensesService.dashboardTotalExpensesSum().done(function (result) {
            $(".divExpensesNotesText").text(result);
        }).always(function () {
            abp.ui.clearBusy(_$table);
        });
    }

    function deleteExpenses(expensesId, expensesName) {
        abp.message.confirm(
            abp.utils.formatString("Are You Sure Want To Delete",
                expensesName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _expensesService.delete({
                        id: expensesId
                    }).done(() => {
                        abp.notify.success(l('SuccessfullyDeleted'));
                        _$ExpensesTable.ajax.reload();
                        getDashboardTotalExpense();
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
        _$ExpensesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$ExpensesTable.ajax.reload();
            return false;
        }
    });

    abp.event.on('expenses.edited', function () {
        _$ExpensesTable.ajax.reload();
        getDashboardTotalExpense();
    });

    $(document).on('click', '.edit-expenses', function (e) {
        var expensesId = $(this).attr("data-expenses-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Financial/ExpensesEditModal?expenseId=' + expensesId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ExpensesEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        })
    });

    $(document).on('click', '.duplicate-expenses', function (e) {
        var expensesId = $(this).attr("data-expenses-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Financial/ExpensesDuplicateModal?expenseId=' + expensesId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#ExpensesEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        })
    });

    $('#ExpenseCategoryFilterId').on("change", function () {
        _$ExpensesTable.ajax.reload();
    });

    if (userIsEasyLifeAdmin) {
        $('#FilterUserId').on("change", function () {
            _$ExpensesTable.ajax.reload();
        });
    }

    //function getCalendarEvents() {
    //    //abp.ui.setBusy($("body"));
    //    _expensesService.getCalendarExpenses().done(function (result) {
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

    $(".btnExpensesNotes").on("click", function () {
        $(".divExpensesNotes").toggle(1000);
    });
})(jQuery);