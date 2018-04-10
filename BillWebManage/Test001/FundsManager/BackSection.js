/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.FundsManager'); 

Test001.FundsManager.BackSectionAction = function() {  
    Test001.FundsManager.BackSectionAction.initializeBase(this);  
}

Test001.FundsManager.BackSectionAction.prototype = {  
    initialize: function() {  
        Test001.FundsManager.BackSectionAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.FundsManager.BackSectionAction.callBaseMethod(this, 'dispose');  
    },

    doBackSection: function(sender) {
        var form = new Sys.UI.Form(sender);
        form.add_closed(function () {
            sender.get_form().grid.get_pager().refresh([]);
        });
        form.showModal("BackSectionManager.gspx");
    },

    raiseExpandChange: function (sender, eventArgs) {
        if (!eventArgs.get_value()) return;
        var form = this.get_form();

        var rowData = this.get_form().grid.getRowData(eventArgs.get_rowIndex());
        eventArgs.set_pageUrl("BackSectionDetailList.gspx?month=" + rowData.month);
    }
}
Test001.FundsManager.BackSectionAction.registerClass('Test001.FundsManager.BackSectionAction', Sys.UI.PageAction);
