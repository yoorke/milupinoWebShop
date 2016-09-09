function GetProductsFromKimtec(){
    $('#ctl00_ContentPlaceHolder1_divPleaseWait').show();
    $.ajax({
        type: "POST",
        url: "/webshopAdmin/WebMethods.aspx/GetProductsFromKimtec",
        data:"",
        contentType:"application/json;charset=utf-8",
        dataType:"json",
        success: function(msg){
            //alert(JSON.parse(msg.d));
            $('#ctl00_ContentPlaceHolder1_divPleaseWait').hide();
            $('#messageBox').show();
            $('#messageBoxText')[0].innerHTML = JSON.parse(msg.d);
        },
        error: function(jqXHR, textStatus, errorThrown){
            alert(jqXHR.responseText);
        }
    });
}

function GetProductsSpecificationFromKimtec(){
    $('#ctl00_ContentPlaceHolder1_divPleaseWait').show();
    $.ajax({
        type: "POST",
        url: "/webshopAdmin/WebMethods.aspx/GetProductsSpecificationFromKimtec",
        data: "",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function(msg){
            $('#ctl00_ContentPlaceHolder1_divPleaseWait').hide();
            $('#messageBox').show();
            $('#messageBoxText')[0].innerHTML = JSON.parse(msg.d);
        },
        error: function(jqXHR, textStatus, errorThrown){
            alert(jqXHR.responseText);
            $('#ctl00_ContentPlaceHolder1_divPleaseWait').hide();
        }
    });
}

function GetCategoriesFromKimtec(){
    $('#ctl00_ContentPlaceHolder1_divPleaseWait').show();
    $.ajax({
        type: "POST",
        url: "/webshopAdmin/WebMethods.aspx/GetCategoriesFromKimtec",
        data: "",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function(msg){
            $('#ctl00_ContentPlaceHolder1_divPleaseWait').hide();
            $('#messageBox').show();
            $('#messageBoxText')[0].innerHTML = JSON.parse(msg.d);
        },
        error: function(jqXHR, textStatus, errorThrown){
            alert(jqXHR.responseText);
            $('#ctl00_ContentPlaceHolder1_divPleaseWait').hide();
        }
    });
}

function btnMessageBoxClose_Click(){
    $('#messageBox').hide();
}

function SaveProduct(code, isApproved, isActive, categoryID) {
    $.ajax({
        type: "POST",
        url: '/webshopAdmin/WebMethods.aspx/SaveProduct',
        data: JSON.stringify({ 'code': code, 'isApproved': isApproved, 'isActive': isActive, 'categoryID': parseInt(categoryID) }),
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (msg) {
            //alert(msg);
            //return 1;
            SetSaveStatus(++saveProductsCurrent, saveProductsCount);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //alert(jqXHR.responseText);
            $('#errorStatus')[0].innerText += JSON.parse(jqXHR.responseText).Message + "\n";
            $('#errorStatus').show();
        }
    })
}

function SaveProductKimtec(code, isApproved, isActive, categoryID) {
    var kimtecCategoryID = $('#ctl00_ContentPlaceHolder1_cmbKimtecCategory').val();
    $.ajax({
        type: 'POST',
        url: '/webshopAdmin/WebMethods.aspx/SaveProductKimtec',
        data: JSON.stringify({ 'code': code, 'isApproved': isApproved, 'isActive': isActive, 'categoryID': parseInt(categoryID), 'kimtecCategoryID': kimtecCategoryID }),
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (msg) {
            SetSaveStatus(++saveProductsCurrent, saveProductsCount);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#errorStatus')[0].innerText += JSON.parse(jqXHR.responseText).Message + '\n';
            $('#errorStatus').show();
        }
    })
}

var saveProductsCount = 0;
var saveProductsCurrent = 0;

function btnSaveProducts_Click(type) {
    var i = 0;
    var isApproved = $('#ctl00_ContentPlaceHolder1_chkApproved')[0].checked;
    var isActive = $('#ctl00_ContentPlaceHolder1_chkActive')[0].checked;
    var categoryID = $('#ctl00_ContentPlaceHolder1_cmbCategory').val();
    //var count = 0;
    //var current = 0;
    saveProductsCount = 0;
    $('#ctl00_ContentPlaceHolder1_dgvProducts > tbody > tr').each(function () {
        if(i++ > 0)
        if (this.cells[0].children[0].children[0].checked)
            saveProductsCount++;
    })
    i = 0;
    //$('#saveStatus').innerText = current + '/' + count;
    $('#saveStatus').show();
    $('#errorStatus').hide();
    
    saveProductsCurrent = 0;
    SetSaveStatus(0, saveProductsCount);
    $('#ctl00_ContentPlaceHolder1_dgvProducts > tbody > tr').each(function () {
        if (i++ > 0)
            if (this.cells[0].children[0].children[0].checked) {
                var code = this.cells[1].innerText;
                type == 'ewe' ? SaveProduct(code, isApproved, isActive, categoryID) : SaveProductKimtec(code, isApproved, isActive, categoryID);
                //$('#saveStatus')[0].innerText = ++current + '/' + count;
            }
    })
    //$('#saveStatus')[0].innerText = "Sačuvano" + saveProductsCount + " proizvoda";
}

function SetSaveStatus(current, count) {
    if (current < count)
        $('#saveStatus')[0].innerText = current + '/' + count;
    else
        $('#saveStatus')[0].innerText = "Sačuvano " + count + " proizvoda";
}