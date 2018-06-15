/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.DataHandler'); 

Test001.DataHandler.BillListAction = function() {  
    Test001.DataHandler.BillListAction.initializeBase(this);  
}

Test001.DataHandler.BillListAction.prototype = {
    initialize: function () {
        Test001.DataHandler.BillListAction.callBaseMethod(this, 'initialize');
    },

    dispose: function () {
        Test001.DataHandler.BillListAction.callBaseMethod(this, 'dispose');
    },
    
    doGridSelectedChanged: function(sender) {
        var form = sender.get_form();
        var rowData = form.grid.get_selectedRowData();
        if (!rowData) {
            form.gridDetail.dataBind(null);
            return;
        }
        var array = [];
        var detail = form.data.detail;
        for (var i = 0; i < detail.length; i++) {
            if (detail[i].bid == rowData.id) {
                array.push(detail[i]);
            }
        }
        form.gridDetail.dataBind(array);
    },

    doRowRending: function (sender, args) {
        var rowIndex = args.get_rowIndex();
        var rowData = sender.findRowData(rowIndex);
        if (!rowData) return;
        var color = "";
        switch (rowData.status) {
            case 0:
                color = "#000000";
                args.set_bgColor("#E35850");
                break;
            case 1:
                color = "#000000";
                args.set_bgColor("#80bbfa");
                break;
            case 2:
                color = "#F213C8";
                break;
            case 3:
                color = "#5A3222";
                break;
            case 4:
                color = "red";
                break;
        }
        args.set_fontColor(color);
    }
}
Test001.DataHandler.BillListAction.registerClass('Test001.DataHandler.BillListAction', Sys.UI.PageAction);
