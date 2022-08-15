var blankGUID = '00000000-0000-0000-0000-000000000000';
(function ($) {
    var _encryptedImportantInformationService = abp.services.app.encryptedImportantInformation,
        l = abp.localization.getSource('EasyLife'),
        _$modal = $('#EncryptedImportantInformationCreateModal'),
        _$form = _$modal.find('form'),
        _$table = $('#EncryptedImportantInformationTable');

    var _$EncryptedImportantInformationTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        searching: false,
        ajax: function (data, callback, settings) {
            var filter = $('#EncryptedImportantInformationSearchForm').serializeFormToObject(true);
            filter.maxResultCount = data.length;
            filter.skipCount = data.start;
            filter.sorting = data.columns[1].data + " desc"
            //data.columns[data.order[0].column].data + " " + data.order[0].dir

            abp.ui.setBusy(_$table);
            _encryptedImportantInformationService.getAllFiltered(filter).done(function (result) {
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
                action: () => _$EncryptedImportantInformationTable.draw(false)
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
                data: 'title'
            },
            {
                targets: 2,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `   <button type="button" class="btn btn-sm bg-secondary edit-encryptedImportantInformation" data-encryptedImportantInformation-id="${row.id}" data-toggle="modal" data-target="#EncryptedImportantInformationEditModal" title="Edit">`,
                        `       <i class="fas fa-pencil-alt" title="Edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-sm bg-danger delete-encryptedImportantInformation" data-encryptedImportantInformation-id="${row.id}" data-name="${row.title}" title="Edit">`,
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

        var encryptedImportantInformationObj = _$form.serializeFormToObject();

        abp.ui.setBusy(_$modal);
        _encryptedImportantInformationService.create(encryptedImportantInformationObj)
            .done(function (data) {
                _$modal.modal('hide');
                _$form[0].reset();
                abp.notify.success(l('SavedSuccessfully'));
                _$EncryptedImportantInformationTable.ajax.reload();
                //getCalendarEvents();
            })
            .always(function () {
                abp.ui.clearBusy(_$modal);
            });
    });

    $(document).on('click', '.delete-encryptedImportantInformation', function () {
        var encryptedImportantInformationId = $(this).attr("data-encryptedImportantInformation-id");
        var encryptedImportantInformationTitle = $(this).attr('data-name');

        deleteEncryptedImportantInformation(encryptedImportantInformationId, encryptedImportantInformationTitle);
    });

    function deleteEncryptedImportantInformation(encryptedImportantInformationId, encryptedImportantInformationTitle) {
        abp.message.confirm(
            abp.utils.formatString("Are You Sure Want To Delete",
                encryptedImportantInformationTitle),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _encryptedImportantInformationService.delete({
                        id: encryptedImportantInformationId
                    }).done(() => {
                        abp.notify.success(l('SuccessfullyDeleted'));
                        _$EncryptedImportantInformationTable.ajax.reload();
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
        _$EncryptedImportantInformationTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$EncryptedImportantInformationTable.ajax.reload();
            return false;
        }
    });

    abp.event.on('encryptedImportantInformation.edited', (data) => {
        _$EncryptedImportantInformationTable.ajax.reload();
        //getDashboardValues();
    });

    $(document).on('click', '.edit-encryptedImportantInformation', function (e) {
        var Id = $(this).attr("data-encryptedImportantInformation-id");

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Personal/EncryptedImportantInformationEditModal?Id=' + Id,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#EncryptedImportantInformationEditModal div.modal-content').html(content);
            },
            error: function (e) { }
        })
    });

    if (userIsEasyLifeAdmin) {
        $('#FilterUserId').on("change", function () {
            _$EncryptedImportantInformationTable.ajax.reload();
        });
    }

    //ConinDCX calling API
    //const request = require('request')
    //const crypto = require('crypto')

    //const baseurl = "https://api.coindcx.com"

    //const timeStamp = Math.floor(Date.now());

    //// Place your API key and secret below. You can generate it from the website.
    //const key = "9576c28a3088da28af3c8a0e5fae8281b4054ec5d90af62f";
    //const secret = "43ef343f6a445bba11aadd5b699bdb05c8e7fea90b0fa1dea8d3152f82d02621";


    //const body = {
    //    "side": "buy",  //Toggle between 'buy' or 'sell'.
    //    "order_type": "limit_order", //Toggle between a 'market_order' or 'limit_order'.
    //    "market": "SNTBTC", //Replace 'SNTBTC' with your desired market pair.
    //    "price_per_unit": "0.03244", //This parameter is only required for a 'limit_order'
    //    "total_quantity": 400, //Replace this with the quantity you want
    //    "timestamp": timeStamp
    //}

    //const payload = new Buffer(JSON.stringify(body)).toString();
    //const signature = crypto.createHmac('sha256', secret).update(payload).digest('hex')

    //const options = {
    //    url: baseurl + "/exchange/v1/orders/create",
    //    headers: {
    //        'X-AUTH-APIKEY': key,
    //        'X-AUTH-SIGNATURE': signature
    //    },
    //    json: true,
    //    body: body
    //}

    //request.post(options, function (error, response, body) {
    //    debugger;
    //    console.log(body);
    //})


    const request = require('request')

    const baseurl = "https://public.coindcx.com"

    // Replace the "B-BTC_USDT" with the desired market pair.
    request.get(baseurl + "/market_data/orderbook?pair=B-BTC_USDT", function (error, response, body) {
        debugger;
        console.log(body);
    })
})(jQuery);