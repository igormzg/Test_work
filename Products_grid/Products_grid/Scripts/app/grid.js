window.PG = {};

$(document).ready(function () {
    $("#prod-grid").jqGrid({
        url: '/Home/GetProducts',
        datatype: "json",
        colNames: ['Picker', 'Id', 'Count', 'Name', 'Price'],
        colModel: [
            { width: 40, editable: false },
            { name: 'Id', index: 'Id', hidedlg: true },
            { name: 'Count', index: 'Count', hidedlg: true },
            { name: 'Name', index: 'Name', width: 150, editable: true },
            { name: 'Price', index: 'Price', width: 100, editable: true }
        ],
        rowNum: 10,
        rowList: [10, 20, 30],
        pager: '#pjmap',
        sortname: 'Name',
        viewrecords: true,
        sortorder: "desc",
        jsonReader: {
            repeatitems: false,
            id: "0"
        },
        caption: "Products",
        height: '100%',
        onSelectRow: $.proxy(window.PG.gridx.rowSelected, window.PG.gridx)
    });
    $("#prod-grid").jqGrid('navGrid', '#pjmap', { edit: false, add: false, del: false });
    $('#prod-grid').hideCol('Id');
    $('#prod-grid').hideCol('Count');
    window.PG.gridx.init();
    window.PG.operationDialog.init();
});


window.PG.gridx = {

    init: function () {
        $('#edit-rows-btn').on('click', $.proxy(this.editRowsClk, this ));
        $('#save-rows-btn').on('click', $.proxy(this.saveRowsClk, this));
        $('#cancel-rows-btn').on('click', $.proxy(this.cancelRowsClk, this));
        $('#delete-rows-btn').on('click', $.proxy(this.deleteRowsClk, this));
    },

    editRowsClk: function () {
        var rowIndex = $("#prod-grid").jqGrid('getGridParam', 'selrow');
        $('#prod-grid').editRow(rowIndex);
        this.editBtnState(true);
    },

    saveRowsClk: function () {
        var rowIndex = $("#prod-grid").jqGrid('getGridParam', 'selrow');
     //   $('#prod-grid').saveRow(rowIndex);

        id = $("#prod-grid").jqGrid('getCell', rowIndex, 'Id');
        name = $("#prod-grid").jqGrid('getCell', rowIndex, 'Name');
        price = $("#prod-grid").jqGrid('getCell', rowIndex, 'Price');

        $.ajax({
            type: 'POST',
            url: '/Home/EditProduct/',
            dataType: 'json',
            data: {
                Id: id,
                Name: name,
                Price: price
            },
            success: function () {
                $('#prod-grid').trigger('reloadGrid');
            },
            error: function (err) {
                alert(err.toString());
            }
        });
        $('#prod-grid').trigger('reloadGrid');

        this.editBtnState(false);
    },

    cancelRowsClk: function () {
        var rowIndex = $("#prod-grid").jqGrid('getGridParam', 'selrow');
        $('#prod-grid').restoreRow(rowIndex);
        this.editBtnState(false);
    },

    deleteRowsClk: function () {
        var rowIndex = $('#prod-grid').jqGrid('getGridParam', 'selrow');
        id = $("#prod-grid").jqGrid('getCell', rowIndex, 'Id');
        
        if (confirm("Are you sure?")) {
            $.ajax({
                type: 'POST',
                url: '/Home/DeleteProduct',
                dataType: 'json',
                data: {
                    Id: id,
                },
                success: function () {
                    $('#prod-grid').trigger('reloadGrid');
                },
                error: function (err) {
                    alert(err.toString());
                }
            });
            $('#prod-grid').trigger('reloadGrid');
            this.editBtnState(false);
        }
    },

    rowSelected: function (index) {
        if ($('tr#' + index).attr('editable') === '1') {
            window.PG.gridx.editBtnState(true);
        }
        else {
            window.PG.gridx.editBtnState(false);
        }

        this.redrawStatisticSection();
        $('#add-operation-btn').css('display', 'inline-block')
    },

    editBtnState: function (edit) {
        if (edit == true) {
            $("#save-rows-btn").css('display', 'inline-block');
            $('#cancel-rows-btn').css('display', 'inline-block');
            $('#delete-rows-btn').css('display', 'none');
            $('#edit-rows-btn').css('display', 'none');
        }
        else {
            $("#save-rows-btn").css('display', 'none');
            $('#cancel-rows-btn').css('display', 'none');
            $('#delete-rows-btn').css('display', 'inline-block');
            $('#edit-rows-btn').css('display', 'inline-block');
        }
    },

    redrawStatisticSection: function () {
        $('#product-statistic').css('display', 'inline-block')

        var rowIndex = $("#prod-grid").jqGrid('getGridParam', 'selrow');
        var name = $("#prod-grid").jqGrid('getCell', rowIndex, 'Name');
        var price = $("#prod-grid").jqGrid('getCell', rowIndex, 'Price');
        var count = $("#prod-grid").jqGrid('getCell', rowIndex, 'Count');

        $('#product-statistic spam').empty();
        $('#product-statistic #stat-name').append(name);
        $('#product-statistic #stat-count').append(count);
        $('#product-statistic #stat-price').append(price);
        $('#product-statistic #stat-tprice').append(price * count);
    }
},

window.PG.operationDialog = {
    init: function () {
        $('#add-operation-btn').on('click', $.proxy(this.showDialog, this));
    },
    showDialog: function () {
        $("#add-operation-dialog").dialog({
            title: 'Add Operation',
            modal: true,
            resizable: false,
            width: "auto",
            buttons: {
                Save: function () {
                    $(this).dialog('close');
                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
        this.setFields();
        
    },
    setFields: function () {
        var rowIndex = $("#prod-grid").jqGrid('getGridParam', 'selrow');

        id = $("#prod-grid").jqGrid('getCell', rowIndex, 'Id');
        name = $("#prod-grid").jqGrid('getCell', rowIndex, 'Name');
        price = $("#prod-grid").jqGrid('getCell', rowIndex, 'Price');

        $('#add-operation-dialog #name').empty();
        $('#add-operation-dialog #name').append(name);
        //$('#add-operation-dialog #type').append('Name: ' + name);
        $('#add-operation-dialog #count').val(price);

    }
}