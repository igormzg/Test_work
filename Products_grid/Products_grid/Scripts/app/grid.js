
$(document).ready(function () {
    $("#jsonmap").jqGrid({
        url: '/Home/GetProducts',
        datatype: "json",
        colNames: ['Actions', 'Name', 'Price'],
        colModel: [
            { name: 'act', index: 'act', sortable: false },
            { name: 'Name', index: 'Name', width: 90, editable: true },
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
        gridComplete: function () {
            var ids = jQuery("#jsonmap").jqGrid('getDataIDs');
            for (var i = 0; i < ids.length; i++) {
                var cl = ids[i];
                be = "<input type='button' value='Edit' onclick=\"$('#jsonmap').editRow('" + cl + "');\"  />";
                se = "<input type='button' value='Save' onclick=\"$('#jsonmap').saveRow('" + cl + "');\"  />";
                ce = "<input type='button' value='Cancel' onclick=\"$('#jsonmap').restoreRow('" + cl + "');\" />";
                $("#jsonmap").jqGrid('setRowData', ids[i], { act: be + se + ce });
            }
        },
    });
    $("#jsonmap").jqGrid('navGrid', '#pjmap', { edit: false, add: false, del: false });




    window.PG.gridx.init();
});


window.PG.gridx = {
    init: function () {
        $('#edit-row-btn').on('click', window.PG.gridx.editRowClk);
        //Подписатся на клик
    },
    editRowClk: function () {
        var rowId = $("#jsonmap").jqGrid('getGridParam', 'selrow');   
    }
}