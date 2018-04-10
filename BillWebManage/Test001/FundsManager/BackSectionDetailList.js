/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.FundsManager'); 

Test001.FundsManager.BackSectionDetailListAction = function() {  
    Test001.FundsManager.BackSectionDetailListAction.initializeBase(this);  
}

Test001.FundsManager.BackSectionDetailListAction.prototype = {  
    initialize: function() {  
        Test001.FundsManager.BackSectionDetailListAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.FundsManager.BackSectionDetailListAction.callBaseMethod(this, 'dispose');  
    },

    doBackSection: function(sender) {
        var form = new Sys.UI.Form(sender);
        form.add_closed(function () {
            sender.get_form().grid.get_pager().refresh([]);
        });
        form.showModal("BackSectionDetailManager.gspx");
    }
}
Test001.FundsManager.BackSectionDetailListAction.registerClass('Test001.FundsManager.BackSectionDetailListAction', Sys.UI.PageAction);
