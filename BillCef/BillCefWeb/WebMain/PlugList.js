/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('WebMain'); 

WebMain.PlugListAction = function() {  
    WebMain.PlugListAction.initializeBase(this);  
}

WebMain.PlugListAction.prototype = {
    initialize: function () {
        WebMain.PlugListAction.callBaseMethod(this, 'initialize');
    },

    dispose: function () {
        WebMain.PlugListAction.callBaseMethod(this, 'dispose');
    },

    doSearch: function (sender) {

    },

    doAdd: function (sender) {
        var form = sender.get_form();
        var mForm = new Sys.UI.Form();
        mForm.add_ok(function () {
            form.grid.refresh();
        });
        mForm.showModal("PlugManager.gspx");
    },

    doStop: function (sender) {

    },

    doRowDbClick: function (sender) {
        var form = sender.get_form();
        var grid = form.grid;
        var data = grid.get_selectedRowData();
        if (!data) {
            return;
        }
        var mForm = new Sys.UI.Form();
        mForm.add_ok(function () {
            grid.refresh();
        });
        mForm.showModal("PlugManager.gspx?id=" + data.pid);
    }
}
WebMain.PlugListAction.registerClass('WebMain.PlugListAction', Sys.UI.PageAction);
