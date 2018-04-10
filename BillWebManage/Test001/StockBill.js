/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.StockBillAction = function() {  
    Test001.StockBillAction.initializeBase(this);  
}

Test001.StockBillAction.prototype = {  
    initialize: function() {  
        Test001.StockBillAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.StockBillAction.callBaseMethod(this, 'dispose');  
    },

    doSelected: function(sender) {
        var form = sender.get_form();
        var data = {
            kind: form.kind.get_value()
        };
        form.grid.get_pager().refresh(data);
        form.grid.findColumn("ctel").set_visible(sender.get_value() == 2);
        form.grid.findColumn("process").set_visible(sender.get_value() == 0);
    },

    doGridLoaded: function(sender) {
        var form = sender.get_form();
        if (form.kind.get_value() != 2) {
            return;
        }
        var data = form.grid.get_dataSource();
        form.get_service().GetTemp(data, function (rt) {
            for (var i = 0 ; i < data.length ; i++) {
                for (var j = 0 ; j < rt.length ; j++) {
                    if (rt[j].id == data[i].bid) {
                        data[i].ctel = rt[j].ctel;
                    }
                }
                form.grid.modifyRowData(i, data[i]);
            }
        });
    },

    doSendClick: function(sender) {
        var form = sender.get_form();
        var data = form.grid.get_selectedRowData();
        if (!data || data.process == '') {
            return;
        }
        form.get_service().ToReady(data.id, function () {
            data.process = '';
            form.grid.modifySelectedRowData(data);
        });
    }
}
Test001.StockBillAction.registerClass('Test001.StockBillAction', Sys.UI.PageAction);
