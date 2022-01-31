var blankGUID = '00000000-0000-0000-0000-000000000000';
(function ($) {
    var _expensesService = abp.services.app.expenses,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#ExpensesCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#ExpensesTable');

    $(".datetimepicker-input").datepicker({
        format: 'L',
        maxDate: new Date(),
        changeMonth: true,
        changeYear: true
    })

    var _$ExpensesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        searching: false,
        ajax: function (data, callback, settings) {
            var filter = $('#ExpensesSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            filter.sorting = data.columns[1].data + " desc"
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
                data: 'payee'
            },
            {
                targets: 2,
                data: 'expenseCategoryName'
            },
            {
                targets: 3,
                data: 'expenseDateDisplay'
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

        var expensesObj = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _expensesService.create(expensesObj)
            .done(function (data) {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.success(l('SavedSuccessfully'));
                _$ExpensesTable.ajax.reload();
                getCalendarEvents();
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
                        abp.notify.info(l('SuccessfullyDeleted'));
                        _$ExpensesTable.ajax.reload();
                        getCalendarEvents();
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

    abp.event.on('expenses.edited', (data) => {
        _$ExpensesTable.ajax.reload();
        getDashboardValues();
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

    $('#ExpenseCategoryFilterId').on("change", function () {
        _$ExpensesTable.ajax.reload();
    });

    function getCalendarEvents() {
        //abp.ui.setBusy($("body"));
        _expensesService.getCalendarExpenses().done(function (result) {
            new Calendar({
                id: '#color-calendar',
                eventsData: result,
                dateChanged: (currentDate, DateEvents) => {
                    debugger;
                },
                monthChanged: (currentDate, DateEvents) => {
                    debugger;
                }
            });
        }).always(function () {
            //abp.ui.clearBusy($("body"));
        });
    }

    //Useful for showing total expenses of the month and year and average daily expense
    //function getDashboardValues() {
    //    abp.ui.setBusy(_$table);
    //    _classService.getDashboardValues({ id: $("#ClassId").val() }).done(function (result) {
    //        $(".enrolledStudents").html("Enrolled Students: " + result.enrolledStudents);
    //        $(".averageCost").html("Average Cost: " + result.averageCost);
    //        $(".totalEnrollmentFees").html("Total Enrollment Fees: " + result.totalEnrollmentFees);
    //        $(".amountReceived").html("Amount Received: " + result.amountReceived);
    //        $(".amountDue").html("Amount Due: " + result.amountDue);
    //    }).always(function () {
    //        abp.ui.clearBusy(_$table);
    //    });
    //}

    getCalendarEvents();
})(jQuery);