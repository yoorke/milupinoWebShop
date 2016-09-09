$(document).ready(function () {
    $('#dtpDateFrom').datetimepicker({
        format: 'd.m.Y',
        timepicker: false,
        defaultDate: new Date()
    });

    $('#dtpDateTo').datetimepicker({
        format: 'd.m.Y',
        timepicker: false,
        defaultDate: new Date()
    });

    var date = new Date();
    date.setDate(1);
    date.setMonth(date.getMonth() - 12);
    $('#dtpDateFrom').val($.datepicker.formatDate('dd.mm.yy', date));

    $('#dtpDateTo').val($.datepicker.formatDate('dd.mm.yy', new Date()));

    $('#chkDay, #chkMonth').change(ShowOrders);

    ShowOrders();
})

function ShowOrders()
{
    if ($('#chkDay').prop('checked')) {
        GetNumberOfOrdersByDay(ChangeDateFormat($('#dtpDateFrom').val().toString()), ChangeDateFormat($('#dtpDateTo').val().toString()));
        GetValuesByDay(ChangeDateFormat($('#dtpDateFrom').val().toString()), ChangeDateFormat($('#dtpDateTo').val().toString()));
        GetCategoryPercentage(ChangeDateFormat($('#dtpDateFrom').val().toString()), ChangeDateFormat($('#dtpDateTo').val().toString()));
        GetCategoryValue(ChangeDateFormat($('#dtpDateFrom').val().toString()), ChangeDateFormat($('#dtpDateTo').val().toString()));
    }
    else {
        GetNumberOfOrdersByMonth(ChangeDateFormat($('#dtpDateFrom').val().toString()), ChangeDateFormat($('#dtpDateTo').val().toString()));
        GetValuesByMonth(ChangeDateFormat($('#dtpDateFrom').val().toString()), ChangeDateFormat($('#dtpDateTo').val().toString()));
        GetCategoryPercentage(ChangeDateFormat($('#dtpDateFrom').val().toString()), ChangeDateFormat($('#dtpDateTo').val().toString()));
        GetCategoryValue(ChangeDateFormat($('#dtpDateFrom').val().toString()), ChangeDateFormat($('#dtpDateTo').val().toString()));
    }

}


function GetNumberOfOrdersByDay(dateFrom, dateTo) {
    var data;
    $('#chartNumberOfOrdersPleaseWait').show();
    $.ajax({
        type: "POST",
        url: "../WebMethods.aspx/GetNumberOfOrdersByDay",
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //alert(JSON.parse(msg.d));
            $('#chartNumberOfOrders').empty();
            data = JSON.parse((msg.d));

            Morris.Line({
                element: 'chartNumberOfOrders',
                data: data,
                parseTime: true,
                xkey: 'date',
                ykeys: ['count'],
                labels: ['Broj porudžbina'],
                resize: true,
                smooth: false
            });
            $('#chartNumberOfOrdersPleaseWait').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
        //async: false
    })
    //return data;
}

function GetNumberOfOrdersByMonth(dateFrom, dateTo) {
    var data;
    $('#chartNumberOfOrdersPleaseWait').show();
    $.ajax({
        type: "POST",
        url: "../WebMethods.aspx/GetNumberOfOrdersByMonth",
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (msg) {
            $('#chartNumberOfOrders').empty();
            data = JSON.parse(msg.d);

            Morris.Line({
                element: 'chartNumberOfOrders',
                data: data,
                parseTime: false,
                xkey: 'date',
                ykeys: ['count'],
                labels: ['Broj porudžbina'],
                resize: true,
                smooth: true
            });
            $('#chartNumberOfOrdersPleaseWait').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    })
}

function GetValuesByMonth(dateFrom, dateTo) {
    var data;
    $('#chartValuesPleaseWait').show();
    $.ajax({
        type: "POST",
        url: "../WebMethods.aspx/GetValuesByMonth",
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (msg) {
            data = JSON.parse(msg.d);
            $('#chartValues').empty();

            Morris.Line({
                element: 'chartValues',
                data: data,
                parseTime: false,
                xkey: 'date',
                ykeys: ['value'],
                labels: ['Iznos'],
                resize: true
            });
            $('#chartValuesPleaseWait').hide();
        },
        error: function(jqXHR, textStatus, errorThrown){
            alert(jqXHR.responseText);
        }
        //async: false
    });
//return data;
}

function GetValuesByDay(dateFrom, dateTo) {
    var data;
    $('#chartValuesPleaseWait').show();
    $.ajax({
        type: "POST",
        url: "../WebMethods.aspx/GetValuesByDay",
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (msg) {
            data = JSON.parse(msg.d);
            $('#chartValues').empty();

            Morris.Line({
                element: 'chartValues',
                data: data,
                parseTime: true,
                xkey: 'date',
                ykeys: ['value'],
                labels: ['Iznos'],
                resize: true
            });
            $('#chartValuesPleaseWait').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
        //async: false
    });
    //return data;
}

function GetCategoryPercentage(dateFrom, dateTo) {
    var data;
    $('#chartCategoryPercentagePleaseWait').show();
    $.ajax({
        type: "POST",
        url: "../WebMethods.aspx/GetCategoryPercentage",
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (msg) {
            data = JSON.parse(msg.d);
            InsertColor(data);
            $('#chartCategoryPercentage').empty();

            var ctx = $('#chartCategoryPercentage').get(0).getContext('2d');
            var categoryChart = new Chart(ctx).Pie(data, {});
            legend(document.getElementById('chartCategoryPercentageLegend'), data);
            $('#chartCategoryPercentagePleaseWait').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    })
}

function GetCategoryValue(dateFrom, dateTo) {
    var data;
    $('#chartCategoryValuePleaseWait').show();
    $.ajax({
        type: "POST",
        url: "../WebMethods.aspx/GetCategoryValue",
        data: JSON.stringify({ "dateFrom": dateFrom, "dateTo": dateTo }),
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (msg) {
            data = JSON.parse(msg.d);
            InsertColor(data);
            $('#chartCategoryValue').empty();

            var ctx = $('#chartCategoryValue').get(0).getContext('2d');
            var categoryChart = new Chart(ctx).Pie(data);
            legend(document.getElementById('chartCategoryValueLegend'), data);
            $('#chartCategoryValuePleaseWait').hide();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    })
}

function InsertColor(data) {
    var colors = ['#4d4d4d', '#5da5da', '#faa43a', '#60bd68', '#F17CB0', '#B2912F', '#B276B2', '#DECF3F', '#F15854'];
    var j = 0;
    for (var i = 0; i < data.length; i++) {
        data[i].color = colors[j];
        data[i].highlight = colors[j];
        if (j == colors.length - 1)
            j = 0;
    }
}