/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('WebMain'); 

WebMain.BillListAction = function() {  
    WebMain.BillListAction.initializeBase(this);  
}

WebMain.BillListAction.prototype = {
    initialize: function () {
        WebMain.BillListAction.callBaseMethod(this, 'initialize');
    },

    dispose: function () {
        WebMain.BillListAction.callBaseMethod(this, 'dispose');
    },

    doGridSelectedChanged: function (sender) {
        var form = sender.get_form();
        var rowData = form.grid.get_selectedRowData();
        if (!rowData) {
            form.gridDetail.dataBind(null);
            return;
        }
        form.get_service().GetDetail(rowData.id, function (data) {
            form.gridDetail.dataBind(data);
        });
    },

    doExport: function (sender) {
        try {
            var form = sender.get_form();
            if (JsObj && JsObj.export) {
                JsObj.export(form.user.get_value().toString(), form.area.get_value().toString());
            } else {
                alert('ÏµÍ³´íÎó')
            }
        } catch (e) {
            alert(e);
        }
    }
}
WebMain.BillListAction.registerClass('WebMain.BillListAction', Sys.UI.PageAction);
