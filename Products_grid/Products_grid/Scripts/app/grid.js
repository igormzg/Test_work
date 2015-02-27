window.PG = {};

$(document).ready(function () {
    $("#prod-grid").jqGrid({
        url: '/Home/GetProducts',
        datatype: "json",
        colNames: ['', 'Id', 'Count', 'Name', 'Price'],
        colModel: [
            { width: 20, editable: false },
            { name: 'Id', index: 'Id', hidden: true },
            { name: 'Count', index: 'Count', hidden: true },
            { name: 'Name', index: 'Name', width: 190, editable: true },
            { name: 'Price', index: 'Price', width: 190, editable: true }
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

    window.PG.gridx.init();
    window.PG.operationDialog.init();
    window.PG.operationGridDialog.init();
});


window.PG.gridx = {

    init: function () {
        $('#add-rows-btn').on('click', $.proxy(this.addRowClk, this));
        $('#edit-rows-btn').on('click', $.proxy(this.editRowsClk, this ));
        $('#save-rows-btn').on('click', $.proxy(this.saveRowsClk, this));
        $('#cancel-rows-btn').on('click', $.proxy(this.cancelRowsClk, this));
        $('#delete-rows-btn').on('click', $.proxy(this.deleteRowsClk, this));
    },

    addRowClk: function () {
        $("#add-product-dialog").dialog({
            title: 'Add Product',
            modal: true,
            resizable: false,
            width: "auto",
            buttons: {
                Ok: function () {
                    $('#add-product-form').submit();
                    $(this).dialog('close');
                },
                Cancel: function () {
                    $(this).dialog('close');
                }
            }
        });
    },

    editRowsClk: function () {
        var rowIndex = this.selectedRow;
        $('#prod-grid').editRow(rowIndex);
        this.editBtnState(true);
    },

    saveRowsClk: function () {
        var rowIndex = this.selectedRow;

        id = $("#prod-grid").jqGrid('getCell', rowIndex, 'Id');
        name = $("#prod-grid").jqGrid('getCell', rowIndex, 'Name');
        price = $("#prod-grid").jqGrid('getCell', rowIndex, 'Price');

        saveparameters = {
            "successfunc": null,
            "url": '/Home/EditProduct/',
            "extraparam": {ProductId: id},
            "aftersavefunc": null,
            "errorfunc": null,
            "afterrestorefunc": null,
            "restoreAfterError": true,
            "mtype": "POST"
        }

        jQuery('#prod-grid').jqGrid('saveRow', rowIndex, saveparameters);
    },

    cancelRowsClk: function () {
        var rowIndex = this.selectedRow;
        $('#prod-grid').restoreRow(rowIndex);
        this.editBtnState(false);
    },

    deleteRowsClk: function () {
        var rowIndex = this.selectedRow;
        var id = $("#prod-grid").jqGrid('getCell', rowIndex, 'Id');
        
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

    rowSelected: function (index, selected) {
        this.selectedRow = $('#prod-grid').jqGrid('getGridParam', 'selrow');

        if ($('tr#' + index).attr('editable') === '1') {
            window.PG.gridx.editBtnState(true);
        }
        else {
            window.PG.gridx.editBtnState(false);
        }

        if (selected === false)
        {
            $("#save-rows-btn").css('display', 'none');
            $('#cancel-rows-btn').css('display', 'none');
            $('#delete-rows-btn').css('display', 'none');
            $('#edit-rows-btn').css('display', 'none');
            $('#add-operation-btn').css('display', 'none');
            $('#view-oper-grid-btn').css('display', 'none');
            this.redrawStatisticSection(false);
            return;
        }

        this.redrawStatisticSection();
    },

    editBtnState: function (edit) {
        if (edit == true) {
            $("#save-rows-btn").css('display', 'inline-block');
            $('#cancel-rows-btn').css('display', 'inline-block');
            $('#delete-rows-btn').css('display', 'none');
            $('#edit-rows-btn').css('display', 'none');
            $('#add-operation-btn').css('display', 'none');
            $('#view-oper-grid-btn').css('display', 'none');
        }
        else {
            $("#save-rows-btn").css('display', 'none');
            $('#cancel-rows-btn').css('display', 'none');
            $('#delete-rows-btn').css('display', 'inline-block');
            $('#edit-rows-btn').css('display', 'inline-block');
            $('#add-operation-btn').css('display', 'inline-block');
            $('#view-oper-grid-btn').css('display', 'inline-block');
        }
    },

    redrawStatisticSection: function (state) {
        if (state === false) {
            $('#product-statistic').css('display', 'none');
            return;
        }
        $('#product-statistic').css('display', 'inline-block');

        var rowIndex = $("#prod-grid").jqGrid('getGridParam', 'selrow');
        var name = $("#prod-grid").jqGrid('getCell', rowIndex, 'Name');
        var price = $("#prod-grid").jqGrid('getCell', rowIndex, 'Price');
        var count = $("#prod-grid").jqGrid('getCell', rowIndex, 'Count');

        $('#product-statistic .statistic-value').empty();
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
                    $.proxy(window.PG.operationDialog.saveBtnClk(), window.PG.operationDialog);
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

        this.id = $("#prod-grid").jqGrid('getCell', rowIndex, 'Id');
        name = $("#prod-grid").jqGrid('getCell', rowIndex, 'Name');

        $('#add-operation-dialog #name').empty();
        $('#add-operation-dialog #name').append(name);

        $('#add-operation-dialog #operation-type').empty();
        $('#add-operation-dialog #operation-type').append('<option selected="selected">Coming</option>');
        $('#add-operation-dialog #operation-type').append('<option>Consumption</option>');
    },

    saveBtnClk: function () {
        var count = $('#add-operation-dialog #count').attr('value');
        var type = $('#add-operation-dialog #operation-type').attr('value');
        if (type == 'Coming') type = true;
        else type = false;

        $.ajax({
            type: 'POST',
            url: '/Home/AddOperation',
            dataType: 'json',
            data: {
                id: this.id,
                count: count,
                type: type
            },
            success: function () {
                $('#prod-grid').trigger('reloadGrid');
                setTimeout(function(){
                    $("#prod-grid").jqGrid('setSelection', window.PG.gridx.selectedRow);
                }, 300);
            },
            error: function (err) {
                alert(err.toString());
            }
        });
    }
},

window.PG.operationGridDialog = {
    init: function () {
        $('#view-oper-grid-btn').on('click', $.proxy(this.showDialog, this));
        this.setGrid();
    },

    showDialog: function () {
        $("#operations-grid-dialog").dialog({
            title: 'Add Operation',
            modal: true,
            resizable: false,
            width: "auto"
        });
        var rowIndex = $('#prod-grid').jqGrid('getGridParam', 'selrow');
        var prod = $("#prod-grid").jqGrid('getCell', rowIndex, 'Id');

        $("#operation-grid").jqGrid('setGridParam', { url: '/Home/GetOperations?prodId=' + prod });
        $('#operation-grid').trigger('reloadGrid');
    },

    setGrid: function () {
        $("#operation-grid").jqGrid({
            url: '/Home/GetOperations',
            datatype: "json",
            colNames: ['User', 'Type', 'Count', 'Date'],
            colModel: [
                { name: 'User', index: 'User', sortable: true },
                { name: 'Type', index: 'Type', width: 150, sortable: true },
                { name: 'Count', index: 'Count', width: 100, sortable: true },
                { name: 'Date', index: 'Date', width: 200, sortable: true }
            ],
            rowNum: 10,
            rowList: [10, 20, 30],
            pager: '#ogmap',
            sortname: 'User',
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
        $("#prod-grid").jqGrid('navGrid', '#ogmap', { edit: false, add: false, del: false });
    }
}
